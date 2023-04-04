using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace WebUI.Tests;

public class ListUserExpensesControllerTest
{
    private static readonly Expense AnExpense = new (
        DateTime.Now,
        Currency.CHF,
        10,
        "a commentary",
        ExpenseType.Hotel
    );

    private static readonly Expense AnotherExpense = new (
        DateTime.Now.AddDays(-1),
        Currency.CHF,
        2000,
        "another commentary",
        ExpenseType.Misc
    );

    private static readonly User AValidUser = new(
        "aLastName",
        "aFirstName",
        Currency.CHF,
        new List<Expense>
        {
            AnExpense,
            AnotherExpense
        }
    );

    private static readonly Guid AValidUserId = Guid.NewGuid();
    private static readonly Guid AnUnknownUserId = Guid.NewGuid();
    private static readonly Guid AnIdLeadingToServerError = Guid.NewGuid();

    private static readonly ListUserExpensesController Controller = new (
        CreateTestableGetUserUseCase().Object
    );

    [Fact(DisplayName = "Given a user id having expenses " +
                        "when getting this user along with its expenses " +
                        "then returns expected output order by ascending date.")]
    public async void CanGetUserWithExpensesFromApi()
    {
        IActionResult response = await Controller
            .ListExpensesByUser(AValidUserId, null, null)
            .ConfigureAwait(false);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(response);
        UserExpensesOutput userExpensesOutput = Assert.IsAssignableFrom<UserExpensesOutput>(
            okResult.Value
        );
        List<ExpenseOutput> expensesOrderedByAscendingDate = new List<ExpenseOutput>
        {
            ExpenseOutput.FromDomain(AnotherExpense),
            ExpenseOutput.FromDomain(AnExpense)
        };
        Assert.Equal(expensesOrderedByAscendingDate, userExpensesOutput.Expenses);
    }

    [Fact(DisplayName = "Given a user id having expenses " +
                        "when getting this user along with its expenses ordered ans sorted " +
                        "then returns expected output.")]
    public async void CanGetExpensesOrderedAndSortedFromApi()
    {
        IActionResult response = await Controller
            .ListExpensesByUser(AValidUserId, OrderBy.AMOUNT, SortBy.DESC)
            .ConfigureAwait(false);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(response);
        UserExpensesOutput userExpensesOutput = Assert.IsAssignableFrom<UserExpensesOutput>(
            okResult.Value
        );
        List<ExpenseOutput> expensesOrderedByDescendingAmount = new List<ExpenseOutput>
        {
            ExpenseOutput.FromDomain(AnotherExpense),
            ExpenseOutput.FromDomain(AnExpense)
        };
        Assert.Equal(expensesOrderedByDescendingAmount, userExpensesOutput.Expenses);
    }

    [Fact(DisplayName = "Given a user id having expenses " +
                        "when getting this user along with inconsistent sort and order option " +
                        "then a bad request is thrown.")]
    public async void CannotFetchUserWithExpensesIfSortAndOrderOptionsAreInconsistent()
    {
        OrderBy? anInconsistentOrderBy = null;
        SortBy anInconsistentSortBy = SortBy.DESC;

        IActionResult response = await Controller
            .ListExpensesByUser(AValidUserId, anInconsistentOrderBy, anInconsistentSortBy)
            .ConfigureAwait(false);

        Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal("Inconsistent listing options.", (response as BadRequestObjectResult)?.Value);
    }

    [Fact(DisplayName = "Given an unknown user id having expenses " +
                        "when getting this user " +
                        "then a bad request is thrown.")]
    public async void CannotFetchUnknownUser()
    {
        IActionResult response = await Controller
            .ListExpensesByUser(AnIdLeadingToServerError, null, null)
            .ConfigureAwait(false);

        Assert.IsType<ObjectResult>(response);
        Assert.Equal("Internal server error.", (response as ObjectResult)?.Value);
        Assert.Equal(500, (response as ObjectResult)?.StatusCode);
    }

    [Fact(DisplayName = "Given an user id having expenses " +
                        "and a random exception being thrown" +
                        "when getting this user " +
                        "then a server error is thrown.")]
    public async void CanHandleServerError()
    {
        IActionResult response = await Controller
            .ListExpensesByUser(AnUnknownUserId, null, null)
            .ConfigureAwait(false);

        Assert.IsType<NotFoundObjectResult>(response);
        Assert.Equal("User not found.", (response as NotFoundObjectResult)?.Value);
    }

    private static Mock<GetUser> CreateTestableGetUserUseCase()
    {
        Mock<GetUser> mockedGetUser = new Mock<GetUser>();
        mockedGetUser
            .Setup(mock => mock.For(AValidUserId))
            .ReturnsAsync(AValidUser);
        mockedGetUser
            .Setup(mock => mock.For(AnUnknownUserId))
            .ThrowsAsync(new UserNotFoundException());
        mockedGetUser
            .Setup(mock => mock.For(AnIdLeadingToServerError))
            .ThrowsAsync(new Exception());
        return mockedGetUser;
    }
}
