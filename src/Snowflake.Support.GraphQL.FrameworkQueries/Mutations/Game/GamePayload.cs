using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Records;
using Snowflake.Framework.Remoting.GraphQL.RelayMutations;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Game
{
    internal sealed class GamePayload
        : RelayMutationBase
    {
        public IGame Game { get; set; }
    }

    internal sealed class GamePayloadType 
        : ObjectType<GamePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<GamePayload> descriptor)
        {
            descriptor.Name("GamePayload")
                .WithClientMutationId();

            descriptor.Field(p => p.Game)
                .Type<GameType>();
        }
    }
}
