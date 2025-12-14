using System.ComponentModel;
using ModelContextProtocol.Server;
using CSEMcp.Infrastructure.ExternalServices;
using CSEMcp.Domain.ValueObjects;
using CSEMcp.Domain.Enums;

namespace CSEMcp.Tools;

[McpServerToolType]
public class ListCompanyStocksTool
{
    private readonly CseDataService _cseDataService;
    private readonly ILogger<ListCompanyStocksTool> _logger;

    public ListCompanyStocksTool(CseDataService cseDataService, ILogger<ListCompanyStocksTool> logger)
    {
        _cseDataService = cseDataService;
        _logger = logger;
    }

    [McpServerTool]
    [Description(ToolDescriptions.ListCompanyStocks)]
    public async Task<CompanyStocksResult> ListCompanyStocks(
        [Description(ToolDescriptions.TickerParameter)] string ticker,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ticker))
            {
                return new CompanyStocksResult
                {
                    Error = "Ticker cannot be empty"
                };
            }

            // Remove any existing suffix to get base ticker
            var baseTicker = ticker.Split('.')[0].ToUpper();
            _logger.LogInformation("Listing stocks for company ticker: {Ticker}", baseTicker);

            var stocks = new List<StockInfo>();
            string? companyName = null;

            // Try to fetch voting shares (.N0000)
            try
            {
                var votingSymbol = StockSymbol.Create(baseTicker, StockType.Voting);
                var votingQuote = await _cseDataService.GetQuoteAsync(votingSymbol.Value, cancellationToken);

                if (votingQuote != null)
                {
                    companyName = votingQuote.Name;
                    stocks.Add(new StockInfo
                    {
                        Symbol = votingQuote.Symbol,
                        StockType = "Voting",
                        LastPrice = votingQuote.Price,
                        Change = votingQuote.Change,
                        ChangePercent = Math.Round(votingQuote.ChangePercent, 2),
                        IsActive = true
                    });
                    _logger.LogInformation("Found voting shares for {Ticker}: {Symbol}", baseTicker, votingQuote.Symbol);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "No voting shares found for {Ticker}", baseTicker);
            }

            // Try to fetch non-voting shares (.X0000)
            try
            {
                var nonVotingSymbol = StockSymbol.Create(baseTicker, StockType.NonVoting);
                var nonVotingQuote = await _cseDataService.GetQuoteAsync(nonVotingSymbol.Value, cancellationToken);

                if (nonVotingQuote != null)
                {
                    companyName ??= nonVotingQuote.Name;
                    stocks.Add(new StockInfo
                    {
                        Symbol = nonVotingQuote.Symbol,
                        StockType = "Non-Voting",
                        LastPrice = nonVotingQuote.Price,
                        Change = nonVotingQuote.Change,
                        ChangePercent = Math.Round(nonVotingQuote.ChangePercent, 2),
                        IsActive = true
                    });
                    _logger.LogInformation("Found non-voting shares for {Ticker}: {Symbol}", baseTicker, nonVotingQuote.Symbol);
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "No non-voting shares found for {Ticker}", baseTicker);
            }

            if (stocks.Count == 0)
            {
                return new CompanyStocksResult
                {
                    Ticker = baseTicker,
                    TotalStocks = 0,
                    Error = $"No stocks found for ticker: {baseTicker}. Please verify the ticker is correct."
                };
            }

            return new CompanyStocksResult
            {
                Ticker = baseTicker,
                CompanyName = companyName,
                Stocks = stocks,
                TotalStocks = stocks.Count,
                Message = stocks.Count > 1
                    ? $"Company has {stocks.Count} stock types: {string.Join(", ", stocks.Select(s => s.StockType))}"
                    : $"Company has 1 stock type: {stocks[0].StockType}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing list_company_stocks tool");
            return new CompanyStocksResult
            {
                Error = $"Error listing company stocks: {ex.Message}"
            };
        }
    }
}