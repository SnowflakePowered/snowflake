using GraphQL.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Configuration
{
    public class ConfigurationOptionTypeEnum : EnumerationGraphType<ConfigurationOptionType>
    {
        public ConfigurationOptionTypeEnum()
        {
            Name = "ConfigurationOptionTypeEnum";
            Description = "The valid types of configuration values";
        }
    }
}
