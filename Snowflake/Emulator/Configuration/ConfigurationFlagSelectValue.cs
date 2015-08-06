using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationFlagSelectValue : IConfigurationFlagSelectValue
    {
        public string Value { get; }
        public string Description { get; set; }
        public ConfigurationFlagSelectValue(string value, string description)
        {
            this.Value = value;
            this.Description = description;
        }
    }
}
