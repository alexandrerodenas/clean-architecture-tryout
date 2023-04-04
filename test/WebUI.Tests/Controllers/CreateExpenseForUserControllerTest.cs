using System.Diagnostics;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace WebUI.Tests;

public class CreateExpenseForUserControllerTest
{
    private static readonly User AValidUser = new (
        "lastname",
        "firstname",
        Currency.EUR,
        new List<Expense>()
    );
    private static readonly ExpenseToCreateInput AValidExpenseToCreateInput = new (
        DateTime.Now,
        "EUR",
        100,
        "a commentary",
        "Hotel"
    );
    private static readonly ExpenseToCreateInput AnExpenseHavingAnIssueAtInsertion = new (
        DateTime.Now,
        "EUR",
        100,
        "a commentary",
        "Hotel"
    );
    private static readonly ExpenseToCreateInput AnInvalidExpenseToCreateInput = new (
        DateTime.Now,
        "EUR",
        100,
        "a commentary",
        "NoAValidType"
    );
    private static readonly ExpenseToCreateInput AnExpenseWithInvalidCurrency = new (
        DateTime.Now,
        "CHF",
        100,
        "a commentary",
        "Hotel"
    );



    private static readonly Guid AValidUserId = Guid.NewGuid();
    private static readonly Guid AnUnknownUserId = Guid.NewGuid();
    private static readonly Guid AnIdLeadingToServerError = Guid.NewGuid();

    private static readonly CreateExpenseForUserController Controller = new (
        CreateTestableGetUserUseCase().Object,
        CreateTestableCreateExpenseUseCase().Object
    );

    [Fact(DisplayName = "Given an expense to create for a user " +
                        "when creating it in database " +
                        "then user has a new expense.")]
    public async void CanCreateNewExpenseForUser()
    {
        IActionResult response = await Controller
            .CreateExpenseFor(AValidUserId, AValidExpenseToCreateInput)
            .ConfigureAwait(false);

        OkObjectResult okResult = Assert.IsType<OkObjectResult>(response);
        UserExpensesOutput responseOutput = Assert.IsAssignableFrom<UserExpensesOutput>(
            okResult.Value
        );
        Assert.Contains(
            ExpenseOutput.FromDomain(AValidExpenseToCreateInput.ToDomain()),
            responseOutput.Expenses
            );
    }

    [Fact(DisplayName = "Given an expense to create for an unknown user " +
                        "when creating it in database " +
                        "then expected error is thrown.")]
    public async void ThrowNotFoundUserExceptionOnUnknownUser()
    {
        CreateExpenseForUserController controller = new (
            CreateTestableGetUserUseCase().Object,
            CreateTestableCreateExpenseUseCase().Object
        );

        IActionResult response = await controller
            .CreateExpenseFor(AnUnknownUserId, AValidExpenseToCreateInput)
            .ConfigureAwait(false);

        Assert.IsType<NotFoundObjectResult>(response);
        Assert.Equal("User not found.", (response as NotFoundObjectResult)?.Value);
    }

    [Fact(DisplayName = "Given an expense to create having an unknown type " +
                        "when creating it in database " +
                        "then expected error is thrown.")]
    public async void ThrowInvalidExpenseTypeException()
    {
        CreateExpenseForUserController controller = new (
            CreateTestableGetUserUseCase().Object,
            CreateTestableCreateExpenseUseCase().Object
        );

        IActionResult response = await controller
            .CreateExpenseFor(AValidUserId, AnInvalidExpenseToCreateInput)
            .ConfigureAwait(false);

        Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal("Expense type is invalid.", (response as BadRequestObjectResult)?.Value);
    }

    [Fact(DisplayName = "Given an expense to create " +
                        "when creating it in database " +
                        "and an error at persistence occurs" +
                        "then expected error is thrown.")]
    public async void ThrowServerErrorOnIssueAtInsertion()
    {
        CreateExpenseForUserController controller = new (
            CreateTestableGetUserUseCase().Object,
            CreateTestableCreateExpenseUseCase().Object
        );

        IActionResult response = await controller
            .CreateExpenseFor(AValidUserId, AnExpenseHavingAnIssueAtInsertion)
            .ConfigureAwait(false);

        Assert.IsType<ObjectResult>(response);
        Assert.Equal("Unexpected error at expense insertion.", (response as ObjectResult)?.Value);
        Assert.Equal(500, (response as ObjectResult)?.StatusCode);
    }

    [Fact(DisplayName = "Given an user " +
                        "and a expense incompatible for this user " +
                        "when creating expense in database for this user " +
                        "then expected error is thrown with expected reason.")]
    public async void ThrowErrorOnIncompatibleExpenseForUser()
    {
        CreateExpenseForUserController controller = new (
            CreateTestableGetUserUseCase().Object,
            CreateTestableCreateExpenseUseCase().Object
        );

        IActionResult response = await controller
            .CreateExpenseFor(AValidUserId, AnExpenseWithInvalidCurrency)
            .ConfigureAwait(false);

        Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal("Cannot have currency different from user's one.", (response as BadRequestObjectResult)?.Value);
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
            .ThrowsAsync(new UnreachableException());
        return mockedGetUser;
    }

    private static Mock<ICreateExpense> CreateTestableCreateExpenseUseCase()
    {
        Mock<ICreateExpense> mockedGetUser = new Mock<ICreateExpense>();
        mockedGetUser
            .Setup(mock => mock
                .ForUser(AValidUserId, AValidExpenseToCreateInput.ToDomain())
            )
            .ReturnsAsync(true);
        mockedGetUser
            .Setup(mock => mock
                .ForUser(AValidUserId, AnExpenseHavingAnIssueAtInsertion.ToDomain())
            )
            .ReturnsAsync(false);
        return mockedGetUser;
    }
}
