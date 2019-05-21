using System;
using System.Collections.Generic;
using System.Text;
using EnumsNET;
using EnumsNET.NonGeneric;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration.Serialization
{
    public abstract class EnumConfigurationNode
        : AbstractConfigurationNode<Enum>
    {
        private protected EnumConfigurationNode(string key, Enum value, Type enumType) : base(key, value)
        {
            this.EnumType = enumType;
            this.Value = NonGenericEnums.GetMember(enumType, value)
                .Attributes.Get<SelectionOptionAttribute>().SerializeAs;
        }
        public new string Value { get; }
        public Type EnumType { get; }
    }

    public sealed class EnumConfigurationNode<T>
       : EnumConfigurationNode where T: Enum
    {
        public EnumConfigurationNode(string key, T value) : base(key, value, typeof(T))
        {
            this.Value = value;
        }
        public new T Value { get; }
    }
}
