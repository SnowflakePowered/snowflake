using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.DynamicConfiguration
{
    public class ConfigurationValue
    {
        public object Value { get; set; }
        public ConfigurationOption Option { get; }
        public Guid Guid { get; }

        public ConfigurationValue(ConfigurationOption option, object value) : this(option, value, Guid.NewGuid())
        {
        }

        public ConfigurationValue(ConfigurationOption option, object value, Guid guid)
        {
            this.Guid = guid;
            this.Option = option;
            this.Value = value;
        }
    }
}
