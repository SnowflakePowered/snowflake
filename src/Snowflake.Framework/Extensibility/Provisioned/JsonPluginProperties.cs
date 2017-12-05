using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Snowflake.Extensibility.Provisioned
{
    public class JsonPluginProperties : IPluginProperties
    {
        private readonly JObject propertyRoot;
        public JsonPluginProperties(JObject propertyRoot)
        {
            this.propertyRoot = propertyRoot;
        }

        /// <inheritdoc/>
        public IEnumerable<string> PropertyKeys => (this.propertyRoot as IDictionary<string, JToken>).Keys;

        /// <inheritdoc/>
        public string Get(string key) => this.propertyRoot[key]?.Value<string>();

        /// <inheritdoc/>
        public IEnumerable<string> GetEnumerable(string key)
        {
            return this.propertyRoot.Value<JArray>(key)?.Values<string>() ?? Enumerable.Empty<string>();
        }

        /// <inheritdoc/>
        public IDictionary<string, string> GetDictionary(string key)
        {
            return this.propertyRoot.Value<JToken>(key)?.ToObject<IDictionary<string, string>>() ?? new Dictionary<string, string>();
        }
    }
}
