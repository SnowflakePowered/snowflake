using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EnumsNET.NonGeneric;
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
<<<<<<< HEAD
            return NonGenericEnums.GetMember(enumValue.GetType(), enumValue)
                .Attributes.Get<SelectionOptionAttribute>().SerializeAs;
=======
            return NonGenericEnums.GetAttributes(enumValue.GetType(), enumValue)
                .Get<SelectionOptionAttribute>().SerializeAs;
>>>>>>> cd39263... Modules: Robust loader and plugin system (#249)
        }
    }
}
