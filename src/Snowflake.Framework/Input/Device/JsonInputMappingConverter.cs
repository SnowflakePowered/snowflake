using Snowflake.Configuration.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EnumsNET;

namespace Snowflake.Input.Device
{
    internal sealed class JsonInputMappingConverter
        : JsonConverter<DictionaryInputMapping>
    {
        public override DictionaryInputMapping? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var rootElem = jsonDoc.RootElement;
            string? driverString = rootElem.GetProperty("driver").GetString();
            InputDriver driver = Enums.Parse<InputDriver>(driverString!);

            var controllerElements = rootElem.GetProperty("mappings").EnumerateObject()
                .Select(prop => (element: Enums.Parse<DeviceCapability>(prop.Name), value: prop.Value.GetString()!))
                .ToDictionary(o => o.element, o => o.value);
            return new DictionaryInputMapping(driver, controllerElements);
        }

        public override void Write(Utf8JsonWriter writer, DictionaryInputMapping value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
