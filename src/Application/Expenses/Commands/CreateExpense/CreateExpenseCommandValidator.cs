using FluentValidation;

namespace Application.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandValidator: AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(expense => expense.Date)
            .Must(BeMoreRecentThanThreeMonthsAgo)
            .WithMessage("Date cannot older than three months ago.");

        RuleFor(expense => expense.Date)
            .Must(BeInThePast)
            .WithMessage("Date cannot be in future.");

        RuleFor(expense => expense.Commentary)
            .NotNull()
            .NotEmpty()
            .WithMessage("Commentary must be defined.");
    }

    private static bool BeMoreRecentThanThreeMonthsAgo(DateTime date)
    {
        return date.CompareTo(DateTime.Now.AddMonths(-3)) >= 0;
    }

    private static bool BeInThePast(DateTime date)
    {
        return date.CompareTo(DateTime.Now) <= 0;
    }
}
