using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Snowflake.Input.Controller;
using Snowflake.Platform;

namespace Snowflake.JsonConverters
{
    public class PlatformInfoConverter : JsonCreationConverter<IPlatformInfo>
    {
        protected override IPlatformInfo Create(Type objectType, JObject jObject)
        {
            string platformId = jObject.Value<string>("PlatformID");
            string friendlyName = jObject.Value<string>("FriendlyName");
            IDictionary<string, string> metadata = jObject.Value<JToken>("Metadata").ToObject<IDictionary<string, string>>();
            int maximumInputs = jObject.Value<int>("MaximumInputs");
            IDictionary<string, string> fileTypes = jObject.Value<JToken>("FileTypes").ToObject<IDictionary<string, string>>();

            var biosProps = jObject.Value<JToken>("BiosFiles")?.Values<JProperty>()?.Select(p => p.Value<JProperty>());
            var biosFiles = (jObject.Value<JToken>("BiosFiles") != null
                ? (from property in biosProps
                   from hash in property?.Values<JToken>().Values<string>().DefaultIfEmpty("")
                   select new { FileName = property?.Name, Hash = hash })?.ToLookup(p => p.FileName, p => p.Hash)
                             : EmptyLookup<string, string>.Instance);
            return new PlatformInfo(platformId, friendlyName, metadata, fileTypes, biosFiles, maximumInputs);
        }
    }
}