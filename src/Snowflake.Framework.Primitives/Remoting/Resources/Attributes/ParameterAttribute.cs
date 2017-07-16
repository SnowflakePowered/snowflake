using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class ParameterAttribute : Attribute
    {
        public Type Type { get; }
        public string Key { get; }
        public ParameterAttribute(Type parameterType, string parameterKey)
        {
            this.Type = parameterType;
            this.Key = parameterKey;
        }
    }
}
