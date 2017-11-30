using GraphQL.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.Configuration
{
    public class ConfigurationOptionTypeEnum : EnumerationGraphType<OptionType>
    {

        internal static OptionType GetType(IConfigurationOption o)
        {
            if (o.Type == typeof(int)) return OptionType.Integer;
            if (o.Type == typeof(bool)) return OptionType.Boolean;
            if (o.Type == typeof(double)) return OptionType.Decimal;
            if (o.Type.GetTypeInfo().IsEnum) return OptionType.Selection;
            if (o.IsPath) return OptionType.Path;
            return OptionType.String;
        }
    }

    public enum OptionType
    {
        Integer,
        Boolean,
        Decimal,
        Selection,
        Path,
        String
    }
}
