namespace Domain;

public class TooOldDateException : ExpenseValidationException
{
    public TooOldDateException() : base("Date cannot older than three months ago.")
    {
    }
}
