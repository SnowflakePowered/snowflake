using System;
using System.Collections.Generic;
using System.Text;
using EnumsNET;
using EnumsNET.NonGeneric;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration.Serialization
{
    public sealed class EnumConfigurationNode
        : AbstractConfigurationNode<Enum>
    {
        internal EnumConfigurationNode(string key, Enum value, Type enumType) : base(key, value)
        {
            this.EnumType = enumType;
            this.Value = Enums.GetMember(enumType, value)?
                .Attributes?.Get<SelectionOptionAttribute>()?.SerializeAs ?? String.Empty;
        }
        public new string Value { get; }
        public Type EnumType { get; }
    }
}
