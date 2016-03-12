using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration;

namespace Snowflake.Configuration
{
    public abstract class ConfigurationSection : IConfigurationSection
    {
        public string SectionName { get; protected set; }
        public string DisplayName { get; protected set; }
        public string ConfigurationFileName { get; protected set; }
    }
}
