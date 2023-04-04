namespace Domain;

public abstract class ExpenseValidationException : Exception
{
    protected ExpenseValidationException(string message) : base(message)
    {
    }
}
