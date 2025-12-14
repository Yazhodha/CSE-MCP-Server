using System.ComponentModel;
using ModelContextProtocol.Server;
using CSEMcp.Infrastructure.ExternalServices;

namespace CSEMcp.Tools;

[McpServerToolType]
public class GetCompanyProfileTool
{
    private readonly CseDataService _cseDataService;
    private readonly ILogger<GetCompanyProfileTool> _logger;

    public GetCompanyProfileTool(CseDataService cseDataService, ILogger<GetCompanyProfileTool> logger)
    {
        _cseDataService = cseDataService;
        _logger = logger;
    }

    [McpServerTool]
    [Description(ToolDescriptions.GetCompanyProfile)]
    public async Task<CompanyProfileResult> GetCompanyProfile(
        [Description(ToolDescriptions.SymbolParameter)] string symbol,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return new CompanyProfileResult
                {
                    Error = "Symbol cannot be empty"
                };
            }

            _logger.LogInformation("Getting company profile for symbol: {Symbol}", symbol);

            var profile = await _cseDataService.GetCompanyProfileAsync(symbol, cancellationToken);

            if (profile == null)
            {
                return new CompanyProfileResult
                {
                    Error = $"Unable to fetch company profile for symbol: {symbol}"
                };
            }

            return new CompanyProfileResult
            {
                Symbol = profile.Symbol,
                Name = profile.Name,
                Sector = profile.Sector,
                RegisteredOffice = profile.RegisteredOffice,
                Phone = profile.Phone,
                Email = profile.Email,
                Website = profile.Website,
                Established = profile.Established
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing get_company_profile tool");
            return new CompanyProfileResult
            {
                Error = $"Error fetching company profile: {ex.Message}"
            };
        }
    }
}