using Infrastructure;

namespace Domain;

public enum ExpenseType
{
    Restaurant,
    Hotel,
    Misc
}

public static class ExpenseTypeExtension
{
    public static ExpenseType Parse(string input)
    {
        try
        {
            return (ExpenseType)Enum.Parse(typeof(ExpenseType), input);
        }
        catch (ArgumentException)
        {
            throw new InvalidExpenseTypeException();
        }

    }
}
