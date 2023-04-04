using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Expenses.Commands.CreateExpense;

public record CreateExpenseCommand : IRequest<Guid>
{
    public DateTime Date { get; init; }
    public string Currency { get; init; }
    public float Amount { get; init; }
    public string Commentary { get; init; }
    public string Type { get; init; }
    public Guid UserId { get; init; }
}

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateExpenseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateExpenseCommand request,
        CancellationToken cancellationToken
    )
    {
        Expense expenseToCreate = new Expense
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Commentary = request.Commentary,
            Currency = CurrencyExtension.Parse(request.Currency),
            Date = request.Date,
            Type = ExpenseTypeExtension.Parse(request.Type),
            UserId = request.UserId
        };

        User user = await _context
            .Users
            .Include(user => user.Expenses)
            .FirstOrDefaultAsync(
                x => x.Id == request.UserId,
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false) ?? throw new UserNotFoundException();

        RunValidationFor(user, expenseToCreate);

        _context.Expenses.Add(expenseToCreate);

        await _context.SaveChangesAsync(cancellationToken);

        return expenseToCreate.Id;
    }

    private static void RunValidationFor(User user, Expense expenseToCreate)
    {
        if (user.Currency != expenseToCreate.Currency)
        {
            throw new IllegalCurrencyException();
        }

        if (user.Expenses.Any(
                expense => Math.Abs(expense.Amount - expenseToCreate.Amount) == 0 &&
                           expense.Date == expenseToCreate.Date))
        {
            throw new DuplicatedExpenseException();
        }
    }
}
