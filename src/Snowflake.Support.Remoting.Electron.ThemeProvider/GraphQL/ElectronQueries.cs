﻿using HotChocolate.Types;
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
            descriptor.ExtendQuery();
            descriptor.Field("electronPackages")
                .Description("All loaded Electron ASAR theme packages.")
                .Resolve(ctx => ctx.SnowflakeService<IElectronPackageProvider>().Interfaces)
                .Type<NonNullType<ListType<NonNullType<ElectronPackageType>>>>();
        }
    }
}
