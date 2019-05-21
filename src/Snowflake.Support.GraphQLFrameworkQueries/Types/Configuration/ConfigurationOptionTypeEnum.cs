using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;

namespace Snowflake.Support.Remoting.GraphQL.Types.Configuration
{
    public class ConfigurationOptionPathTypeEnum : EnumerationGraphType<PathType>
    {
        public ConfigurationOptionPathTypeEnum()
        {
            Name = "ConfigurationOptionPathTypeEnum";
            Description = "The type of the path this path points to.";
        }
    }
}
