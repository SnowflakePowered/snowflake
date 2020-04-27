using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Saving;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Saving
{
    internal sealed class CreateSaveProfilePayload
        : RelayMutationBase
    {
        public IGame Game { get; set; }
        public ISaveProfile SaveProfile { get; set; }
    }

    internal sealed class CreateSaveProfilePayloadType
        : ObjectType<CreateSaveProfilePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<CreateSaveProfilePayload> descriptor)
        {
            descriptor.Name(nameof(CreateSaveProfilePayload))
                .WithClientMutationId();

            descriptor.Field(g => g.Game)
                .Description("The game that the new save profile was created for.")
                .Type<NonNullType<GameType>>();

            descriptor.Field(g => g.SaveProfile)
                .Description("The newly created save profile.")
                .Type<NonNullType<SaveProfileType>>();
        }
    }
}
