using HotChocolate.Types;
using Snowflake.Remoting.Electron;
using Snowflake.Remoting.GraphQL.Model.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Support.Remoting.Electron.ThemeProvider.GraphQL
{
    public sealed class ElectronPackageType
        : ObjectType<IElectronPackage>
    {
        protected override void Configure(IObjectTypeDescriptor<IElectronPackage> descriptor)
        {
            descriptor.Name("ElectronPackage")
                .Description("Represents an Electron ASAR Theme Package");

            descriptor.Field("packagePath")
                .Description("The path of the package on disk.")
                .Resolver(ctx => new FileInfo(ctx.Parent<IElectronPackage>().PackagePath))
                .Type<NonNullType<OSFilePathType>>();
            descriptor.Field(e => e.Author)
                .Description("The author of the theme.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(e => e.Entry)
                .Description("The entry file to load first when loading this theme.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(e => e.Icon)
                .Description("The icon of this theme.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(e => e.Description)
                .Description("The description of the theme.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(e => e.Name)
                .Description("The name of the theme.")
                .Type<NonNullType<StringType>>();
        }
    }
}
