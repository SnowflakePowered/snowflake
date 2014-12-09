using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Configuration.Template
{
    public class ConfigurationEntry
    {
        public readonly string Description;
        public readonly string Name;
        public readonly dynamic DefaultValue;
        public ConfigurationEntry(string description, string name, dynamic defaultValue)
        {
            this.Description = description;
            this.Name = name;
            this.DefaultValue = defaultValue;
        }

    }
}
