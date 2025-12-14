namespace SharesMCP.Models;

public class StockQuote
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Change { get; set; }
    public decimal ChangePercent { get; set; }
    public long Volume { get; set; }
    public string? MarketCap { get; set; }
    public string Currency { get; set; } = "LKR";
    public DateTime Timestamp { get; set; }
}