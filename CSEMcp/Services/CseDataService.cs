using System.Text.Json;
using CSEMcp.Models;

namespace CSEMcp.Services;

public class CseDataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CseDataService> _logger;

    public CseDataService(HttpClient httpClient, ILogger<CseDataService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        // Configure HttpClient with CSE base URL and headers
        _httpClient.BaseAddress = new Uri("https://www.cse.lk/");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; CSEMcp/1.0)");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
    }

    public async Task<StockQuote?> GetQuoteAsync(string symbol, CancellationToken cancellationToken = default)
    {
        try
        {
            // Convert symbol from .CM format to .N0000 format if needed
            var cseSymbol = ConvertToCseSymbol(symbol);

            _logger.LogInformation("Fetching quote for CSE symbol: {Symbol}", cseSymbol);

            // CSE API endpoint for company info summary
            var endpoint = "api/companyInfoSummery";

            var formData = new Dictionary<string, string>
            {
                { "symbol", cseSymbol }
            };

            var response = await _httpClient.PostAsync(
                endpoint,
                new FormUrlEncodedContent(formData),
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("CSE API returned error status: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var jsonDoc = JsonDocument.Parse(content);

            // Extract data from reqSymbolInfo
            if (!jsonDoc.RootElement.TryGetProperty("reqSymbolInfo", out var symbolInfo))
            {
                _logger.LogWarning("No symbol info available for: {Symbol}", cseSymbol);
                return null;
            }

            var quote = new StockQuote
            {
                Symbol = symbolInfo.GetProperty("symbol").GetString() ?? cseSymbol,
                Name = symbolInfo.GetProperty("name").GetString() ?? cseSymbol,
                Price = symbolInfo.GetProperty("lastTradedPrice").GetDecimal(),
                Change = symbolInfo.GetProperty("change").GetDecimal(),
                ChangePercent = symbolInfo.GetProperty("changePercentage").GetDecimal(),
                Volume = symbolInfo.GetProperty("tdyShareVolume").GetInt64(),
                MarketCap = FormatMarketCap(symbolInfo.GetProperty("marketCap").GetDouble()),
                Currency = "LKR",
                Timestamp = DateTime.UtcNow // CSE doesn't provide timestamp in this endpoint
            };

            _logger.LogInformation("Successfully fetched quote for {Symbol}: {Price}", cseSymbol, quote.Price);
            return quote;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching quote for symbol: {Symbol}", symbol);
            return null;
        }
    }

    /// <summary>
    /// Converts symbol to CSE format (.N0000) from various input formats
    /// </summary>
    private string ConvertToCseSymbol(string symbol)
    {
        // If already in CSE format (e.g., JKH.N0000), return as is
        if (symbol.Contains(".N"))
        {
            return symbol;
        }

        // If in .CM format (e.g., JKH.CM), convert to CSE format
        if (symbol.EndsWith(".CM", StringComparison.OrdinalIgnoreCase))
        {
            var ticker = symbol.Substring(0, symbol.Length - 3); // Remove .CM
            return $"{ticker}.N0000";
        }

        // If just ticker (e.g., JKH), add .N0000 suffix
        return $"{symbol}.N0000";
    }

    private static string FormatMarketCap(double marketCap)
    {
        if (marketCap >= 1_000_000_000_000)
            return $"{marketCap / 1_000_000_000_000.0:F1}T";
        if (marketCap >= 1_000_000_000)
            return $"{marketCap / 1_000_000_000.0:F1}B";
        if (marketCap >= 1_000_000)
            return $"{marketCap / 1_000_000.0:F1}M";
        return marketCap.ToString("F0");
    }

    /// <summary>
    /// Gets detailed company profile information
    /// </summary>
    public async Task<CompanyProfile?> GetCompanyProfileAsync(string symbol, CancellationToken cancellationToken = default)
    {
        try
        {
            var cseSymbol = ConvertToCseSymbol(symbol);
            _logger.LogInformation("Fetching company profile for: {Symbol}", cseSymbol);

            var formData = new Dictionary<string, string>
            {
                { "symbol", cseSymbol }
            };

            var response = await _httpClient.PostAsync(
                "api/companyProfile",
                new FormUrlEncodedContent(formData),
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("CSE API returned error status: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var jsonDoc = JsonDocument.Parse(content);

            // Extract company summary info
            if (!jsonDoc.RootElement.TryGetProperty("reqComSumInfo", out var comSumInfo) ||
                comSumInfo.GetArrayLength() == 0)
            {
                _logger.LogWarning("No company summary info available for: {Symbol}", cseSymbol);
                return null;
            }

            var companyInfo = comSumInfo[0];

            return new CompanyProfile
            {
                Symbol = companyInfo.GetProperty("symbol").GetString() ?? cseSymbol,
                Name = companyInfo.GetProperty("name").GetString() ?? "",
                Sector = companyInfo.TryGetProperty("sector", out var sector) ? sector.GetString() : null,
                RegisteredOffice = companyInfo.TryGetProperty("registeredOffice", out var office) ? office.GetString() : null,
                Phone = companyInfo.TryGetProperty("tel1", out var tel) ? tel.GetString() : null,
                Email = companyInfo.TryGetProperty("email1", out var email) ? email.GetString() : null,
                Website = companyInfo.TryGetProperty("web", out var web) ? web.GetString() : null,
                Established = companyInfo.TryGetProperty("established", out var est) ? est.GetString() : null
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching company profile for symbol: {Symbol}", symbol);
            return null;
        }
    }

    /// <summary>
    /// Gets day's trading data (individual trades)
    /// </summary>
    public async Task<List<Trade>?> GetDayTradesAsync(string symbol, CancellationToken cancellationToken = default)
    {
        try
        {
            var cseSymbol = ConvertToCseSymbol(symbol);
            _logger.LogInformation("Fetching day's trades for: {Symbol}", cseSymbol);

            var formData = new Dictionary<string, string>
            {
                { "symbol", cseSymbol }
            };

            var response = await _httpClient.PostAsync(
                "api/daysTrade",
                new FormUrlEncodedContent(formData),
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("CSE API returned error status: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var trades = JsonSerializer.Deserialize<List<Trade>>(content);

            return trades;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching day's trades for symbol: {Symbol}", symbol);
            return null;
        }
    }
}