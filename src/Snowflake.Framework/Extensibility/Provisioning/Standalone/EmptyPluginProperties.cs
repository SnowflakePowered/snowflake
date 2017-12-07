using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Snowflake.Extensibility.Provisioning.Standalone
{
    internal class EmptyPluginProperties : IPluginProperties
    {
        private static EmptyPluginProperties emptyProperties = new EmptyPluginProperties();
        public static EmptyPluginProperties EmptyProperties => EmptyPluginProperties.emptyProperties;

        public IEnumerable<string> PropertyKeys => Enumerable.Empty<string>();

        public string Get(string key)
        {
            return null;
        }

        public IDictionary<string, string> GetDictionary(string key)
        {
            return ImmutableDictionary.Create<string, string>();
        }

        public IEnumerable<string> GetEnumerable(string key)
        {
            yield break;
        }
    }
}
