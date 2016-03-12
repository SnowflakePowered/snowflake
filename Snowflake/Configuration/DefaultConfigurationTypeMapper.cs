using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public class DefaultConfigurationTypeMapper : ConfigurationTypeMapper
    {
        public DefaultConfigurationTypeMapper(IBooleanMapping booleanMapping, string nullSerializer)
            : base(nullSerializer)
        {
            this.Add<string>(value => value);
            this.Add<bool>(booleanMapping.FromBool);
            this.Add<Enum>(this.EnumConverter);
        }

        private string EnumConverter(Enum enumValue)
        {
            Type enumType = enumValue.GetType();
            SelectionOptionAttribute selectionObject =
                enumType.GetField(Enum.GetName(enumType, enumValue))
                .GetCustomAttribute<SelectionOptionAttribute>();
            return selectionObject.SerializeAs;
        }
    }
}
