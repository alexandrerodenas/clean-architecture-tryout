namespace Domain;

public enum Currency
{
    USD, // US Dollar
    EUR, // Euro
    GBP, // British Pound Sterling
    RUB, // Russian Ruble
    CHF  // Swiss Franc
}

public static class CurrencyExtension
{
    public static Currency Parse(string input)
    {
        try
        {
            return (Currency)Enum.Parse(typeof(Currency), input);
        }
        catch (ArgumentException)
        {
            throw new InvalidCurrencyException();
        }

    }
}
