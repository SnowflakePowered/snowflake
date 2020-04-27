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

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class CancelScrapeContextPayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public Guid JobID { get; set; }
        public Task<IGame> Game { get; set; }
    }

    internal sealed class CancelScrapeContextPayloadType
        : ObjectType<CancelScrapeContextPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<CancelScrapeContextPayload> descriptor)
        {
            descriptor.Name(nameof(CancelScrapeContextPayload))
                .WithClientMutationId();
         
            descriptor.Field(c => c.JobID)
                .Name("jobId")
                .Description("The `jobId` of the scrape context that can be used to query or update the scrape context.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(c => c.ScrapeContext)
                .Name("context")
                .Description("The scrape context that cancellation was requested for.")
                .Type<ScrapeContextType>();

            descriptor.Field(c => c.Game)
               .Description("The game this scrape context was created for.")
               .Type<NonNullType<GameType>>();
        }
    }
}
