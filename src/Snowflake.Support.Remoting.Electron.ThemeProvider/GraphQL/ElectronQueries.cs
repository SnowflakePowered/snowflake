using HotChocolate.Types;
using Snowflake.Remoting.Electron;
using Snowflake.Remoting.GraphQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Electron.ThemeProvider.GraphQL
{
    public sealed class ElectronQueries
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Query");
            descriptor.Field("electronPackages")
                .Description("All loaded Electron ASAR theme packages.")
                .Resolver(ctx => ctx.SnowflakeService<IElectronPackageProvider>().Interfaces)
                .Type<NonNullType<ListType<NonNullType<ElectronPackageType>>>>();
        }
    }
}
