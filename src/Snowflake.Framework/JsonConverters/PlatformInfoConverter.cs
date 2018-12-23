using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Platform;
using IPlatformInfo = Snowflake.Platform.IPlatformInfo;

namespace Snowflake.JsonConverters
{
    internal class PlatformInfoConverter : JsonCreationConverter<IPlatformInfo>
    {
        /// <inheritdoc/>
        protected override IPlatformInfo Create(Type objectType, JObject jObject)
        {
            string platformId = jObject.Value<string>("PlatformID");
            string friendlyName = jObject.Value<string>("FriendlyName");
            IDictionary<string, string> metadata = jObject.Value<JToken>("Metadata").ToObject<IDictionary<string, string>>();
            int maximumInputs = jObject.Value<int>("MaximumInputs");
            IDictionary<string, string> fileTypes = jObject.Value<JToken>("FileTypes").ToObject<IDictionary<string, string>>();

            var biosProps = jObject.Value<JToken>("BiosFiles")?.Values<JProperty>()?.Select(p => p.Value<JProperty>());
            var biosFiles = jObject.Value<JToken>("BiosFiles") != null
                ? (from property in biosProps
                   from hash in property?.Values<JToken>().Values<string>().DefaultIfEmpty(string.Empty)
                   select new { FileName = property?.Name, Hash = hash })?
                   .Select(p => new BiosFile(p.FileName, p.Hash))
                   .ToList()
                   : Enumerable.Empty<IBiosFile>();

            return new PlatformInfo(platformId, friendlyName, metadata, fileTypes, biosFiles, maximumInputs);
        }
    }
}
