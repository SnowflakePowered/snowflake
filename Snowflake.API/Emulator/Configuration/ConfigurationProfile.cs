using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Mapping;
using Snowflake.Emulator.Configuration.Template;

using SharpYaml.Serialization;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationProfile 
    {

        public IDictionary<string, dynamic> Keys;
        private ConfigurationTemplate template;
      
        private void IncludeDefaults()
        {
            foreach (ConfigurationEntry entry in this.template.ConfigurationEntries)
            {
                if(!this.Keys.ContainsKey(entry.Name)){
                    this.Keys.Add(entry.Name, entry.DefaultValue);
                }

            }
        }
        public ConfigurationProfile(ConfigurationTemplate template, IDictionary<string, dynamic> keys)
        {
            this.Keys = new Dictionary<string, dynamic>();
            this.template = template;
            this.IncludeDefaults();
        }

    }
}
