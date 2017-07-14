using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.Marshalling
{
    public class EndpointArgument
    {
        public string Key { get; }
        public string StringValue { get; }

        public EndpointArgument(string key, string value)
        {
            this.Key = key;
            this.StringValue = value;
        }
    }
}
