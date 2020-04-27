using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.Model.Records;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class NextScrapeContextStepInput
    {
        public Guid JobID { get; set; }
        public IList<SeedTreeGraftInput> Seeds { get; set; }
    }

    internal sealed class NextScrapeContextStepInputType
        : InputObjectType<NextScrapeContextStepInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<NextScrapeContextStepInput> descriptor)
        {
            descriptor.Name(nameof(NextScrapeContextStepInput));
            descriptor.Field(i => i.JobID)
                .Name("jobId")
                .Description("The `jobId` of the scrape context that can be used to query or update the scrape context.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Seeds)
                .Name("seeds")
                .Description("User-provided seeds to add to the scrape context.")
                .Type<ListType<NonNullType<SeedTreeGraftInputType>>>();
        }
    }
}
