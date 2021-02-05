using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Snowflake.Support.StoneProvider.JsonConverters
{
    internal class ControllerLayoutConverter : JsonConverter<ControllerLayout>
    {
        public override ControllerLayout Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var rootElem = jsonDoc.RootElement;
            string layoutName = rootElem.GetProperty("LayoutID").GetString()!;
            string friendlyName = rootElem.GetProperty("FriendlyName").GetString()!;

            IEnumerable<PlatformId> platforms = rootElem.GetProperty("Platforms")
                .EnumerateArray().Select(s => (PlatformId)s.GetString()!)
                .ToImmutableArray();

            IEnumerable<(ControllerElement element, ControllerElementInfo info)> elements =
                from layoutElem in rootElem.GetProperty("Layout").EnumerateObject()
                let elementKey = ControllerElementExtensions.Parse(layoutElem.Name)
                let elementInfoElem = layoutElem.Value
                let elementLabel = elementInfoElem.GetProperty("Label").GetString()
                let elementType = ControllerElementTypeExtensions.Parse(elementInfoElem.GetProperty("Type").GetString()!)
                select (elementKey, elementInfo: new ControllerElementInfo(elementLabel, elementType));

            var layout = new ControllerElementCollection();
           
            foreach (var (elementKey, elementInfo) in elements)
            {
                layout.Add(elementKey, elementInfo);
            }

            return new ControllerLayout(layoutName, platforms, friendlyName, layout);
        }

        public override void Write(Utf8JsonWriter writer, ControllerLayout value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
