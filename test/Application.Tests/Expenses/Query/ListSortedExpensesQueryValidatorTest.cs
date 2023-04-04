using FluentValidation;
using Xunit;

namespace Application.Tests.Expenses.Query;

public class ListSortedExpensesQueryValidatorTest
{
    private static readonly ListSortedExpensesQueryValidator ListSortedExpensesQueryValidator = new ListSortedExpensesQueryValidator();

    [Fact(DisplayName = "Given a query with sort being null and order being not null " +
                        "when validating query " +
                        "then validation error is thrown.")]
    public void CannotHaveSortBeingNullAndOrderNotNull()
    {
        var command = new ListSortedExpensesQuery
        {
            UserId = Guid.NewGuid(),
            OrderBy = OrderBy.DATE,
            SortBy = null
        };

        Assert.Throws<ValidationException>(() =>
            ListSortedExpensesQueryValidator.ValidateAndThrow(command)
        );
    }

    [Fact(DisplayName = "Given a query with sort not being null and order being null " +
                        "when validating query " +
                        "then validation error is thrown.")]
    public void CannotHaveSortBeingNotNullAndOrderNull()
    {
        var command = new ListSortedExpensesQuery
        {
            UserId = Guid.NewGuid(),
            OrderBy = null,
            SortBy = SortBy.ASC
        };

        Assert.Throws<ValidationException>(() =>
            ListSortedExpensesQueryValidator.ValidateAndThrow(command)
        );
    }


    [Fact(DisplayName = "Given a query with sort being null and order being null " +
                        "when validating query " +
                        "then no validation error is thrown.")]
    public void CannotHaveSortBeingNullAndOrderNull()
    {
        var command = new ListSortedExpensesQuery
        {
            UserId = Guid.NewGuid(),
            OrderBy = null,
            SortBy = null
        };

        ListSortedExpensesQueryValidator.ValidateAndThrow(command);
    }
}
