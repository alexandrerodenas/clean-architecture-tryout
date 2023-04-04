namespace Domain;

public class InvalidCurrencyException : Exception
{
    public InvalidCurrencyException() : base("Currency is invalid.")
    {
    }
}
