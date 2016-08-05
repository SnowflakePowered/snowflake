using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration.Tests
{
    public sealed class ExampleConfigurationCollection: ConfigurationCollection
    {
        public ExampleConfigurationSection ExampleConfiguration { get; set; }
        public ExampleConfigurationCollection() : base(new IniConfigurationSerializer(new BooleanMapping("true", "false"), "null"), "test")
        {
        }
    }
}
