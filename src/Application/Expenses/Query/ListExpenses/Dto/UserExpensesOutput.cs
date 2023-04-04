namespace Application;

public class UserExpensesOutput
{
    public string UserNames { get; init; }
    public List<ExpenseOutput> Expenses { get; init; }

    public override int GetHashCode()
    {
        return HashCode.Combine(UserNames, Expenses);
    }
}
