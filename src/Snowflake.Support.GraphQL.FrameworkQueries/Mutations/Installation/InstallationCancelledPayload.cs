using HotChocolate.Types;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual;
using Snowflake.Remoting.GraphQL.Model.Installation;
using Snowflake.Remoting.GraphQL.Model.Installation.Tasks;
using Snowflake.Remoting.GraphQL.Model.Records;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation
{
    internal sealed class InstallationCancelledPayload
        : RelayMutationBase
    {
        public Guid JobID { get; set; }
        public Task<IGame> Game { get; set; }
    }

    internal sealed class InstallationCancelledPayloadType
        : ObjectType<InstallationCancelledPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<InstallationCancelledPayload> descriptor)
        {
            descriptor.Name(nameof(InstallationCancelledPayload))
                .WithClientMutationId()
                .Interface<InstallationPayloadInterface>();

            descriptor.Field(i => i.JobID)
                .Name("jobId")
                .Description("The `jobId` of the installation or validation that was cancelled.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Game)
                .Description("The game that is the target of the installation or validation.")
                .Type<GameType>();
        }
    }

}
