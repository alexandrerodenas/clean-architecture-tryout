using System.Net;
using Domain;
using Xunit;

namespace WebUI.Integration.Tests;


[Collection("Database collection")]
public class ListUserExpensesControllerIntegrationTest
{
    private readonly HttpClient _client;
    private readonly User _natasha = DatabaseFixture.Natasha;
    private readonly Expense _natashaExpense = DatabaseFixture.Natasha.Expenses[0];

    public ListUserExpensesControllerIntegrationTest(DatabaseFixture databaseFixture)
    {
        _client = databaseFixture.Client;
    }

    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when getting natasha expenses " +
                        "then it returns expected expenses.")]
    public async Task CanListUserExpenses()
    {
        HttpResponseMessage response = await _client
            .GetAsync($"/users/{_natasha.Id}/expenses?sortBy=ASC&orderBy=DATE");
        response.EnsureSuccessStatusCode();
        string contentStream = await response.Content.ReadAsStringAsync();

        string expectedOutput =
            $@"{{""userNames"":""{_natasha.FirstName} {_natasha.LastName}"",""expenses"":[{{""date"":""{_natashaExpense.Date:dd/MM/yyyy HH:mm:ss}"",""currency"":""{_natashaExpense.Currency}"",""amount"":{_natashaExpense.Amount},""commentary"":""{_natashaExpense.Commentary}"",""expenseType"":""{_natashaExpense.Type}""}}]}}";
        Assert.Equal(expectedOutput, contentStream);
    }

    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when getting an unknown user id " +
                        "then an error is thrown.")]
    public async Task NotFoundUserOnUnknownId()
    {
        Guid anUnknownId = Guid.NewGuid();

        HttpResponseMessage response = await _client
            .GetAsync($"/users/{anUnknownId}/expenses?sortBy=ASC&orderBy=DATE");
        Assert.True(response.StatusCode == HttpStatusCode.NotFound);

        string content = await response.Content.ReadAsStringAsync();
        Assert.Contains("User not found.", content);
    }

    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when getting natasha expenses but with inconsistent listing options (sortBy is null) " +
                        "then an error is thrown.")]
    public async Task SortByCannotBeNullWhenOrderByIsNot()
    {
        HttpResponseMessage response = await _client
            .GetAsync($"/users/{_natasha.Id}/expenses?orderBy=DATE");
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

        string content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Sort by cannot be null if Order by is not", content);
    }


    [Fact(DisplayName = "Given database populated with stark and natasha " +
                        "both having expenses in database " +
                        "when getting natasha expenses but with inconsistent listing options (orderBy is null) " +
                        "then an error is thrown.")]
    public async Task OrderByCannotBeNullWhenSortByIsNot()
    {
        HttpResponseMessage response = await _client
            .GetAsync($"/users/{_natasha.Id}/expenses?sortBy=ASC");
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);

        string content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Order by cannot be null if Sort by is not", content);
    }
}
