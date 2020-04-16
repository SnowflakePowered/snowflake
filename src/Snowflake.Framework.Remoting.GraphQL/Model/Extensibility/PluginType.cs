using HotChocolate.Types;
using Snowflake.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Extensibility
{
    public sealed class PluginType
        : ObjectType<IPlugin>
    {
        protected override void Configure(IObjectTypeDescriptor<IPlugin> descriptor)
        {
            descriptor.Name("Plugin")
                .Description("Describes a loaded plugin.");
            descriptor.Field(p => p.Author).Description("The author of the plugin.");
            descriptor.Field(p => p.Description).Description("The description of the plugin.");
            descriptor.Field(p => p.Name).Description("The plugin name.");
            descriptor.Field("version")
                      .Description("The version of the module.")
                      .Resolver(ctx => ctx.Parent<IPlugin>().Version.ToString());
            descriptor.Field("interfaces")
                .Description("The plugin interfaces this plugin implements.")
                .Resolver(c => c.Parent<IPlugin>().GetType().GetInterfaces().Select(t => t.FullName));
        }
    }
}
