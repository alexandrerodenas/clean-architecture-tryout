using Application.Expenses.Commands.CreateExpense;
using Domain;
using FluentValidation;
using Xunit;

namespace Application.Tests;

public class CreateExpenseCommandValidatorTest
{
    private static readonly Guid ValidUUID = Guid.NewGuid();
    private const Currency ValidCurrency = Currency.EUR;
    private const float ValidAmount = 100f;
    private const string ValidCommentary = "a commentary";
    private static readonly DateTime ValidDate = DateTime.Now;
    private static readonly ExpenseType ValidExpenseType = ExpenseType.Hotel;

    private static readonly CreateExpenseCommandValidator ExpenseCommandValidator = new();


    [Fact(DisplayName = "Given a date in future " +
                        "when validating a command with such a date " +
                        "then expected validation error is thrown.")]
    public void ExpenseCannotHaveDateInFuture()
    {
        DateTime tomorrow = DateTime.Now.AddDays(1);
        var command = new CreateExpenseCommand
        {
            Amount = ValidAmount,
            Commentary = ValidCommentary,
            Currency = ValidCurrency.ToString(),
            Date = tomorrow,
            Type = ValidExpenseType.ToString(),
            UserId = ValidUUID
        };

        Assert.Throws<ValidationException>(() =>
            ExpenseCommandValidator.ValidateAndThrow(command)
        );
    }

    [Fact(DisplayName = "Given a date older than three months ago " +
                        "when validating a command with such a date " +
                        "then expected validation error is thrown.")]
    public void ExpenseCannotHaveDateOlderThanThreeMonthsAgo()
    {
        DateTime moreThanThreeMonthsAgo = DateTime.Now.AddMonths(-3).AddMilliseconds(-1);
        var command = new CreateExpenseCommand
        {
            Amount = ValidAmount,
            Commentary = ValidCommentary,
            Currency = ValidCurrency.ToString(),
            Date = moreThanThreeMonthsAgo,
            Type = ValidExpenseType.ToString(),
            UserId = ValidUUID
        };

        Assert.Throws<ValidationException>(() =>
            ExpenseCommandValidator.ValidateAndThrow(command)
        );
    }

    [Fact(DisplayName = "Given an empty commentary " +
                        "when validating a command with such a commentary " +
                        "then expected validation error is thrown.")]
    public void ExpenseCannotHaveEmptyCommentary()
    {
        string anEmptyCommentary = "";
        var command = new CreateExpenseCommand
        {
            Amount = ValidAmount,
            Commentary = anEmptyCommentary,
            Currency = ValidCurrency.ToString(),
            Date = ValidDate,
            Type = ValidExpenseType.ToString(),
            UserId = ValidUUID
        };

        Assert.Throws<ValidationException>(() =>
            ExpenseCommandValidator.ValidateAndThrow(command)
        );
    }

    [Fact(DisplayName = "Given no commentary " +
                        "when validating a command with such a commentary " +
                        "then expected validation error is thrown.")]
    public void ExpenseCannotHaveNoCommentary()
    {
        string noCommentary = null!;
        var command = new CreateExpenseCommand
        {
            Amount = ValidAmount,
            Commentary = noCommentary,
            Currency = ValidCurrency.ToString(),
            Date = ValidDate,
            Type = ValidExpenseType.ToString(),
            UserId = ValidUUID
        };

        Assert.Throws<ValidationException>(() =>
            ExpenseCommandValidator.ValidateAndThrow(command)
        );
    }
}
