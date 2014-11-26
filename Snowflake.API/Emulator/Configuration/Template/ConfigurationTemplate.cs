using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Snowflake.Extensions;
using Snowflake.Emulator.Configuration.Mapping;
using Snowflake.Emulator.Configuration.Type;
using SharpYaml.Serialization;

namespace Snowflake.Emulator.Configuration.Template
{
    public class ConfigurationTemplate
    {
        public string StringTemplate { get; set; }
        private IDictionary<string, ConfigurationEntry> configurationEntries;
        private IDictionary<string, CustomType> customTypes;
        public string ConfigurationName { get; private set; }
        public IReadOnlyDictionary<string, ConfigurationEntry> ConfigurationEntries { get { return this.configurationEntries.AsReadOnly(); } }
        public string FileName { get; private set; }
        public BooleanMapping BooleanMapping { get; private set; }
        public IReadOnlyDictionary<string, CustomType> CustomTypes { get { return this.customTypes.AsReadOnly(); } }

        public ConfigurationTemplate(string stringTemplate, IDictionary<string, ConfigurationEntry> configurationEntries, IDictionary<string, CustomType> customTypes, BooleanMapping booleanMapping, string fileName, string configurationName)
        {
            this.StringTemplate = stringTemplate;
            this.configurationEntries = configurationEntries;
            this.customTypes = customTypes;
            this.BooleanMapping = booleanMapping;
            this.FileName = fileName;
            this.ConfigurationName = configurationName;
        }

        public static ConfigurationTemplate FromDictionary(IDictionary<string, dynamic> protoTemplate)
        {
            string stringTemplate = protoTemplate["template"];
            var booleanMapping = new BooleanMapping(protoTemplate["boolean"][true], protoTemplate["boolean"][false]);
            var types = new Dictionary<string, CustomType>();
            var entries = new Dictionary<string, ConfigurationEntry>();
            var fileName = protoTemplate["filename"];
            var configName = protoTemplate["configuration_name"];
            var defaults = new Dictionary<string, dynamic>();
            foreach (var value in protoTemplate["types"])
            {
                var typeValues = new List<CustomTypeValue>();
                foreach (var type in value.Value)
                {
                    typeValues.Add(new CustomTypeValue(type[0], type[1]));
                }
                types.Add(value.Key, new CustomType(value.Key, typeValues));
            }
            foreach (var value in protoTemplate["keys"])
            {
                entries.Add(value.Key, new ConfigurationEntry(value.Value["description"], value.Value["protection"], value.Value["type"], value.Key, value.Value["default"]));
            }
          
            return new ConfigurationTemplate(stringTemplate, entries, types, booleanMapping, fileName, configName);
        }

        private void ParseKeysAndTypesLINQ(IDictionary<string, dynamic> protoTemplate)
        {
            //this linq code is equivalent to the foreach loops in FromDictionary and is kept for reasons :3
            IDictionary<string, ConfigurationEntry> entries = (from keys in (IDictionary<object, object>)protoTemplate["keys"] select keys)
              .ToDictionary(value => (string)value.Key, value => ((IDictionary<object, object>)value.Value)
                  .ToDictionary(entry => (string)entry.Key, entry => (dynamic)entry.Value))
              .ToDictionary(value => value.Key, value => new ConfigurationEntry(value.Value["description"], value.Value["protection"], value.Value["type"], value.Key, value.Value["default"]));

         IDictionary<string, CustomType> types = (from value in (IDictionary<object, object>)protoTemplate["types"] select value)
             .ToDictionary(k => (string)k.Key, k => ((IList<object>)k.Value)
                 .Select(x => (IList<object>)x)
                 .Select(type => new CustomTypeValue((string)type[0], (string)type[1])))
             .ToDictionary(typeValues => typeValues.Key, typeValues => new CustomType(typeValues.Key, typeValues.Value.ToList()));
          
                
        }
    }
}
