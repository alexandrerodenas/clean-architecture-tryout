namespace Domain;

public class NoCommentaryException : ExpenseValidationException
{
    public NoCommentaryException() : base("Commentary cannot be null or empty.")
    {
    }
}
