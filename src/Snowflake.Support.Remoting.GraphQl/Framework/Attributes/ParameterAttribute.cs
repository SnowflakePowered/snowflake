using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ParameterAttribute : Attribute
    {
        public Type ParameterType { get; }
        public Type GraphQlType { get; }
        public string Key { get; }
        public string Description { get; }
        public bool Nullable { get; }
        public Type ConvertableType { get; }
        public ParameterAttribute(Type parameterType, Type graphQlType, string parameterKey, string description, bool nullable = false, Type convertableType = null)
        {
            this.ParameterType = parameterType;
            this.Key = parameterKey;
            this.Description = description;
            this.Nullable = nullable;
            this.GraphQlType = graphQlType;
        }
    }
}
