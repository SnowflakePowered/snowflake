using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.Marshalling
{
    public class SerializedArgument
    {
        public string Key { get; }
        public string StringValue { get; }
        public Type Type { get; }

        public SerializedArgument(string key, string strValue, Type type)
        {
            this.Key = key;
            this.StringValue = strValue;
            this.Type = type;
        }
    }
}
