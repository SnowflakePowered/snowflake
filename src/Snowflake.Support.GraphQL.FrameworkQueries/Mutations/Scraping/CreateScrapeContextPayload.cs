using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class CreateScrapeContextPayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public Guid JobID { get; set; }
        public IGame Game { get; set; }
    }

    internal sealed class CreateScrapeContextPayloadType
        : ObjectType<CreateScrapeContextPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<CreateScrapeContextPayload> descriptor)
        {
            descriptor.Name(nameof(CreateScrapeContextPayload))
                .WithClientMutationId();
            descriptor.Field("jobContextField")
                .Description("The subfield of the `job` Query that this job can be accessed from.")
                .Resolver("scraping");

            descriptor.Field(c => c.JobID)
                .Name("jobId")
                .Description("The job GUID.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(c => c.ScrapeContext)
                .Name("context")
                .Type<NonNullType<ScrapeContextType>>();

            descriptor.Field(c => c.Game)
               .Description("The game this scrape context was created for.")
               .Type<NonNullType<GameType>>();
        }
    }
}
