using HotChocolate.Types;
using Snowflake.Configuration;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration
{
    internal sealed class UpdateGameConfigurationValuePayload
        : RelayMutationBase
    {
        public List<IConfigurationValue> Values { get; set; }
        public IEnumerable<IGrouping<Guid, Guid>> Collections { get; set; }
    }

    internal sealed class UpdateGameConfigurationValuePayloadType
        : ObjectType<UpdateGameConfigurationValuePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdateGameConfigurationValuePayload> descriptor)
        {
            descriptor.Name(nameof(UpdateGameConfigurationValuePayload))
                .WithClientMutationId();
            descriptor.Field(g => g.Values)
                .Description("The modified configuration values.")
                .Type<NonNullType<ListType<NonNullType<ConfigurationValueType>>>>();
            descriptor.Field( g=> g.Collections)
                .Description("The modified configuration value GUIDs grouped by the value collection GUID.")
                .Type<NonNullType<UpdatedValueConfigurationMappingPayloadType>>();
        }
    }

    internal sealed class UpdatedValueConfigurationMappingPayloadType
        : ObjectType<IGrouping<Guid, Guid>>
    {
        protected override void Configure(IObjectTypeDescriptor<IGrouping<Guid, Guid>> descriptor)
        {
            descriptor.Name("ValueIdCollectionIdMappingPayload")
                .Description("Describes the modified configuration value GUID grouped by their value collection GUID");
            descriptor.Field(g => g.Key)
                .Description("The GUID of the value collection the grouped valueIds belong to.")
                .Name("collectionId")
                .Type<NonNullType<UuidType>>();
            descriptor.Field("valueIds")
                .Description("The GUIDs of the configuration values that were modified.")
                .Resolver(ctx => ctx.Parent<IGrouping<Guid, Guid>>().AsEnumerable())
                .Type<NonNullType<ListType<NonNullType<UuidType>>>>();
        }
    }
}
