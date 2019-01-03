using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;

namespace Snowflake.Support.Remoting.GraphQL.Inputs.Configuration
{
    public class ConfigurationValueInputObject : IConfigurationValue
    {
        public object Value { get; set; }
        public Guid Guid { get; set; }
    }
}
