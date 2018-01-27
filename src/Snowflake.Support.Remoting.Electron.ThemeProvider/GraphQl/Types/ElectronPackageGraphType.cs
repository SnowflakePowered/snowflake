using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Framework.Remoting.Electron;

namespace Snowflake.Support.Remoting.Electron.ThemeProvider.GraphQl.Types
{
    public class ElectronPackageGraphType : ObjectGraphType<IElectronPackage>
    {
        public ElectronPackageGraphType()
        {
            Name = "ElectronPackage";
            Description = "Represents an Electron ASAR Theme Package";
            Field(e => e.PackagePath)
                .Description("The path of the package on disk.");
            Field(e => e.Author)
                .Description("The author of the theme.");
            Field(e => e.Entry)
                .Description("The entry file to load first when loading this theme.");
            Field(e => e.Icon)
                .Description("The icon of this theme.");
            Field(e => e.Description)
                .Description("The description of the theme.");
            Field(e => e.Name)
                .Description("The name of the theme.");
        }
    }
}
