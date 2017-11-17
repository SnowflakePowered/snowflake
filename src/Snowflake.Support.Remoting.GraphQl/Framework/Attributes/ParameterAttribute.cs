using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ParameterAttribute : Attribute
    {
        public Type Type { get; }
        public string Key { get; }
        public string Description { get; }
        public bool Nullable { get; }
        public ParameterAttribute(Type parameterType, string parameterKey, string description, bool nullable = false)
        {
            this.Type = parameterType;
            this.Key = parameterKey;
        }
    }
}
