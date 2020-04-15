using HotChocolate.Types;
using Snowflake.Extensibility;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Extensibility
{
    public sealed class ModuleType
        : ObjectType<IModule>
    {
        protected override void Configure(IObjectTypeDescriptor<IModule> descriptor)
        {
            descriptor.Name("Module")
                .Description("Describes a loaded module.");
            descriptor.Field(p => p.Author).Description("The author of the module.");
            descriptor.Field(p => p.Name).Description("The module name.");
            descriptor
                .Field("version")
                .Description("The version of the module.")
                .Resolver(ctx => ctx.Parent<IModule>().Version.ToString());
            descriptor.Field(p => p.DisplayName).Description("The friendly display name of the module.");
            descriptor.Field(p => p.Loader).Description("The loader that is responsible for loading this module.");
            descriptor.Field(p => p.ContentsDirectory).Description("The directory that contains the modules contents. " +
                "Use the filesystem root query to explore this path.")
                .Type<OSDirectoryPathType>();
            descriptor.Field(p => p.ModuleDirectory).Description("The directory that contains the modules contents folder. " +
                "Use the filesystem root query to explore this path.")
                .Type<OSDirectoryPathType>();
            descriptor.Field(p => p.Entry).Description("Gets the entry point of this module that is passed to the loader.");
        }
    }
}
