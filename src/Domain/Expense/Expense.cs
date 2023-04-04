namespace Domain;

public class Expense : Entity
{
    public DateTime Date { get; init; }
    public Currency Currency { get; init; }
    public float Amount { get; init; }
    public string Commentary { get; init; }
    public ExpenseType Type { get; init; }
    public Guid UserId { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var otherExpense = (Expense)obj;

        return Id == otherExpense.Id &&
               Date == otherExpense.Date &&
               Currency == otherExpense.Currency &&
               Math.Abs(Amount - otherExpense.Amount) <= 0 &&
               Commentary == otherExpense.Commentary &&
               Type == otherExpense.Type &&
               UserId == otherExpense.UserId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Date, Amount);
    }
}
