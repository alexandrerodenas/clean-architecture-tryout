using System;
using Xunit;

namespace Domain;

public class ExpenseTest
{
    private static readonly Guid ValidUserUuid = Guid.NewGuid();
    private static readonly Guid ValidExpenseUuid = Guid.NewGuid();
    private const Currency ValidCurrency = Currency.EUR;
    private const float ValidAmount = 100f;
    private const string ValidCommentary = "a commentary";
    private static readonly DateTime ValidDate = DateTime.Now;
    private static readonly ExpenseType ValidExpenseType = ExpenseType.Hotel;

    private static readonly Expense ValidExpense = new()
    {
        Id = ValidExpenseUuid,
        Date = ValidDate,
        Currency = ValidCurrency,
        Amount = ValidAmount,
        Commentary = ValidCommentary,
        Type = ValidExpenseType,
        UserId = ValidUserUuid
    };

    [Fact(DisplayName = "Given an expense " +
                        "when getting its uuid " +
                        "then it returns expected uuid.")]
    public void ExpenseHasUuid()
    {
        Assert.Equal(ValidExpenseUuid, ValidExpense.Id);
    }

    [Fact(DisplayName = "Given an expense " +
                        "when getting its date " +
                        "then it returns expected date.")]
    public void ExpenseIsRelatedToAUser()
    {
        Assert.Equal(ValidDate, ValidExpense.Date);
    }

    [Fact(DisplayName = "Given an expense " +
                        "when getting its currency " +
                        "then it returns expected currency.")]
    public void ExpenseHasACurrency()
    {
        Assert.Equal(ValidCurrency, ValidExpense.Currency);
    }

    [Fact(DisplayName = "Given an expense " +
                        "when getting its amount " +
                        "then it returns expected amount.")]
    public void ExpenseHasAnAmount()
    {
        Assert.Equal(ValidAmount, ValidExpense.Amount);
    }

    [Fact(DisplayName = "Given an expense " +
                        "when getting its commentary " +
                        "then it returns expected commentary.")]
    public void ExpenseHasACommentary()
    {
        Assert.Equal(ValidCommentary, ValidExpense.Commentary);
    }

    [Fact(DisplayName = "Given an expense " +
                        "when getting its type " +
                        "then it returns expected type.")]
    public void ExpenseHasAType()
    {
        Assert.Equal(ValidExpenseType, ValidExpense.Type);
    }

    [Fact(DisplayName = "Given an expense " +
                        "when getting its user uuid " +
                        "then it returns expected uuid.")]
    public void ExpenseHasUserUuid()
    {
        Assert.Equal(ValidUserUuid, ValidExpense.UserId);
    }
}
