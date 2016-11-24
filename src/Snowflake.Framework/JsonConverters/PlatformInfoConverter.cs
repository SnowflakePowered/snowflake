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
            IEnumerable<string> biosFiles = jObject.Value<JArray>("BiosFiles")?.Values<string>()
                ?? new List<string>();
            return new PlatformInfo(platformId, friendlyName, metadata, fileTypes, biosFiles, maximumInputs);
        }
    }
}
