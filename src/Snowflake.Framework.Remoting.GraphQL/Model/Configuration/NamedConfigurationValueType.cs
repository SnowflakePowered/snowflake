using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Configuration
{
    public sealed class NamedConfigurationValueType
        : ObjectType<(string section, string option, IConfigurationValue value)>
    {
        protected override void Configure(IObjectTypeDescriptor<(string section, string option, IConfigurationValue value)> descriptor)
        {
            descriptor.Name("NamedConfigurationValue")
                .Description("Represents a single unit of configuration with a named option key.");
            descriptor.Field(v => v.value)
                .Name("value")
                .Description("The value set for the configuration option.")
                .Type<ConfigurationValueType>();
            descriptor.Field(v => v.option)
                .Name("optionKey")
                .Description("The string key of the configuration option this value is set for.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(v => v.section)
                .Name("sectionKey")
                .Description("The string key of the configuration section that contains the option this value is set for.")
                .Type<StringType>();
        }
    }
}
