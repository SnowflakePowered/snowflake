using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Installation;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.PlatformInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Installables
{
    public sealed class InstallableQueries
        : InterfaceTypeExtension
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("OSDirectoryContents")
                .Field("installables")
                .Description("Fetches installables for this directory, searching all top level files and directories," +
                "not including the current directory itself.")
                .Argument("platformID", arg => arg.Type<NonNullType<PlatformIdType>>()
                    .Description("The platform to look up installables for."))
                .Type<NonNullType<ListType<NonNullType<InstallableType>>>>();
        }
    }
}
