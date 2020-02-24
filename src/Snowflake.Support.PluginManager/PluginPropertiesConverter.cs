using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Snowflake.Support.PluginManager
{
    internal class PluginPropertiesConverter : JsonConverter<PluginPropertiesData>
    {
        public override PluginPropertiesData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            PluginPropertiesData props = new PluginPropertiesData();
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.Comment)
                {
                    continue;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.Number:
                            string value = reader.TryGetInt64(out long l) ? l.ToString() : reader.GetDouble().ToString();
                            props.Strings.Add(propertyName, value);
                            break;
                        case JsonTokenType.True:
                        case JsonTokenType.False:
                            props.Strings.Add(propertyName, reader.GetBoolean().ToString());
                            break;
                        case JsonTokenType.String:
                        case JsonTokenType.Null:
                            props.Strings.Add(propertyName, reader.GetString());
                            break;
                        case JsonTokenType.StartObject:
                            props.Dictionaries.Add(propertyName, this.ParseDict(ref reader));
                            break;
                        case JsonTokenType.StartArray:
                            props.Arrays.Add(propertyName, this.ParseList(ref reader));
                            break;
                    }
                }
            }

            return props;
        }

        public override void Write(Utf8JsonWriter writer, PluginPropertiesData value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, string> ParseDict(ref Utf8JsonReader reader)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var dict = jsonDoc.RootElement.EnumerateObject()
                .ToDictionary(e => e.Name, e => e.Value.ToString()); // notice not GetString.
            return dict;
        }

        private List<string> ParseList(ref Utf8JsonReader reader)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var arr = jsonDoc.RootElement.EnumerateArray().Select(e => e.ToString()).ToList(); // notice not GetString.
            return arr;
        }
    }
}
