namespace Domain;

public class User : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Currency Currency { get; set; }
    public List<Expense> Expenses { get; set; } = new();
}
