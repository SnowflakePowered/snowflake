using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Snowflake.Extensibility.Provisioning
{
    internal class PluginProperties : IPluginProperties
    {
        public PluginProperties(IDictionary<string, string> strings,
            IDictionary<string, Dictionary<string, string>> dicts, 
            IDictionary<string, List<string>> arrays)
        {
            this.Strings = strings;
            this.Dictionaries = dicts;
            this.Arrays = arrays;
            this.PropertyKeys = this.Strings.Keys.Concat(this.Dictionaries.Keys).Concat(this.Arrays.Keys).ToList();
        }

        public IEnumerable<string> PropertyKeys { get; }

        private IDictionary<string, string> Strings { get; }
        private IDictionary<string, Dictionary<string, string>> Dictionaries { get; }
        private IDictionary<string, List<string>> Arrays { get; }

        public string Get(string key) => this.Strings.TryGetValue(key, out var val) ? val : string.Empty;

        public IReadOnlyDictionary<string, string> GetDictionary(string key) => this.Dictionaries.TryGetValue(key, out var val) 
            ? (IReadOnlyDictionary<string, string>)val 
            : ImmutableDictionary<string, string>.Empty;

        public IEnumerable<string> GetEnumerable(string key) => this.Arrays.TryGetValue(key, out var val)
            ? val
            : Enumerable.Empty<string>();
    }
}
