using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.PluginManager
{
    internal class PluginPropertiesData
    {
        internal PluginPropertiesData()
        {
            this.Strings = new Dictionary<string, string>();
            this.Dictionaries = new Dictionary<string, Dictionary<string, string>>();
            this.Arrays = new Dictionary<string, List<string>>();
        }

        public IDictionary<string, string> Strings { get; }
        public IDictionary<string, Dictionary<string, string>> Dictionaries { get; }
        public IDictionary<string, List<string>> Arrays { get; }
    }
}
