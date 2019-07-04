using System;
using System.Collections.Generic;
using System.Text;
using EnumsNET;
using EnumsNET.NonGeneric;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration.Serialization
{
    internal sealed class EnumConfigurationNode
        : AbstractConfigurationNode<Enum>, IEnumConfigurationNode
    {
        internal EnumConfigurationNode(string key, Enum value, Type enumType) : base(key, value)
        {
            this.EnumType = enumType;
            this.Value = NonGenericEnums.GetMember(enumType, value)
                .Attributes.Get<SelectionOptionAttribute>().SerializeAs;
        }
        public new string Value { get; }
        public Type EnumType { get; }
    }
}
