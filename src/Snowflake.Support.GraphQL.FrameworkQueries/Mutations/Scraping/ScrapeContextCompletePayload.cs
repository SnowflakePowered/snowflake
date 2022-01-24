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
    internal sealed class ScrapeContextCompletePayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public Guid JobID { get; set; }
        public Task<IGame> Game { get; set; }
    }

    internal sealed class ScrapeContextCompletePayloadType
        : ObjectType<ScrapeContextCompletePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<ScrapeContextCompletePayload> descriptor)
        {
            descriptor.Name(nameof(ScrapeContextCompletePayload))
                .Description("Describes the final step of the scrape context.")
                .WithClientMutationId()
                .Implements<ScrapeContextPayloadInterface>();
           
            descriptor.Field(s => s.ScrapeContext)
                .Name("context")
                .Description("The scrape context that was completed.")
                .Type<ScrapeContextType>();
            descriptor.Field(s => s.JobID)
                .Name("jobId")
                .Description("The `jobId` of the scrape context that can be used to query or update the scrape context.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(c => c.Game)
               .Description("The game this scrape context was created for.")
               .Type<NonNullType<GameType>>();
        }
    }
}
