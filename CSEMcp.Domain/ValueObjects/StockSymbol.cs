using CSEMcp.Domain.Enums;

namespace CSEMcp.Domain.ValueObjects;

/// <summary>
/// Value object representing a CSE stock symbol with validation and formatting
/// </summary>
public class StockSymbol
{
    private readonly string _value;

    private StockSymbol(string value)
    {
        _value = value;
    }

    public string Value => _value;

    /// <summary>
    /// Gets the base ticker without suffix (e.g., JKH from JKH.N0000)
    /// </summary>
    public string Ticker => GetTicker(_value);

    /// <summary>
    /// Gets the stock type based on the suffix
    /// </summary>
    public StockType Type => GetStockType(_value);

    /// <summary>
    /// Creates a StockSymbol from various input formats
    /// </summary>
    /// <param name="symbol">Symbol in any format (JKH, JKH.N0000, JKH.X0000)</param>
    /// <param name="defaultType">Default stock type if not specified (defaults to Voting)</param>
    public static StockSymbol Create(string symbol, StockType defaultType = StockType.Voting)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("Symbol cannot be empty", nameof(symbol));
        }

        var normalized = NormalizeSymbol(symbol, defaultType);
        return new StockSymbol(normalized);
    }

    /// <summary>
    /// Normalizes symbol to CSE format (.N0000 or .X0000)
    /// </summary>
    private static string NormalizeSymbol(string symbol, StockType defaultType)
    {
        symbol = symbol.Trim().ToUpperInvariant();

        // Already in CSE format
        if (symbol.EndsWith(".N0000") || symbol.EndsWith(".X0000"))
        {
            return symbol;
        }

        // Extract ticker (remove any suffix)
        var ticker = symbol.Split('.')[0];

        // Apply correct suffix based on default type
        var suffix = defaultType == StockType.Voting ? ".N0000" : ".X0000";
        return $"{ticker}{suffix}";
    }

    /// <summary>
    /// Extracts the ticker from a full symbol
    /// </summary>
    private static string GetTicker(string symbol)
    {
        return symbol.Split('.')[0];
    }

    /// <summary>
    /// Determines stock type from symbol suffix
    /// </summary>
    private static StockType GetStockType(string symbol)
    {
        if (symbol.EndsWith(".X0000"))
        {
            return StockType.NonVoting;
        }
        return StockType.Voting; // .N0000 or default
    }

    public override string ToString() => _value;

    public override bool Equals(object? obj)
    {
        if (obj is StockSymbol other)
        {
            return _value == other._value;
        }
        return false;
    }

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(StockSymbol? left, StockSymbol? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(StockSymbol? left, StockSymbol? right) => !(left == right);
}