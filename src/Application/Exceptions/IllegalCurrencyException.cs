namespace Domain;

public class IllegalCurrencyException : ExpenseValidationException
{
    public IllegalCurrencyException() : base("Cannot have currency different from user's one.")
    {
    }
}
