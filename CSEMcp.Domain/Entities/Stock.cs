using CSEMcp.Domain.Enums;

namespace CSEMcp.Domain.Entities;

/// <summary>
/// Represents a specific stock/share class listed on the CSE
/// A company can have multiple stocks (e.g., voting and non-voting shares)
/// </summary>
public class Stock
{
    /// <summary>
    /// Full CSE symbol including suffix (e.g., JKH.N0000 for voting, TESS.X0000 for non-voting)
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Base ticker symbol without suffix (e.g., JKH, TESS)
    /// </summary>
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Type of stock - Voting (.N0000) or NonVoting (.X0000)
    /// </summary>
    public StockType Type { get; set; }

    /// <summary>
    /// Reference to the parent company
    /// </summary>
    public string CompanySymbol { get; set; } = string.Empty;

    // Trading information
    public decimal LastTradedPrice { get; set; }
    public decimal Change { get; set; }
    public decimal ChangePercentage { get; set; }
    public long Volume { get; set; }
    public double MarketCap { get; set; }
    public DateTime LastUpdated { get; set; }
}