using Application.Expenses.Commands.CreateExpense;
using Domain;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Application.Tests;

public class CreateExpenseCommandHandlerTest
{
    private static readonly Guid UserId = Guid.NewGuid();

    private static readonly Expense AlreadyExistingUserExpense = new()
    {
        Id = Guid.NewGuid(),
        Date = DateTime.Today.AddDays(-1),
        Currency = Currency.EUR,
        Amount = 100,
        Commentary = "a commentary",
        Type = ExpenseType.Hotel,
        UserId = UserId
    };

    private static readonly User User = new()
    {
        Id = UserId,
        LastName = "LastName",
        FirstName = "FirstName",
        Currency = Currency.EUR,
        Expenses = new List<Expense>
        {
            AlreadyExistingUserExpense
        }
    };

    private static readonly CreateExpenseCommand ValidCommand = new()
    {
        Amount = 100,
        Commentary = "a commentary",
        Currency = User.Currency.ToString(),
        Date = DateTime.Now,
        Type = "Hotel",
        UserId = User.Id
    };

    [Fact(DisplayName = "Given a valid command to create an expense " +
                        "when handling it " +
                        "then it creates expense.")]
    public async void CanCreateExpenseFromCommand()
    {
        var mockedApplicationDbContext = TestableApplicationDbContext();
        CreateExpenseCommandHandler createExpenseCommandHandler = new CreateExpenseCommandHandler(
            mockedApplicationDbContext.Object
        );

        await createExpenseCommandHandler.Handle(
            ValidCommand,
            It.IsAny<CancellationToken>()
        );

        mockedApplicationDbContext
            .Verify(mock => mock
                    .Expenses
                    .Add(It.Is<Expense>(
                        x =>
                            x.UserId == ValidCommand.UserId
                            && x.Date == ValidCommand.Date
                            && Math.Abs(x.Amount - ValidCommand.Amount) == 0
                            && x.Commentary == ValidCommand.Commentary
                            && x.Type == ExpenseTypeExtension.Parse(ValidCommand.Type)
                            && x.Currency == CurrencyExtension.Parse(ValidCommand.Currency)
                    )), Times.Once()
            );

        mockedApplicationDbContext
            .Verify(mock => mock
                    .SaveChangesAsync(
                        It.IsAny<CancellationToken>()
                    )
                , Times.Once()
            );
    }

    [Fact(DisplayName = "Given a command to create an expense with unknown user " +
                        "when handling it " +
                        "then it throws an error.")]
    public async void CannotCreateExpenseForUnknownUser()
    {
        var mockedApplicationDbContext = TestableApplicationDbContext();
        CreateExpenseCommandHandler createExpenseCommandHandler = new CreateExpenseCommandHandler(
            mockedApplicationDbContext.Object
        );
        var unknownUserId = Guid.NewGuid();

        await Assert.ThrowsAsync<UserNotFoundException>(
            () => createExpenseCommandHandler.Handle(
                new CreateExpenseCommand
                {
                    Amount = 100,
                    Commentary = "a commentary",
                    Currency = User.Currency.ToString(),
                    Date = DateTime.Now,
                    Type = "Hotel",
                    UserId = unknownUserId
                },
                It.IsAny<CancellationToken>()
            ));
    }

    [Fact(DisplayName = "Given a command to create an expense with illegal currency " +
                        "when handling it " +
                        "then it throws an error.")]
    public async void CannotCreateExpenseWithIllegalCurrency()
    {
        var mockedApplicationDbContext = TestableApplicationDbContext();
        CreateExpenseCommandHandler createExpenseCommandHandler = new CreateExpenseCommandHandler(
            mockedApplicationDbContext.Object
        );
        var illegalCurrency = "USD";

        await Assert.ThrowsAsync<IllegalCurrencyException>(
            () => createExpenseCommandHandler.Handle(
                new CreateExpenseCommand
                {
                    Amount = AlreadyExistingUserExpense.Amount,
                    Commentary = "a commentary",
                    Currency = illegalCurrency,
                    Date = AlreadyExistingUserExpense.Date,
                    Type = "Hotel",
                    UserId = User.Id
                },
                It.IsAny<CancellationToken>()
            ));
    }

    [Fact(DisplayName = "Given a command to create a duplicated expense " +
                        "when handling it " +
                        "then it throws an error.")]
    public async void CannotCreateDuplicatedExpense()
    {
        var mockedApplicationDbContext = TestableApplicationDbContext();
        CreateExpenseCommandHandler createExpenseCommandHandler = new CreateExpenseCommandHandler(
            mockedApplicationDbContext.Object
        );

        await Assert.ThrowsAsync<DuplicatedExpenseException>(
            () => createExpenseCommandHandler.Handle(
                new CreateExpenseCommand
                {
                    Amount = AlreadyExistingUserExpense.Amount,
                    Commentary = AlreadyExistingUserExpense.Commentary,
                    Currency = AlreadyExistingUserExpense.Currency.ToString(),
                    Date = AlreadyExistingUserExpense.Date,
                    Type = AlreadyExistingUserExpense.Type.ToString(),
                    UserId = User.Id
                },
                It.IsAny<CancellationToken>()
            ));
    }


    private static Mock<IApplicationDbContext> TestableApplicationDbContext()
    {
        Mock<IApplicationDbContext> mockedApplicationDbContext = new();
        Mock<DbSet<User>> mockedUsersDbSet = new List<User> { User }
            .AsQueryable()
            .BuildMockDbSet();

        mockedApplicationDbContext
            .Setup(mock => mock
                .Expenses
                .Add(It.IsAny<Expense>()
                )
            );
        mockedApplicationDbContext
            .Setup(mock => mock.Users)
            .Returns(mockedUsersDbSet.Object);
        return mockedApplicationDbContext;
    }
}
