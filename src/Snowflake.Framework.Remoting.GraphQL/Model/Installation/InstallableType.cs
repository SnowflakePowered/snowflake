using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Installation.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Installation
{
    public sealed class InstallableType
        : ObjectType<IInstallable>
    {
        protected override void Configure(IObjectTypeDescriptor<IInstallable> descriptor)
        {
            descriptor.Name("Installable")
                .Description("Describes a unit of installation for a system.");
            descriptor.Field(i => i.Source)
                .Description("The installer plugin that will be used to install this unit.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(i => i.DisplayName)
                .Description("The human readable display name.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(i => i.Artifacts)
                .Description("The list of files and folders to be passed to the source installer to be used as artifacts during installation.")
                .Type<NonNullType<ListType<NonNullType<OSTaggedFileSystemPathType>>>>();
        }
    }
}
