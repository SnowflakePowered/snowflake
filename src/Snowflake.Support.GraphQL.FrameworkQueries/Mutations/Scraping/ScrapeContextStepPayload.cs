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
    internal sealed class ScrapeContextStepPayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public IEnumerable<ISeed> Current { get; set; }
        public Task<IGame> Game { get; set; }
        public Guid JobID { get; set; }
    }

    internal sealed class ScrapeContextStepPayloadType
        : ObjectType<ScrapeContextStepPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<ScrapeContextStepPayload> descriptor)
        {
            descriptor.Name(nameof(ScrapeContextStepPayload))
                .Description("Describes one step of the payload job.")
                .WithClientMutationId()
                .Interface<ScrapeContextPayloadInterface>();

            descriptor.Field(s => s.ScrapeContext)
                .Name("context")
                .Type<ScrapeContextType>();
            descriptor.Field(s => s.JobID)
                .Name("jobId")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(s => s.Current)
                .Type<ListType<SeedType>>();
            descriptor.Field(c => c.Game)
               .Description("The game this scrape context was created for.")
               .Type<NonNullType<GameType>>();
        }
    }
}
