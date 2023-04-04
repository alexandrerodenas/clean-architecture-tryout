using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WebUI.Integration.Tests;

public class DatabaseFixture : ICollectionFixture<WebApplicationFactory<Startup>>
{
    public readonly DatabaseContext DatabaseContext;
    public readonly HttpClient Client;

    private static readonly Guid StarkId = Guid.Parse("e479c7fb-1c49-4da5-8c9e-78b21d36c32d");
    private static readonly Guid NatashaId = Guid.Parse("7e1018cb-d07d-41c3-b28d-0d30a272b4a4");

    public static readonly User Stark = new()
    {
        Id = StarkId,
        FirstName = "Anthony",
        LastName = "Stark",
        Currency = Currency.USD,
        Expenses = new List<Expense>
        {
            new()
            {
                Id = Guid.Parse("b0d1745c-3c12-4725-8f9f-9b2f594c6f23"),
                Date = DateTime.Now.AddDays(-50),
                Currency = Currency.USD,
                Amount = 100,
                Commentary = "whatever",
                Type = ExpenseType.Hotel,
                UserId = StarkId
            },
            new()
            {
                Id = Guid.Parse("c77a27a7-7c06-48aa-99b0-8b9d49ed21f9"),
                Date = DateTime.Now,
                Currency = Currency.USD,
                Amount = 200,
                Commentary = "whatever",
                Type = ExpenseType.Restaurant,
                UserId = StarkId
            }
        }
    };

    public static readonly User Natasha = new()
    {
        Id = NatashaId,
        FirstName = "Natasha",
        LastName = "Romanova",
        Currency = Currency.RUB,
        Expenses = new List<Expense>
        {
            new()
            {
                Id = Guid.Parse("82bfbb91-e110-4079-95e8-43543d637482"),
                Date = DateTime.Now.AddDays(-500),
                Currency = Currency.RUB,
                Amount = 400,
                Commentary = "whatever",
                Type = ExpenseType.Hotel,
                UserId = NatashaId
            }
        }
    };

    public DatabaseFixture()
    {
        var factory = new WebApplicationFactory<Startup>();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryTestDB")
            .Options;
        DatabaseContext = new DatabaseContext(options);

        DatabaseContext.Users.AddRange(new List<User>
        {
            Natasha,
            Stark
        });

        DatabaseContext.SaveChanges();

        Client = factory.CreateClient();
    }

    public void Dispose()
    {
        DatabaseContext.Database.EnsureDeleted();
        DatabaseContext.Dispose();
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
