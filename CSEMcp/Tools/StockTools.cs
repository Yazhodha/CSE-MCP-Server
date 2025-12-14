using System.ComponentModel;
using ModelContextProtocol.Server;
using CSEMcp.Infrastructure.ExternalServices;

namespace CSEMcp.Tools;

[McpServerToolType]
public class StockTools
{
    private readonly CseDataService _cseDataService;
    private readonly ILogger<StockTools> _logger;

    public StockTools(CseDataService cseDataService, ILogger<StockTools> logger)
    {
        _cseDataService = cseDataService;
        _logger = logger;
    }

    [McpServerTool]
    [Description("Get current stock quote for a CSE (Colombo Stock Exchange) stock. Provide the stock symbol (e.g., JKH, COMB) or with .CM suffix (e.g., JKH.CM) or CSE format (e.g., JKH.N0000). Returns current price, change, volume, and market cap.")]
    public async Task<StockQuoteResult> GetStockQuote(
        [Description("Stock symbol (e.g., 'JKH' or 'JKH.CM' or 'JKH.N0000' for John Keells Holdings)")] string symbol,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return new StockQuoteResult
                {
                    Error = "Symbol cannot be empty"
                };
            }

            _logger.LogInformation("Getting quote for symbol: {Symbol}", symbol);

            var quote = await _cseDataService.GetQuoteAsync(symbol, cancellationToken);

            if (quote == null)
            {
                return new StockQuoteResult
                {
                    Error = $"Unable to fetch quote for symbol: {symbol}. " +
                           "Please verify the symbol is correct (e.g., JKH, JKH.CM, or JKH.N0000 for CSE stocks)."
                };
            }

            return new StockQuoteResult
            {
                Symbol = quote.Symbol,
                Name = quote.Name,
                Price = quote.Price,
                Change = quote.Change,
                ChangePercent = Math.Round(quote.ChangePercent, 2),
                Volume = quote.Volume,
                MarketCap = quote.MarketCap,
                Currency = quote.Currency,
                Timestamp = quote.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing get_stock_quote tool");
            return new StockQuoteResult
            {
                Error = $"Error fetching stock quote: {ex.Message}"
            };
        }
    }

    [McpServerTool]
    [Description("Get detailed company profile information for a CSE stock including sector, contact details, and business information.")]
    public async Task<CompanyProfileResult> GetCompanyProfile(
        [Description("Stock symbol (e.g., 'JKH' or 'JKH.CM' or 'JKH.N0000')")] string symbol,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return new CompanyProfileResult
                {
                    Error = "Symbol cannot be empty"
                };
            }

            _logger.LogInformation("Getting company profile for symbol: {Symbol}", symbol);

            var profile = await _cseDataService.GetCompanyProfileAsync(symbol, cancellationToken);

            if (profile == null)
            {
                return new CompanyProfileResult
                {
                    Error = $"Unable to fetch company profile for symbol: {symbol}"
                };
            }

            return new CompanyProfileResult
            {
                Symbol = profile.Symbol,
                Name = profile.Name,
                Sector = profile.Sector,
                RegisteredOffice = profile.RegisteredOffice,
                Phone = profile.Phone,
                Email = profile.Email,
                Website = profile.Website,
                Established = profile.Established
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing get_company_profile tool");
            return new CompanyProfileResult
            {
                Error = $"Error fetching company profile: {ex.Message}"
            };
        }
    }

    [McpServerTool]
    [Description("Get today's trading data including individual trades with price, quantity, and time for a CSE stock.")]
    public async Task<DayTradesResult> GetDayTrades(
        [Description("Stock symbol (e.g., 'JKH' or 'JKH.CM' or 'JKH.N0000')")] string symbol,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return new DayTradesResult
                {
                    Error = "Symbol cannot be empty"
                };
            }

            _logger.LogInformation("Getting day's trades for symbol: {Symbol}", symbol);

            var trades = await _cseDataService.GetDayTradesAsync(symbol, cancellationToken);

            if (trades == null || trades.Count == 0)
            {
                return new DayTradesResult
                {
                    Symbol = symbol,
                    Trades = new List<TradeInfo>(),
                    TotalTrades = 0,
                    Message = "No trades available for today"
                };
            }

            // Convert to result format
            var tradeInfos = trades.Select(t => new TradeInfo
            {
                Price = t.Price,
                Quantity = t.Quantity,
                Time = t.Time,
                Change = t.Change,
                ChangePercentage = t.ChangePercentage
            }).ToList();

            return new DayTradesResult
            {
                Symbol = symbol,
                Trades = tradeInfos,
                TotalTrades = tradeInfos.Count,
                HighPrice = trades.Max(t => t.HiTrade),
                LowPrice = trades.Min(t => t.LowTrade),
                TotalVolume = trades.Sum(t => t.Quantity)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing get_day_trades tool");
            return new DayTradesResult
            {
                Error = $"Error fetching day's trades: {ex.Message}"
            };
        }
    }
}

public class StockQuoteResult
{
    public string? Symbol { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public decimal Change { get; set; }
    public decimal ChangePercent { get; set; }
    public long Volume { get; set; }
    public string? MarketCap { get; set; }
    public string Currency { get; set; } = "LKR";
    public string? Timestamp { get; set; }
    public string? Error { get; set; }
}

public class CompanyProfileResult
{
    public string? Symbol { get; set; }
    public string? Name { get; set; }
    public string? Sector { get; set; }
    public string? RegisteredOffice { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Established { get; set; }
    public string? Error { get; set; }
}

public class DayTradesResult
{
    public string? Symbol { get; set; }
    public List<TradeInfo> Trades { get; set; } = new();
    public int TotalTrades { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public long TotalVolume { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
}

public class TradeInfo
{
    public decimal Price { get; set; }
    public long Quantity { get; set; }
    public string? Time { get; set; }
    public decimal Change { get; set; }
    public decimal ChangePercentage { get; set; }
}