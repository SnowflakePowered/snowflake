using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    internal class ConfigurationProperty : IConfigurationProperty
    {
        public object Value { get; }
        public ConfigurationOptionAttribute Metadata { get; }

        public ConfigurationProperty(object value, ConfigurationOptionAttribute metadata)
        {
            this.Value = value;
            this.Metadata = metadata;
        }
    }
}
