using Application;
using Domain;

namespace WebUI;

public class DbSeeder: IDbSeeder
{
    private readonly IApplicationDbContext _context;

    public DbSeeder(IApplicationDbContext context)
    {
        _context = context;
    }
    public void Seed()
    {
        if (_context.Users.Any())
        {
            return;
        }

        Guid starkId = Guid.Parse("d3c48410-bec0-11ed-afa1-0242ac120002");
        Guid romanovaId = Guid.Parse("eb623f7c-bec0-11ed-afa1-0242ac120002");
        List<User> usersToSeed = new List<User>
        {
            new()
            {
                Id = starkId,
                FirstName = "Anthony",
                LastName = "Stark",
                Currency = Currency.USD,
                Expenses = new List<Expense>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(-2),
                        Currency = Currency.USD,
                        Amount = 100,
                        Commentary = "a commentary",
                        Type = ExpenseType.Hotel,
                        UserId = starkId
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(-5),
                        Currency = Currency.USD,
                        Amount = 40,
                        Commentary = "another commentary",
                        Type = ExpenseType.Restaurant,
                        UserId = starkId
                    }
                }
            },
            new()
            {
                Id = romanovaId,
                FirstName = "Natasha",
                LastName = "Romanova",
                Currency = Currency.RUB,
                Expenses = new List<Expense>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now.AddDays(-8),
                        Currency = Currency.RUB,
                        Amount = 150,
                        Commentary = "a commentary",
                        Type = ExpenseType.Misc,
                        UserId = romanovaId
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now.AddYears(-2),
                        Currency = Currency.RUB,
                        Amount = 950,
                        Commentary = "another commentary",
                        Type = ExpenseType.Hotel,
                        UserId = romanovaId
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Amount = 250,
                        Commentary = "yet another commentary",
                        Currency = Currency.RUB,
                        Date = DateTime.Now.AddDays(-18),
                        Type = ExpenseType.Misc,
                        UserId = romanovaId
                    }
                }
            }
        };

        _context.Users.AddRange(usersToSeed);
        _context.SaveChanges();
    }
}
