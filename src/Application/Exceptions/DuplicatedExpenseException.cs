namespace Domain;

public class DuplicatedExpenseException : ExpenseValidationException
{
    public DuplicatedExpenseException() : base("Cannot have duplicated expenses.")
    {
    }
}
