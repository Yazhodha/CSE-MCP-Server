namespace CSEMcp.Tools;

/// <summary>
/// Result models for MCP tools
/// </summary>

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
    public string? StockType { get; set; } // "Voting" or "Non-Voting"
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

public class CompanyStocksResult
{
    public string? Ticker { get; set; }
    public string? CompanyName { get; set; }
    public List<StockInfo> Stocks { get; set; } = new();
    public int TotalStocks { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
}

public class StockInfo
{
    public string Symbol { get; set; } = string.Empty;
    public string StockType { get; set; } = string.Empty; // "Voting" or "Non-Voting"
    public decimal? LastPrice { get; set; }
    public decimal? Change { get; set; }
    public decimal? ChangePercent { get; set; }
    public bool IsActive { get; set; }
}