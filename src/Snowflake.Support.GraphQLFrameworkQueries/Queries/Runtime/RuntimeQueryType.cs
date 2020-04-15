using GraphQL.Types;
using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Extensibility;
using Snowflake.Loader;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Runtime
{
    public sealed class RuntimeQueryType
        : ObjectType<RuntimeQueryRoot>
    {
        protected override void Configure(IObjectTypeDescriptor<RuntimeQueryRoot> descriptor)
        {
            descriptor.Name("RuntimeQuery")
                .Description("Provides access to Snowflake runtime details");
            descriptor.Field("plugins")
                .Description("Currently loaded plugins.")
                .Resolver(ctx => ctx.Service<IPluginManager>())
                .Type<ListType<NonNullType<PluginType>>>();
            descriptor.Field("modules")
                .Description("Currently enumerated modules. These may or may not have been loaded.")
                .Resolver(ctx => ctx.Service<IModuleEnumerator>().Modules)
                .Type<ListType<NonNullType<ModuleType>>>();
        }
    }
}
