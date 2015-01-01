using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Snowflake.Extensions;
using Snowflake.Emulator.Configuration;
using SharpYaml.Serialization;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationTemplate : IConfigurationTemplate
    {
        public string StringTemplate { get; set; }
        private IList<ConfigurationEntry> configurationEntries;
        public string ConfigurationName { get; private set; }
        public IList<ConfigurationEntry> ConfigurationEntries { get { return this.configurationEntries.AsReadOnly(); } }
        public string FileName { get; private set; }
        public BooleanMapping BooleanMapping { get; private set; }

        public ConfigurationTemplate(string stringTemplate, IList<ConfigurationEntry> configurationEntries, BooleanMapping booleanMapping, string fileName, string configurationName)
        {
            this.StringTemplate = stringTemplate;
            this.configurationEntries = configurationEntries;
            this.BooleanMapping = booleanMapping;
            this.FileName = fileName;
            this.ConfigurationName = configurationName;
        }

        public static ConfigurationTemplate FromDictionary(IDictionary<string, dynamic> protoTemplate)
        {
            string stringTemplate = protoTemplate["template"];
            var booleanMapping = new BooleanMapping(protoTemplate["boolean"][true], protoTemplate["boolean"][false]);
            var entries = new List<ConfigurationEntry>();
            var fileName = protoTemplate["filename"];
            var configName = protoTemplate["configuration_name"];
            var defaults = new Dictionary<string, dynamic>();
           
            foreach (var value in protoTemplate["keys"])
            {
                entries.Add(new ConfigurationEntry(value.Value["description"], value.Key, value.Value["default"]));
            }
          
            return new ConfigurationTemplate(stringTemplate, entries, booleanMapping, fileName, configName);
        }
    }
}
