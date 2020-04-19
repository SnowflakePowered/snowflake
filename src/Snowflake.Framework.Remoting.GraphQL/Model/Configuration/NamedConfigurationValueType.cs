using HotChocolate.Types;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Configuration
{
    public sealed class NamedConfigurationValueType
        : ObjectType<(string section, string option, IConfigurationValue value)>
    {
        protected override void Configure(IObjectTypeDescriptor<(string section, string option, IConfigurationValue value)> descriptor)
        {
            descriptor.Name("NamedConfigurationValue")
                .Description("Represents a single unit of configuration with a named option key.");
            descriptor.Field("value")
                .Resolver(ctx => ctx.Parent<(string section, string option, IConfigurationValue value)>().value)
                .Description("The value set for the configuration option.")
                .Type<ConfigurationValueType>();
            descriptor.Field("optionKey")
                .Resolver(ctx => ctx.Parent<(string section, string option, IConfigurationValue value)>().option)
                .Description("The string key of the configuration option this value is set for.")
                .Type<NonNullType<StringType>>();
            descriptor.Field("sectionKey")
                .Resolver(ctx => ctx.Parent<(string section, string option, IConfigurationValue value)>().section)
                .Description("The string key of the configuration section that contains the option this value is set for.")
                .Type<StringType>();
        }
    }
}
