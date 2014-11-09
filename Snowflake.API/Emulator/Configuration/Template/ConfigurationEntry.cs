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
        public readonly string ProtectionLevel;
        public readonly string Type;
        public readonly string Name;
        public ConfigurationEntry(string description, string protectionLevel, string type, string name)
        {
            this.Description = description;
            this.ProtectionLevel = protectionLevel;
            this.Type = type;
            this.Name = name;
        }

    }
}
