using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationEntry : IConfigurationEntry
    {
        public string Description { get; }
        public string Name { get; }
        public dynamic DefaultValue { get; }
        public ConfigurationEntry(string description, string name, dynamic defaultValue)
        {
            this.Description = description;
            this.Name = name;
            this.DefaultValue = defaultValue;
        }

    }
}
