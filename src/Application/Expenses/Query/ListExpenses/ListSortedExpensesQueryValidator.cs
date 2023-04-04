using FluentValidation;

namespace Application;

public class ListSortedExpensesQueryValidator: AbstractValidator<ListSortedExpensesQuery>
{

    public ListSortedExpensesQueryValidator()
    {
        RuleFor(query => query.SortBy)
            .NotNull()
            .Unless(query => query.OrderBy == null)
            .WithMessage("Sort by cannot be null if Order by is not.");
        RuleFor(query => query.OrderBy)
            .NotNull()
            .Unless(query => query.SortBy == null)
            .WithMessage("Order by cannot be null if Sort by is not.");
    }

}
