using System.Text.Json.Serialization;
using Domain;

namespace Application;

public class ExpenseOutput
{
    [JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime Date { get; }
    public string Currency { get; }
    public float Amount { get; }
    public string Commentary { get; }
    public string ExpenseType { get; }

    public ExpenseOutput(
        DateTime date,
        string currency,
        float amount,
        string commentary,
        string expenseType
    )
    {
        Date = date;
        Currency = currency;
        Amount = amount;
        Commentary = commentary;
        ExpenseType = expenseType;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var otherExpenseOutput = (ExpenseOutput)obj;

        return Date == otherExpenseOutput.Date &&
               Currency == otherExpenseOutput.Currency &&
               Math.Abs(Amount - otherExpenseOutput.Amount) <= 0 &&
               Commentary == otherExpenseOutput.Commentary &&
               ExpenseType == otherExpenseOutput.ExpenseType;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Date, Currency, Amount, Commentary, ExpenseType);
    }

    public static ExpenseOutput FromDomain(Expense expense)
    {
        return new ExpenseOutput(
            expense.Date,
            expense.Currency.ToString(),
            expense.Amount,
            expense.Commentary,
            expense.Type.ToString()
        );
    }

}
