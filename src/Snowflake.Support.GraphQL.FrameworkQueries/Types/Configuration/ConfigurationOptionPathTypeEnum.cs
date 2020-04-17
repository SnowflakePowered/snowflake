using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Configuration
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
