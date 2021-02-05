using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Game.Orchestration
{
    public sealed class ConfigurationProfileType
        : ObjectType<(string profileName, Guid collectionGuid)>
    {
        protected override void Configure(IObjectTypeDescriptor<(string profileName, Guid collectionGuid)> descriptor)
        {
            descriptor.Name("ConfigurationProfileType")
                .Description("Describes a configuration profile name by its association with a specific collection GUID");
            descriptor.Field("profileName")
                .Description("The name of the configuration profile")
                .Type<NonNullType<StringType>>()
                .Resolve(ctx => ctx.Parent<(string profileName, Guid _)>().profileName);
            descriptor.Field("collectionId")
                .Description("The GUID of the configuration profile collection")
                .Type<NonNullType<UuidType>>()
                .Resolve(ctx => ctx.Parent<(string _, Guid collectionGuid)>().collectionGuid);
        }
    }
}
