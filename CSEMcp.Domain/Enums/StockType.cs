namespace CSEMcp.Domain.Enums;

/// <summary>
/// Represents the type of stock listed on the Colombo Stock Exchange (CSE)
/// </summary>
public enum StockType
{
    /// <summary>
    /// Voting shares - Normal shares with voting rights (.N0000 suffix)
    /// Example: JKH.N0000, SPEN.N0000
    /// </summary>
    Voting,

    /// <summary>
    /// Non-voting shares - Shares without voting rights (.X0000 suffix)
    /// Example: TESS.X0000
    /// </summary>
    NonVoting
}