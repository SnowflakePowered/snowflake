using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class CancelScrapeContextInput
        : RelayMutationBase
    {
        public Guid JobID { get; set; }
    }

    internal sealed class CancelScrapeContextInputType
        : InputObjectType<CancelScrapeContextInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<CancelScrapeContextInput> descriptor)
        {
            descriptor.Name(nameof(CancelScrapeContextInput))
                .WithClientMutationId();

            descriptor.Field(i => i.JobID)
                .Name("jobId")
                .Description("The `jobId` of the scrape context that can be used to query or update the scrape context.")
                .Type<NonNullType<UuidType>>();
        }
    }
}
