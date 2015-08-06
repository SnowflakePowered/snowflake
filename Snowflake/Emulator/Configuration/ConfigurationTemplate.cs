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
        private IList<IConfigurationEntry> configurationEntries;
        public string TemplateID { get; private set; }
        public IList<IConfigurationEntry> ConfigurationEntries { get { return this.configurationEntries.AsReadOnly(); } }
        public string FileName { get; private set; }
        public IBooleanMapping BooleanMapping { get; private set; }
        public IConfigurationStore ConfigurationStore { get; private set; }
        public ConfigurationTemplate(string stringTemplate, IList<IConfigurationEntry> configurationEntries, IBooleanMapping booleanMapping, string fileName, string configurationName)
        {
            this.StringTemplate = stringTemplate;
            this.configurationEntries = configurationEntries;
            this.BooleanMapping = booleanMapping;
            this.FileName = fileName;
            this.TemplateID = configurationName;
            this.ConfigurationStore = new ConfigurationStore(this.GetDefaultProfile());
        }

        public static IConfigurationTemplate FromJsonProtoTemplate(IDictionary<string, dynamic> protoTemplate, EmulatorBridge bridge)
        {
            var booleanMapping = new BooleanMapping((string)protoTemplate["boolean"]["true"], (string)protoTemplate["boolean"]["false"]);
            var entries = new List<IConfigurationEntry>();
            string fileName = protoTemplate["filename"];
            string configName = protoTemplate["templateid"];
            string stringTemplate = bridge.GetStringResource($"{configName}.template");
            IDictionary<string, dynamic> defaults = new Dictionary<string, dynamic>();
           
            foreach (var value in protoTemplate["keys"])
            {
                entries.Add(new ConfigurationEntry((string)value.Value.description, (string)value.Name, (object)value.Value.defaultValue.Value));
            }
          
            return new ConfigurationTemplate(stringTemplate, entries, booleanMapping, fileName, configName);
        }

        /// <summary>
        /// Generates a default configuration profile from the defaultValues in the configuration entry
        /// </summary>
        /// <returns>A default configuration profile</returns>
        private IConfigurationProfile GetDefaultProfile()
        {
            IDictionary<string, dynamic> defaultValues = this.ConfigurationEntries.ToDictionary(entry => entry.Name, entry => entry.DefaultValue);
            return new ConfigurationProfile(this.TemplateID, defaultValues);
        }
    }
}
