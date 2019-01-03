using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Execution.Extensibility;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Support.Remoting.GraphQL.Types.Plugin
{
    public class EmulatorGraphType : ObjectGraphType<IEmulator>
    {
        public EmulatorGraphType()
        {
            Name = "Emulator";
            Description = "An emulator plugin.";
            Field(p => p.Author).Description("The author of the plugin.");
            Field(p => p.Description).Description("The description of the plugin.");
            Field(p => p.Name).Description("The name of the emulator.");
            Field<NonNullGraphType<StringGraphType>>("version",
                resolve: context => context.Source.Version.ToString(),
                description: "The version of the plugin.");
            Field<ListGraphType<StringGraphType>>("mimetypes",
                resolve: context => context.Source.Properties.Mimetypes,
                description: "The mimetypes this emulator supports executing.");
            Field<ListGraphType<StringGraphType>>("requiredBios",
                resolve: context => context.Source.Properties.RequiredSystemFiles,
                description: "The BIOS system files this emulator requires.");
            Field<ListGraphType<StringGraphType>>("optionalBios",
                resolve: context => context.Source.Properties.OptionalSystemFiles,
                description: "The BIOS system files this emulator may use to unlock additional functionality.");
            Field<StringGraphType>("saveFormat",
                resolve: context => context.Source.Properties.SaveFormat,
                description: "The format this emulator saves game data as.");
            Field<ListGraphType<StringGraphType>>("specialCapabilities",
                resolve: context => context.Source.Properties.SpecialCapabilities,
                description: "The special capabilities this emulator has.");
            Interface<PluginInterfaceType>();
        }
    }
}
