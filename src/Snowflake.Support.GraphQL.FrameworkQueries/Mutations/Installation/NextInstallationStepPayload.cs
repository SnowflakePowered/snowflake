using HotChocolate.Types;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Remoting.GraphQL.Model.Filesystem.Contextual;
using Snowflake.Remoting.GraphQL.Model.Installation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation
{
    internal sealed class NextInstallationStepPayload
        : RelayMutationBase
    {
        public Guid JobID { get; set; }
        public TaskResult<IFile> Current { get; set; }
    }

    internal sealed class NextInstallationStepPayloadType
        : ObjectType<NextInstallationStepPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<NextInstallationStepPayload> descriptor)
        {
            descriptor.Name(nameof(NextInstallationStepPayload))
                .WithClientMutationId();

            descriptor.Field(i => i.JobID)
                .Name("jobId")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Current)
                .Name("current")
                .Type<TaskResultType<IFile, ContextualFileInfoType>>();
        }
    }

}
