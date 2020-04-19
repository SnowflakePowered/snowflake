using HotChocolate.Types;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Remoting.GraphQL.Model.Extensibility;
using Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Runtime
{
    public sealed class PluginQueries
        : ObjectTypeExtension<IPlugin>
    {
        protected override void Configure(IObjectTypeDescriptor<IPlugin> descriptor)
        {
            descriptor.Name("Plugin");
            descriptor.Field("provision")
                .Description("Provides access to the plugin's provisioned resources if this plugin is provisioned.")
                .Resolver(ctx =>
                {
                    if (ctx.Parent<IPlugin>() is IProvisionedPlugin provisionedPlugin)
                    {
                        return provisionedPlugin;
                    }
                    return null;
                }).Type<ProvisionedPluginType>();
        }
    }
}
