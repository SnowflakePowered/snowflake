using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation
{
    internal sealed class CreateInstallationPayload
        : RelayMutationBase
    {
        public Guid JobID { get; set; }
        public IGame Game { get; set; }
    }

    internal sealed class CreateInstallationPayloadType
        : ObjectType<CreateInstallationPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<CreateInstallationPayload> descriptor)
        {
            descriptor.Name(nameof(CreateInstallationPayload))
                .WithClientMutationId();
            descriptor.Field("jobContextField")
                .Description("The subfield of the `job` Query that this job can be accessed from.")
                .Resolver("installation");

            descriptor.Field(c => c.JobID)
                .Name("jobId")
                .Description("The job GUID.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(c => c.Game)
                .Description("The game that this job will install to.")
                .Type<NonNullType<GameType>>();
        }
    }
}
