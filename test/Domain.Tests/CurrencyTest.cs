using Xunit;

namespace Domain;

public class CurrencyTest
{
    [Fact(DisplayName = "Given an string representing a currency " +
                        "when getting it as enum value " +
                        "then it returns expected enum value.")]
    public void CanGetCurrencyFromString()
    {
        string aCurrency = "EUR";

        Currency currency = CurrencyExtension.Parse(aCurrency);

        Assert.Equal(Currency.EUR, currency);
    }

    [Fact(DisplayName = "Given an string NOT representing a currency " +
                        "when getting it as enum value " +
                        "then it returns expected enum value.")]
    public void ThrowExceptionOnUnknownCurrency()
    {
        string notAnCurrency = "NotAnCurrency";
        Assert.Throws<InvalidCurrencyException>(
            () => CurrencyExtension.Parse(notAnCurrency)
        );
    }
}
