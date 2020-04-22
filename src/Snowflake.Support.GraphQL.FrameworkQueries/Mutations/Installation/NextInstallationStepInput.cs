using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation
{
    internal sealed class NextInstallationStepInput
        : RelayMutationBase
    {
        public Guid JobID { get; set; }
    }

    internal sealed class NextInstallationStepInputType
        : InputObjectType<NextInstallationStepInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<NextInstallationStepInput> descriptor)
        {
            descriptor.Name(nameof(NextInstallationStepInput))
                .WithClientMutationId();

            descriptor.Field(i => i.JobID)
                .Name("jobId")
                .Type<NonNullType<UuidType>>();
        }
    }

}
