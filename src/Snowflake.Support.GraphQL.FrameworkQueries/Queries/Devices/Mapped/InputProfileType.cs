using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices.Mapped
{
    public sealed class InputProfileType
        : ObjectType<(string profileName, Guid profileGuid)>
    {
        protected override void Configure(IObjectTypeDescriptor<(string profileName, Guid profileGuid)> descriptor)
        {
            descriptor.Name("InputProfileType")
                .Description("Describes a input profile name by its association with a specific profile GUID");
            descriptor.Field("profileName")
                .Description("The name of the input profile")
                .Type<NonNullType<StringType>>()
                .Resolve(ctx => ctx.Parent<(string profileName, Guid _)>().profileName);
            descriptor.Field("profileId")
                .Description("The GUID of the input profile")
                .Type<NonNullType<UuidType>>()
                .Resolve(ctx => ctx.Parent<(string _, Guid profileGuid)>().profileGuid);
        }
    }
}
