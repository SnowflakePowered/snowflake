using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Records;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation
{
    internal sealed class InstallationPayloadInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("InstallationPayload")
                .WithClientMutationId();

            descriptor.Field("jobId")
                .Type<NonNullType<UuidType>>();
            descriptor.Field("game")
               .Type<GameType>();
        }
    }
}
