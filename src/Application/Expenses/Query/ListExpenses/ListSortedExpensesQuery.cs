using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application;

public record ListSortedExpensesQuery : IRequest<UserExpensesOutput>
{
    public Guid UserId { get; init; }
    public OrderBy? OrderBy { get; init; }
    public SortBy? SortBy { get; init; }
}

public class ListSortedExpensesQueryHandler : IRequestHandler<ListSortedExpensesQuery, UserExpensesOutput>
{
    private readonly IApplicationDbContext _context;

    public ListSortedExpensesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserExpensesOutput> Handle(ListSortedExpensesQuery request, CancellationToken cancellationToken)
    {
        var user = await _context
            .Users
            .Include(user => user.Expenses)
            .FirstOrDefaultAsync(
                x => x.Id == request.UserId,
                cancellationToken: cancellationToken
            )
            .ConfigureAwait(false) ?? throw new UserNotFoundException();

        var sortedExpensesOfUser = ApplySortAndOrder(request, user.Expenses);

        return new UserExpensesOutput
        {
            UserNames = $"{user.FirstName} {user.LastName}",
            Expenses = sortedExpensesOfUser.Select(ExpenseOutput.FromDomain).ToList()
        };
    }

    private static List<Expense> ApplySortAndOrder(
        ListSortedExpensesQuery request,
        List<Expense> expensesOfUser
    )
    {
        switch (request.SortBy)
        {
            case SortBy.ASC:
                return request.OrderBy switch
                {
                    OrderBy.DATE => expensesOfUser.OrderBy(expense => expense.Date).ToList(),
                    _ => expensesOfUser.OrderBy(expense => expense.Amount).ToList()
                };
            default:
                return request.OrderBy switch
                {
                    OrderBy.DATE => expensesOfUser.OrderByDescending(expense => expense.Date).ToList(),
                    _ => expensesOfUser.OrderByDescending(expense => expense.Amount).ToList()
                };
        }
    }
}
