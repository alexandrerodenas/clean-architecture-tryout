namespace Domain;

public class FutureDateException : ExpenseValidationException
{
    public FutureDateException() : base("Date cannot be in the future.")
    {
    }
}
