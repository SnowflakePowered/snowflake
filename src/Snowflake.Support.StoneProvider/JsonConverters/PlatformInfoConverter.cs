using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Snowflake.Support.StoneProvider.JsonConverters
{
    internal class PlatformInfoConverter : JsonConverter<PlatformInfo>
    {
        public override PlatformInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var rootElem = jsonDoc.RootElement;
            string platformId = rootElem.GetProperty("PlatformID").GetString()!;
            string friendlyName = rootElem.GetProperty("FriendlyName").GetString()!;

            IDictionary<string, string> metadata = rootElem.GetProperty("Metadata").
                EnumerateObject().ToDictionary(p => p.Name, p => p.Value.GetString()!);

            int maximumInputs = rootElem.GetProperty("MaximumInputs").GetInt32();

            IDictionary<string, string> fileTypes = rootElem.GetProperty("FileTypes").
               EnumerateObject().ToDictionary(p => p.Name, p => p.Value.GetString()!);
            IEnumerable<ISystemFile> biosFiles = rootElem.TryGetProperty("BiosFiles", out var biosFileElem) ?
                (from biosFile in biosFileElem.EnumerateObject()
                let noHashes = biosFile.Value.GetArrayLength() == 0
                from hash in biosFile.Value.EnumerateArray()
                select new BiosFile(biosFile.Name, noHashes ? string.Empty : hash.GetString()!)).ToList()
                    : Enumerable.Empty<ISystemFile>();
           
            return new PlatformInfo(platformId, friendlyName, metadata, fileTypes, biosFiles, maximumInputs);
        }

        public override void Write(Utf8JsonWriter writer, PlatformInfo value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
