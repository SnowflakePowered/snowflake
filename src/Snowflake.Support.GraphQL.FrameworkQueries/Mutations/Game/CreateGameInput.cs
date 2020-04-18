using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Framework.Remoting.GraphQL.RelayMutations;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

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
                .Type<NonNullType<PlatformIdType>>();
        }
    }
}
