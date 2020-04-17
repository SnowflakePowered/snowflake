using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Configuration
{
    public sealed class ConfigurationOptionTypeEnum
        : EnumType<ConfigurationOptionType>
    {
        protected override void Configure(IEnumTypeDescriptor<ConfigurationOptionType> descriptor)
        {
            descriptor.Name("ConfigurationOptionType")
                .Description("The primitive type of the value a configuration option accepts.")
                .BindValues(BindingBehavior.Implicit);
        }
    }
}
