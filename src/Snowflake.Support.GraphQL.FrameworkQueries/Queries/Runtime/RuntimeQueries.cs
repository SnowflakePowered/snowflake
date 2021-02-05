using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Extensibility;
using Snowflake.Loader;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Snowflake.Remoting.GraphQL;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Runtime
{
    public sealed class RuntimeQueries
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("RuntimeQuery")
                .Description("Provides access to Snowflake runtime details");
            descriptor.Field("plugins")
                .Description("Currently loaded plugins.")
                .Resolve(ctx => ctx.SnowflakeService<IPluginManager>())
                .Type<ListType<NonNullType<PluginType>>>();
            descriptor.Field("modules")
                .Description("Currently enumerated modules. These may or may not have been loaded.")
                .Resolve(ctx => ctx.SnowflakeService<IModuleEnumerator>().Modules)
                .Type<NonNullType<ListType<NonNullType<ModuleType>>>>();
            descriptor.Field("os")
                .Description("Gets the operating system currently running.")
                .Resolve(ctx => RuntimeInformation.OSDescription)
                .Type<NonNullType<StringType>>();
        }
    }
}
