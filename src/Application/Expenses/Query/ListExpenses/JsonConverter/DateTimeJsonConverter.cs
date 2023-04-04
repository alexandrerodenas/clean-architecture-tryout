using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application;

public class DateTimeJsonConverter : JsonConverter<DateTime>
{
    private const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        return DateTime.ParseExact(
            reader.GetString()!,
            DateTimeFormat,
            CultureInfo.InvariantCulture
        );
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTime dateTime,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(
            dateTime.ToString(
                DateTimeFormat, CultureInfo.InvariantCulture
            )
        );
    }
}
