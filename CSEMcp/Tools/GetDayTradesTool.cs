using System.ComponentModel;
using ModelContextProtocol.Server;
using CSEMcp.Infrastructure.ExternalServices;

namespace CSEMcp.Tools;

[McpServerToolType]
public class GetDayTradesTool
{
    private readonly CseDataService _cseDataService;
    private readonly ILogger<GetDayTradesTool> _logger;

    public GetDayTradesTool(CseDataService cseDataService, ILogger<GetDayTradesTool> logger)
    {
        _cseDataService = cseDataService;
        _logger = logger;
    }

    [McpServerTool]
    [Description(ToolDescriptions.GetDayTrades)]
    public async Task<DayTradesResult> GetDayTrades(
        [Description(ToolDescriptions.SymbolParameter)] string symbol,
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