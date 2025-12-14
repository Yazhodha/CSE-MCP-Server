namespace CSEMcp.Domain.ValueObjects;

/// <summary>
/// Value object representing a price in LKR (Sri Lankan Rupees)
/// </summary>
public class Price
{
    private readonly decimal _amount;
    private readonly string _currency;

    private Price(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Price cannot be negative", nameof(amount));
        }

        _amount = amount;
        _currency = currency;
    }

    public decimal Amount => _amount;
    public string Currency => _currency;

    /// <summary>
    /// Creates a price in LKR (default currency for CSE)
    /// </summary>
    public static Price FromLKR(decimal amount)
    {
        return new Price(amount, "LKR");
    }

    /// <summary>
    /// Creates a price in the specified currency
    /// </summary>
    public static Price From(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be empty", nameof(currency));
        }

        return new Price(amount, currency.ToUpperInvariant());
    }

    /// <summary>
    /// Formats the price for display (e.g., "Rs. 145.50")
    /// </summary>
    public string Format()
    {
        return _currency == "LKR"
            ? $"Rs. {_amount:N2}"
            : $"{_currency} {_amount:N2}";
    }

    public override string ToString() => Format();

    public override bool Equals(object? obj)
    {
        if (obj is Price other)
        {
            return _amount == other._amount && _currency == other._currency;
        }
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(_amount, _currency);

    public static bool operator ==(Price? left, Price? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Price? left, Price? right) => !(left == right);

    public static bool operator >(Price left, Price right)
    {
        ValidateSameCurrency(left, right);
        return left._amount > right._amount;
    }

    public static bool operator <(Price left, Price right)
    {
        ValidateSameCurrency(left, right);
        return left._amount < right._amount;
    }

    public static bool operator >=(Price left, Price right)
    {
        ValidateSameCurrency(left, right);
        return left._amount >= right._amount;
    }

    public static bool operator <=(Price left, Price right)
    {
        ValidateSameCurrency(left, right);
        return left._amount <= right._amount;
    }

    public static Price operator +(Price left, Price right)
    {
        ValidateSameCurrency(left, right);
        return new Price(left._amount + right._amount, left._currency);
    }

    public static Price operator -(Price left, Price right)
    {
        ValidateSameCurrency(left, right);
        return new Price(left._amount - right._amount, left._currency);
    }

    private static void ValidateSameCurrency(Price left, Price right)
    {
        if (left._currency != right._currency)
        {
            throw new InvalidOperationException(
                $"Cannot perform operation on prices with different currencies: {left._currency} and {right._currency}");
        }
    }
}