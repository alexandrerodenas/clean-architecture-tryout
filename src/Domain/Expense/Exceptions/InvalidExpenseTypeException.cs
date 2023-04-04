using Domain;

namespace Infrastructure;

public class InvalidExpenseTypeException : ExpenseValidationException
{
    public InvalidExpenseTypeException() : base("Expense type is invalid.")
    {
    }
}
