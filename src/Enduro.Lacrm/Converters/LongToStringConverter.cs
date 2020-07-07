using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Enduro.Lacrm.Converters
{
    public class NumberToStringConverter : JsonConverter<string?>
    {
        public override string? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.Number:
                    var val = reader.GetInt64().ToString();
                    return val;
                default:
                    return null;
            }
        }

        public override void Write(
            Utf8JsonWriter writer, 
            string? value, 
            JsonSerializerOptions options) 
            => writer.WriteStringValue(value);
    }
}