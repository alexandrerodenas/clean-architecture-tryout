using Infrastructure;
using Xunit;

namespace Domain;

public class ExpenseTypeTest
{
    [Fact(DisplayName = "Given an string representing a type of expense " +
                        "when getting it as enum value " +
                        "then it returns expected enum value.")]
    public void CanGetExpenseTypeFromString()
    {
        string aTypeOfExpense = "Restaurant";

        ExpenseType expenseType = ExpenseTypeExtension.Parse(aTypeOfExpense);

        Assert.Equal(ExpenseType.Restaurant, expenseType);
    }

    [Fact(DisplayName = "Given an string NOT representing a type of expense " +
                        "when getting it as enum value " +
                        "then it returns expected enum value.")]
    public void ThrowExceptionOnUnknownExpenseType()
    {
        string notAnExpenseType = "NotAnExpenseType";
        Assert.Throws<InvalidExpenseTypeException>(
            () => ExpenseTypeExtension.Parse(notAnExpenseType)
        );
    }
}
