using System;
using System.Collections.Generic;
using Xunit;

namespace Domain;

public class UserTest
{
    private const string ALastName = "aLastName";
    private const string AFirstName = "aFirstName";
    private const Currency UserCurrency = Currency.EUR;
    private static readonly Guid AUuid = Guid.NewGuid();
    private static readonly List<Expense> SomeExpenses = TestableExpenses();

    private static readonly User AUser = new()
    {
        Id = AUuid,
        LastName = ALastName,
        FirstName = AFirstName,
        Currency = UserCurrency,
        Expenses = SomeExpenses
    };

    [Fact(DisplayName = "Given a user " +
                        "when getting its id " +
                        "then it returns its id.")]
    public void UserHasId()
    {
        Assert.Equal(AUuid, AUser.Id);
    }

    [Fact(DisplayName = "Given a user " +
                        "when getting its last name " +
                        "then it returns its last name.")]
    public void UserHasLastName()
    {
        Assert.Equal(ALastName, AUser.LastName);
    }

    [Fact(DisplayName = "Given a user " +
                        "when getting its first name " +
                        "then it returns its first name.")]
    public void UserHasFirstName()
    {
        Assert.Equal(AFirstName, AUser.FirstName);
    }

    [Fact(DisplayName = "Given a user " +
                        "when getting its currency " +
                        "then it returns its currency.")]
    public void UserHasCurrency()
    {
        Assert.Equal(UserCurrency, AUser.Currency);
    }

    [Fact(DisplayName = "Given a user " +
                        "when getting its expenses " +
                        "then it returns its expenses.")]
    public void UserHasExpenses()
    {
        Assert.Equal(SomeExpenses, AUser.Expenses);
    }

    private static List<Expense> TestableExpenses()
    {
        return new List<Expense>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Currency = UserCurrency,
                Amount = 100,
                Commentary = "no breakfast",
                Type = ExpenseType.Hotel,
                UserId = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Currency = UserCurrency,
                Amount = 90,
                Commentary = "BioBurger",
                Type = ExpenseType.Restaurant,
                UserId = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Currency = UserCurrency,
                Amount = 800,
                Commentary = "Garage 19",
                Type = ExpenseType.Misc,
                UserId = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Currency = UserCurrency,
                Amount = 250.5f,
                Commentary = "London IBIS",
                Type = ExpenseType.Hotel,
                UserId = Guid.NewGuid()
            }
        };
    }
}
