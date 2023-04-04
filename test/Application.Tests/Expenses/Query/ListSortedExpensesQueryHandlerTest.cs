using Domain;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Application.Tests.Expenses.Query;

public class ListSortedExpensesQueryHandlerTest
{
    private static readonly Guid AUserUuid = Guid.NewGuid();
    private static readonly Guid AnotherUserUuid = Guid.NewGuid();

    private static readonly List<Expense> ExpensesOfJohnDoe = new()
    {
        new()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now.AddDays(-2),
            Currency = Currency.EUR,
            Amount = 100,
            Commentary = "A commentary",
            Type = ExpenseType.Hotel,
            UserId = AUserUuid
        },
        new()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now.AddDays(-5),
            Currency = Currency.EUR,
            Amount = 200,
            Commentary = "A commentary",
            Type = ExpenseType.Hotel,
            UserId = AUserUuid
        },
        new()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now.AddDays(-3),
            Currency = Currency.EUR,
            Amount = 300,
            Commentary = "A commentary",
            Type = ExpenseType.Hotel,
            UserId = AUserUuid
        }
    };

    private static readonly List<Expense> ExpensesOfLinusTorval = new()
    {
        new()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now,
            Currency = Currency.CHF,
            Amount = 200,
            Commentary = "A commentary",
            Type = ExpenseType.Hotel,
            UserId = AnotherUserUuid
        },
        new()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now,
            Currency = Currency.CHF,
            Amount = 100,
            Commentary = "A commentary",
            Type = ExpenseType.Hotel,
            UserId = AnotherUserUuid
        },
    };

    private static readonly List<User> Users = new()
    {
        new()
        {
            Id = AUserUuid,
            FirstName = "john",
            LastName = "doe",
            Currency = Currency.EUR,
            Expenses = ExpensesOfJohnDoe
        },
        new()
        {
            Id = AnotherUserUuid,
            FirstName = "linus",
            LastName = "torval",
            Currency = Currency.CHF,
            Expenses = ExpensesOfLinusTorval
        }
    };


    [Fact(DisplayName = "Given a query to list expenses by date desc " +
                        "when handling it " +
                        "then it lists expenses as expected.")]
    public async void CanListExpensesByDateDesc()
    {
        ListSortedExpensesQuery listSortedExpensesQuery = new()
        {
            UserId = AUserUuid,
            OrderBy = OrderBy.DATE,
            SortBy = SortBy.DESC
        };

        UserExpensesOutput userExpensesOutput = await TestableListSortedExpensesQueryHandler().Handle(
            listSortedExpensesQuery,
            It.IsAny<CancellationToken>()
        );

        Assert.Equal("john doe", userExpensesOutput.UserNames);
        Assert.Equal(
            ExpensesOfJohnDoe
                .OrderByDescending(expense => expense.Date)
                .ToList()
                .Select(ExpenseOutput.FromDomain),
            userExpensesOutput.Expenses
        );
    }

    [Fact(DisplayName = "Given a query to list expenses by date asc " +
                        "when handling it " +
                        "then it lists expenses as expected.")]
    public async void CanListExpensesByDateAsc()
    {
        ListSortedExpensesQuery listSortedExpensesQuery = new()
        {
            UserId = AUserUuid,
            OrderBy = OrderBy.DATE,
            SortBy = SortBy.ASC
        };

        UserExpensesOutput userExpensesOutput = await TestableListSortedExpensesQueryHandler().Handle(
            listSortedExpensesQuery,
            It.IsAny<CancellationToken>()
        );

        Assert.Equal("john doe", userExpensesOutput.UserNames);
        Assert.Equal(
            ExpensesOfJohnDoe
                .OrderBy(expense => expense.Date)
                .ToList()
                .Select(ExpenseOutput.FromDomain),
            userExpensesOutput.Expenses
        );
    }


    [Fact(DisplayName = "Given a query to list expenses by amount desc " +
                        "when handling it " +
                        "then it lists expenses as expected.")]
    public async void CanListExpensesByAmountDesc()
    {
        ListSortedExpensesQuery listSortedExpensesQuery = new()
        {
            UserId = AUserUuid,
            OrderBy = OrderBy.AMOUNT,
            SortBy = SortBy.DESC
        };

        UserExpensesOutput userExpensesOutput = await TestableListSortedExpensesQueryHandler().Handle(
            listSortedExpensesQuery,
            It.IsAny<CancellationToken>()
        );

        Assert.Equal("john doe", userExpensesOutput.UserNames);
        Assert.Equal(
            ExpensesOfJohnDoe
                .OrderByDescending(expense => expense.Amount)
                .ToList()
                .Select(ExpenseOutput.FromDomain),
            userExpensesOutput.Expenses
        );
    }

    [Fact(DisplayName = "Given a query to list expenses by amount asc " +
                        "when handling it " +
                        "then it lists expenses as expected.")]
    public async void CanListExpensesByAmountAsc()
    {
        ListSortedExpensesQuery listSortedExpensesQuery = new()
        {
            UserId = AUserUuid,
            OrderBy = OrderBy.AMOUNT,
            SortBy = SortBy.ASC
        };

        UserExpensesOutput userExpensesOutput = await TestableListSortedExpensesQueryHandler()
            .Handle(
                listSortedExpensesQuery,
                It.IsAny<CancellationToken>()
            );

        Assert.Equal("john doe", userExpensesOutput.UserNames);
        Assert.Equal(
            ExpensesOfJohnDoe
                .OrderBy(expense => expense.Amount)
                .ToList()
                .Select(ExpenseOutput.FromDomain),
            userExpensesOutput.Expenses
        );
    }


    private static ListSortedExpensesQueryHandler TestableListSortedExpensesQueryHandler()
    {
        var mockedUsersDbSet = Users.AsQueryable().BuildMockDbSet();

        Mock<IApplicationDbContext> mockedApplicationDbContext = new();
        mockedApplicationDbContext
            .Setup(mock => mock
                .Users
            ).Returns(mockedUsersDbSet.Object);
        ListSortedExpensesQueryHandler listSortedExpensesQueryHandler = new(
            mockedApplicationDbContext.Object
        );
        return listSortedExpensesQueryHandler;
    }
}
