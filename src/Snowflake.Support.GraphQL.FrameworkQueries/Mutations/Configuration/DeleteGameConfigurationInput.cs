using HotChocolate;
using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration
{
    internal sealed class DeleteGameConfigurationInput
        : RelayMutationBase
    {
        public Guid CollectionID { get; set; }
#nullable enable
        public RetrieveGameConfigurationInput? Retrieval { get; set; }
#nullable disable
    }

    internal sealed class DeleteGameConfigurationInputType
        : InputObjectType<DeleteGameConfigurationInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteGameConfigurationInput> descriptor)
        {
            descriptor.Name(nameof(DeleteGameConfigurationInput))
                .WithClientMutationId();
            descriptor.Field(g => g.CollectionID)
                .Name("collectionId")
                .Description("The collectionId of the configuration to delete.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(g => g.Retrieval)
                .Description("If this is set, the `configuration` key in the resulting payload will retrieve the specified configuration after " +
                "deletion. The retrieved configuration must match the configuration specified by `collectionId`, or deletion will fail. If this is " +
                "not set or is null, no configuration will be returned, but deletion will continue if `collectionId` exists in the configuration store.")
                .Type<RetrieveGameConfigurationInputType>();
        }
    }
}
