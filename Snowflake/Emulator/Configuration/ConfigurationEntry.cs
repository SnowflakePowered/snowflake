using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationEntry : IConfigurationEntry
    {
        public string Description { get; private set; }
        public string Name { get; private set; }
        public dynamic DefaultValue { get; private set; }
        public ConfigurationEntry(string description, string name, dynamic defaultValue)
        {
            this.Description = description;
            this.Name = name;
            this.DefaultValue = defaultValue;
        }

    }
}
