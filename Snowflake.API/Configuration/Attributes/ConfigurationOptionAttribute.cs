using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ConfigurationOptionAttribute : Attribute
    {
        public string OptionName { get; }

        public ConfigurationOptionAttribute(string optionName)
        {
            this.OptionName = optionName;
        }

    }
}
