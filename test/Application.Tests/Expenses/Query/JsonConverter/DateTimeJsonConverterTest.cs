using System.Text.Json;
using Xunit;

namespace Application.Tests;

public class DateTimeJsonConverterTest
{
    [Fact(DisplayName = "Given a date time" +
                        "when serializing it" +
                        "then it returns a string in expected format.")]
    public void CanSerializeDateTime()
    {
        DateTime aDateTime = new DateTime(
            1990,
            1,
            21,
            10,
            50,
            55
        );

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new DateTimeJsonConverter() }
        };
        string aDateTimeAsString = JsonSerializer
            .Serialize(aDateTime, options);

        Assert.Equal("\"21/01/1990 10:50:55\"", aDateTimeAsString);
    }

    [Fact(DisplayName = "Given a string representing a date time" +
                        "when deserializing it" +
                        "then it returns expected date time.")]
    public void CanDeserializeDateTime()
    {
        string aDateTimeAsString = "\"30/01/2000 12:10:55\"";

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new DateTimeJsonConverter() }
        };
        DateTime aDateTime = JsonSerializer
            .Deserialize<DateTime>(aDateTimeAsString, options);

        Assert.Equal(
            new DateTime(
                2000,
                1,
                30,
                12,
                10,
                55
            ),
            aDateTime
        );
    }
}
