using System.Net;
using System.Text;
using System.Text.Json;
using Application.Expenses.Commands.CreateExpense;
using Domain;

using Xunit;

namespace WebUI.Integration.Tests;

[Collection("Database collection")]
public class CreateUserExpenseControllerIntegrationTest
{
    private readonly HttpClient _client;
    private readonly User _stark = DatabaseFixture.Stark;

    public CreateUserExpenseControllerIntegrationTest(DatabaseFixture databaseFixture)
    {
        _client = databaseFixture.Client;
    }

    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when creating a new valid stark expense " +
                        "then it returns id of new created expense.")]
    public async Task CanCreateNewExpenseForUser()
    {
        CreateExpenseCommand createExpenseCommand = new()
        {
            Amount = 100,
            Commentary = "new expense",
            Currency = _stark.Currency.ToString(),
            Date = DateTime.Today.AddDays(-1),
            Type = "Hotel",
            UserId = _stark.Id
        };
        var json = JsonSerializer.Serialize(createExpenseCommand);

        var jsonAsStringContent = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage creationResponse = await _client
            .PostAsync($"/users/{_stark.Id}/expenses", jsonAsStringContent);
        creationResponse.EnsureSuccessStatusCode();

        string responseContent = await creationResponse.Content.ReadAsStringAsync();
        string deserialized = JsonSerializer.Deserialize<string>(responseContent)!;

        Assert.True(Guid.TryParse(deserialized, out _));
    }

    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when creating new expense with date in future " +
                        "then it returns bad request error.")]
    public async Task CannotCreateExpenseWithDateInFuture()
    {
        var DateInFuture = DateTime.Today.AddDays(1);
        CreateExpenseCommand createExpenseCommand = new()
        {
            Amount = 100,
            Commentary = "new expense",
            Currency = _stark.Currency.ToString(),
            Date = DateInFuture,
            Type = "Hotel",
            UserId = _stark.Id
        };
        var json = JsonSerializer.Serialize(createExpenseCommand);

        var jsonAsStringContent = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage creationResponse = await _client
            .PostAsync($"/users/{_stark.Id}/expenses", jsonAsStringContent);
        Assert.Equal(HttpStatusCode.BadRequest, creationResponse.StatusCode);
        string creationResponseContent = await creationResponse.Content.ReadAsStringAsync();
        Assert.Contains("Date cannot be in future.", creationResponseContent);
    }

    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when creating new expense with date being more than three months old " +
                        "then it returns bad request error.")]
    public async Task CannotCreateExpenseWithDateTooOld()
    {
        var DateTooOld = DateTime.Today.AddMonths(-3).AddDays(-1);
        CreateExpenseCommand createExpenseCommand = new()
        {
            Amount = 100,
            Commentary = "new expense",
            Currency = _stark.Currency.ToString(),
            Date = DateTooOld,
            Type = "Hotel",
            UserId = _stark.Id
        };
        var json = JsonSerializer.Serialize(createExpenseCommand);

        var jsonAsStringContent = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage creationResponse = await _client
            .PostAsync($"/users/{_stark.Id}/expenses", jsonAsStringContent);
        Assert.Equal(HttpStatusCode.BadRequest, creationResponse.StatusCode);
        string creationResponseContent = await creationResponse.Content.ReadAsStringAsync();
        Assert.Contains("Date cannot older than three months ago.", creationResponseContent);
    }

    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when creating new expense with empty commentary " +
                        "then it returns bad request error.")]
    public async Task CannotCreateExpenseWithEmptyCommentary()
    {
        var emptyCommentary = "";
        CreateExpenseCommand createExpenseCommand = new()
        {
            Amount = 100,
            Commentary = emptyCommentary,
            Currency = _stark.Currency.ToString(),
            Date = DateTime.Today.AddDays(-1),
            Type = "Hotel",
            UserId = _stark.Id
        };
        var json = JsonSerializer.Serialize(createExpenseCommand);

        var jsonAsStringContent = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage creationResponse = await _client
            .PostAsync($"/users/{_stark.Id}/expenses", jsonAsStringContent);
        Assert.Equal(HttpStatusCode.BadRequest, creationResponse.StatusCode);
        string creationResponseContent = await creationResponse.Content.ReadAsStringAsync();
        Assert.Contains("Commentary must be defined.", creationResponseContent);
    }

    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when creating a duplicated stark expense " +
                        "then it returns id of new created expense.")]
    public async Task CannotCreateTwiceTheSameExpense()
    {
        var existingStarkExpense = _stark.Expenses[0];
        CreateExpenseCommand createExpenseCommand = new()
        {
            Amount = existingStarkExpense.Amount,
            Commentary = existingStarkExpense.Commentary,
            Currency = _stark.Currency.ToString(),
            Date = existingStarkExpense.Date,
            Type = existingStarkExpense.Type.ToString(),
            UserId = _stark.Id
        };
        var json = JsonSerializer.Serialize(createExpenseCommand);

        var jsonAsStringContent = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage creationResponse = await _client
            .PostAsync($"/users/{_stark.Id}/expenses", jsonAsStringContent);
        Assert.Equal(HttpStatusCode.BadRequest, creationResponse.StatusCode);
        string creationResponseContent = await creationResponse.Content.ReadAsStringAsync();
        Assert.Contains("Cannot have duplicated expenses.", creationResponseContent);
    }


    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when creating a duplicated stark expense " +
                        "then it returns id of new created expense.")]
    public async Task CannotHaveInconsistentUserId()
    {
        var anotherUserId = Guid.NewGuid();
        CreateExpenseCommand createExpenseCommand = new()
        {
            Amount = 100,
            Commentary = "a commentary",
            Currency = _stark.Currency.ToString(),
            Date = DateTime.Today.AddDays(-1),
            Type = "Hotel",
            UserId = anotherUserId
        };
        var json = JsonSerializer.Serialize(createExpenseCommand);

        var jsonAsStringContent = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage creationResponse = await _client
            .PostAsync($"/users/{_stark.Id}/expenses", jsonAsStringContent);
        Assert.Equal(HttpStatusCode.BadRequest, creationResponse.StatusCode);
        string creationResponseContent = await creationResponse.Content.ReadAsStringAsync();
        Assert.Contains("User id inconsistent.", creationResponseContent);
    }
}
