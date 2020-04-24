using HotChocolate.Types;
using Snowflake.Configuration;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration
{
    internal sealed class DeleteGameConfigurationPayload
        : RelayMutationBase
    {
        public Guid CollectionID { get; set; }
        public IConfigurationCollection Configuration { get; set; }
    }

    internal sealed class DeleteGameConfigurationPayloadType
        : ObjectType<DeleteGameConfigurationPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<DeleteGameConfigurationPayload> descriptor)
        {
            descriptor.Name(nameof(DeleteGameConfigurationPayload))
                .WithClientMutationId();

            descriptor.Field(p => p.CollectionID)
                .Name("collectionId")
                .Description("The collectionId of the configuration collection that was deleted.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(p => p.Configuration)
                .Description("If in the deletion query, the `retrieval` field was specified, contains the configuration that was deleted.")
                .Type<ConfigurationCollectionType>();
        }
    }
}
