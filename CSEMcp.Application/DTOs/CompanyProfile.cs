namespace CSEMcp.Application.DTOs;

public class CompanyProfile
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Sector { get; set; }
    public string? RegisteredOffice { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Established { get; set; }
}