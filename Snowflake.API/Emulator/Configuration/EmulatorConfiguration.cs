using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Mapping;
using Snowflake.Emulator.Configuration.Type;
using Snowflake.Emulator.Configuration.Template;

using SharpYaml.Serialization;

namespace Snowflake.Emulator.Configuration
{
    public class EmulatorConfiguration 
    {

        public IDictionary<string, dynamic> Keys;
        private ConfigurationTemplate template;
        public string Compile()
        {
         //fix this pls. 
            var builder = new StringBuilder(this.template.StringTemplate);
            foreach (ConfigurationEntry entry in this.template.ConfigurationEntries)
            {
                var value = this.Keys.ContainsKey(entry.Name) ? this.Keys[entry.Name] : entry.DefaultValue;
                string input;
                switch (entry.Type)
                {
                    case "boolean":
                        input = this.template.BooleanMapping.FromBool((bool)value);
                        break;
                    default:
                        input = value.ToString();
                        break;
                }

                builder.Replace("{" + entry.Name + "}", input);
            }
            return builder.ToString();
        }

        private void IncludeDefaults()
        {
            foreach (ConfigurationEntry entry in this.template.ConfigurationEntries)
            {
                if(!this.Keys.ContainsKey(entry.Name)){
                    this.Keys.Add(entry.Name, entry.DefaultValue);
                }

            }
        }
        public EmulatorConfiguration(ConfigurationTemplate template, IDictionary<string, dynamic> keys)
        {
            this.Keys = new Dictionary<string, dynamic>();
            this.template = template;
            this.IncludeDefaults();
        }

    }
}
