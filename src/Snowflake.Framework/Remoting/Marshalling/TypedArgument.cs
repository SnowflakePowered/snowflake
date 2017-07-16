using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Marshalling
{
    public class TypedArgument : ITypedArgument
    {
        public string Key { get; }
        public object Value { get; }
        public Type Type { get; }
        public TypedArgument(string key, object value, Type type)
        {
            this.Key = key;
            this.Value = value;
            this.Type = type;
        }
    }
}
