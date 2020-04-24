using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using Snowflake.Support.GraphQL.FrameworkQueries.Queries.Game.Orchestration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation
{
    internal sealed class CreateValidationPayload
        : RelayMutationBase
    {
        public Guid JobID { get; set; }
        public IGame Game { get; set; }
        public IEmulatorOrchestrator Orchestrator { get; set; }
    }

    internal sealed class CreateValidationPayloadType
        : ObjectType<CreateValidationPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<CreateValidationPayload> descriptor)
        {
            descriptor.Name(nameof(CreateValidationPayload))
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

            descriptor.Field("orchestrator")
                .Description("The orchestrator that is verifying this game.")
                .Resolver(ctx =>
                {
                    var input = ctx.Parent<CreateValidationPayload>();
                    return (input.Game, input.Orchestrator);
                })
                .Type<NonNullType<GameOrchestratorType>>();
        }
    }

}
