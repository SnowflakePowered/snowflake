using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Extensibility;

namespace Snowflake.Support.Remoting.GraphQL.Types.Plugin
{
    public class PluginGraphType : ObjectGraphType<IPlugin>
    {
        public PluginGraphType()
        {
            Name = "Plugin";
            Description = "A loadable Snowflake plugin managed by the plugin manager.";
            Field(p => p.Author).Description("The author of the plugin.");
            Field(p => p.Description).Description("The description of the plugin.");
            Field(p => p.Name).Description("The plugin name.");
            Field<NonNullGraphType<StringGraphType>>("version",
                resolve: context => context.Source.Version.ToString(),
                description: "The version of the plugin.");
            Interface<PluginInterfaceType>();
        }
    }
}
