namespace CSEMcp.Tools;

/// <summary>
/// Contains all MCP tool descriptions and parameter descriptions for CSE MCP Server.
/// Centralized location for maintaining tool metadata.
/// </summary>
public static class ToolDescriptions
{
    // Tool Descriptions
    public const string GetStockQuote =
        "Get current stock quote for a CSE (Colombo Stock Exchange) stock. " +
        "Supports voting shares (.N0000) and non-voting shares (.X0000). " +
        "Provide the stock symbol (e.g., JKH, JKH.N0000, TESS.X0000). " +
        "Returns current price, change, volume, and market cap.";

    public const string GetCompanyProfile =
        "Get detailed company profile information for a CSE stock including sector, " +
        "contact details, and business information. Works with any stock symbol format.";

    public const string GetDayTrades =
        "Get today's trading data including individual trades with price, quantity, " +
        "and time for a CSE stock. Supports both voting and non-voting shares.";

    public const string ListCompanyStocks =
        "List all available stock types for a company on the CSE. " +
        "Some companies have both voting (.N0000) and non-voting (.X0000) shares. " +
        "Provide the base ticker symbol (e.g., COMB) to see all available stocks.";

    // Parameter Descriptions
    public const string SymbolParameter =
        "Stock symbol. Examples: 'JKH' (voting), 'JKH.N0000' (voting), 'TESS.X0000' (non-voting)";

    public const string TickerParameter =
        "Company ticker symbol without suffix (e.g., 'COMB', 'JKH')";

    public const string StockTypeParameter =
        "Optional stock type filter: 'voting', 'non-voting', or 'all' (default: 'all')";
}