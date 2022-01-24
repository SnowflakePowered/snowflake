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
    internal sealed class InstallationCompletePayload
        : RelayMutationBase
    {
        public Guid JobID { get; set; }
        public Task<IGame> Game { get; set; }
    }

    internal sealed class InstallationCompletePayloadType
        : ObjectType<InstallationCompletePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<InstallationCompletePayload> descriptor)
        {
            descriptor.Name(nameof(InstallationCompletePayload))
                .WithClientMutationId()
                .Implements<InstallationPayloadInterface>();

            descriptor.Field(i => i.JobID)
                .Name("jobId")
                .Description("The `jobId` of the installation or validation that was completed.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Game)
                .Description("The game the installation or validation was for.")
                .Type<GameType>();
        }
    }
}
