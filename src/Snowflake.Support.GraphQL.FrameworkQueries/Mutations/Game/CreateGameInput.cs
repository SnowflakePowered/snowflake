using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game
{
    internal sealed class CreateGameInput
        : RelayMutationBase
    {
        public PlatformId PlatformID { get; set; }
    }

    internal sealed class CreateGameInputType
        : InputObjectType<CreateGameInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CreateGameInput> descriptor)
        {
            descriptor.Name(nameof(CreateGameInput))
                .WithClientMutationId();

            descriptor.Field(i => i.PlatformID)
                .Name("platformId")
                .Description("The Stone platform ID of the platform of this game.")
                .Type<NonNullType<PlatformIdType>>();
        }
    }
}
