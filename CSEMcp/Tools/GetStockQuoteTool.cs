using System.ComponentModel;
using ModelContextProtocol.Server;
using CSEMcp.Infrastructure.ExternalServices;
using CSEMcp.Domain.ValueObjects;
using CSEMcp.Domain.Enums;

namespace CSEMcp.Tools;

[McpServerToolType]
public class GetStockQuoteTool
{
    private readonly CseDataService _cseDataService;
    private readonly ILogger<GetStockQuoteTool> _logger;

    public GetStockQuoteTool(CseDataService cseDataService, ILogger<GetStockQuoteTool> logger)
    {
        _cseDataService = cseDataService;
        _logger = logger;
    }

    [McpServerTool]
    [Description(ToolDescriptions.GetStockQuote)]
    public async Task<StockQuoteResult> GetStockQuote(
        [Description(ToolDescriptions.SymbolParameter)] string symbol,
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
                           "Please verify the symbol is correct (e.g., JKH, JKH.N0000 for voting, TESS.X0000 for non-voting)."
                };
            }

            // Determine stock type from symbol
            var stockSymbol = StockSymbol.Create(symbol);
            var stockType = stockSymbol.Type == StockType.Voting ? "Voting" : "Non-Voting";

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
                Timestamp = quote.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                StockType = stockType
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
}