namespace CSEMcp.Domain.Entities;

/// <summary>
/// Represents a company listed on the Colombo Stock Exchange (CSE)
/// A company can have multiple stock types (voting and non-voting shares)
/// </summary>
public class Company
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Sector { get; set; }
    public string? RegisteredOffice { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Established { get; set; }

    /// <summary>
    /// Collection of stocks issued by this company (voting and non-voting)
    /// </summary>
    public List<Stock> Stocks { get; set; } = new();
}