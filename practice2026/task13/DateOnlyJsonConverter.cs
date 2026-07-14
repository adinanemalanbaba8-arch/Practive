using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public class DateOnlyJsonConverter : JsonConverter<DateTime>
{
    private const string Format = "dd.MM.yyyy";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            throw new JsonException("Дата не может быть пустой.");
        }

        return DateTime.ParseExact(value, Format, System.Globalization.CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format, System.Globalization.CultureInfo.InvariantCulture));
        // test CI
    }
}
