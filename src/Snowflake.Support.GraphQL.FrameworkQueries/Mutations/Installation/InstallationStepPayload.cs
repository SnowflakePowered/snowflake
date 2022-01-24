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
    internal sealed class InstallationStepPayload
        : RelayMutationBase
    {
        public Guid JobID { get; set; }
        public TaskResult<IFile> Current { get; set; }
        public Task<IGame> Game { get; set; }
    }

    internal sealed class InstallationStepPayloadType
        : ObjectType<InstallationStepPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<InstallationStepPayload> descriptor)
        {
            descriptor.Name(nameof(InstallationStepPayload))
                .WithClientMutationId()
                .Implements<InstallationPayloadInterface>();

            descriptor.Field(i => i.JobID)
                .Name("jobId")
                .Description("The `jobId` of the installation or verification job that was updated.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Current)
                .Name("current")
                .Description("The current file that was yielded by the installation or verification job.")
                .Type<TaskResultType<IFile, ContextualFileInfoType>>();
            descriptor.Field(i => i.Game)
                .Description("The game that is the target of the installation or validation.")
                .Type<GameType>();
        }
    }
}
