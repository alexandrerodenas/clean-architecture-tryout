using Domain;
using Xunit;

namespace Application.Tests;

public class ExpenseOutputTest
{
    [Fact(DisplayName = "Given an expense from domain " +
                        "when mapping it to expense output " +
                        "then return expected expense output.")]
    public void CanConvertExpenseFromDomainToOutput()
    {
        Expense expense = new()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Today,
            Currency = Currency.EUR,
            Amount = 12.3f,
            Commentary = "commentary",
            Type = ExpenseType.Hotel,
            UserId = Guid.NewGuid()
        };

        ExpenseOutput expenseOutput = ExpenseOutput.FromDomain(expense);

        Assert.Equal(expense.Amount, expenseOutput.Amount);
        Assert.Equal(expense.Date, expenseOutput.Date);
        Assert.Equal(expense.Commentary, expenseOutput.Commentary);
        Assert.Equal(expense.Currency.ToString(), expenseOutput.Currency);
        Assert.Equal(expense.Type.ToString(), expenseOutput.ExpenseType);
    }
}
