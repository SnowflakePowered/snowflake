using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.Resources
{
    public class Parameter
    {
        public string Key { get; }
        public Type Type { get; }

        public Parameter(Type type, string key)
        {
            this.Type = type;
            this.Key = key;
        }
    }
}
