using System.Text.Json.Serialization;

namespace CSEMcp.Application.DTOs;

public class Trade
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("quantity")]
    public long Quantity { get; set; }

    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("change")]
    public decimal Change { get; set; }

    [JsonPropertyName("changePercentage")]
    public decimal ChangePercentage { get; set; }

    [JsonPropertyName("hiTrade")]
    public decimal HiTrade { get; set; }

    [JsonPropertyName("lowTrade")]
    public decimal LowTrade { get; set; }
}