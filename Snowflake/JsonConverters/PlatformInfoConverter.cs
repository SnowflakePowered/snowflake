using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
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
            IEnumerable<string> fileExtensions = jObject.Value<JToken>("FileExtensions").ToObject<IEnumerable<string>>();
            return new PlatformInfo(platformId, friendlyName, metadata, fileExtensions, maximumInputs);
        }
    }
}
