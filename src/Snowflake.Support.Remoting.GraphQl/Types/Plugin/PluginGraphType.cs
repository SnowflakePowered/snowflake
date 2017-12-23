using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Extensibility;

namespace Snowflake.Support.Remoting.GraphQl.Types.Plugin
{
    public class PluginGraphType : ObjectGraphType<IPlugin>
    {
        public PluginGraphType()
        {
            Field(p => p.Author).Description("The author of the plugin.");
            Field(p => p.Description).Description("The description of the plugin.");
            Field(p => p.Name).Description("The plugin name.");
            Field<NonNullGraphType<StringGraphType>>("version",
                resolve: context => context.Source.Version.ToString(),
                description: "The version of the plugin.");
        }
    }
}
