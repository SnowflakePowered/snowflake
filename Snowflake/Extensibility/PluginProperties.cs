using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Snowflake.Extensibility
{
    internal class JsonPluginProperties : IPluginProperties
    {
        private readonly JObject propertyRoot;
        public JsonPluginProperties(JObject propertyRoot)
        {
            this.propertyRoot = propertyRoot;
        }

        public IEnumerable<string> PropertyKeys => (this.propertyRoot as IDictionary<string, JToken>).Keys;
        public string Get(string key) => this.propertyRoot[key].Value<string>();

        public IEnumerable<string> GetEnumerable(string key)
        {
            return this.propertyRoot.Value<JArray>(key).Values<string>();
        }

        public IDictionary<string, string> GetDictionary(string key)
        {
            return this.propertyRoot.Value<JToken>(key).ToObject<IDictionary<string, string>>();
        }
    }
}
