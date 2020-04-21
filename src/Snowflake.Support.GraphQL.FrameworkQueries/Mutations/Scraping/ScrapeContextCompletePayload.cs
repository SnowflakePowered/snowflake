using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class ScrapeContextCompletePayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public Guid JobID { get; set; }
    }

    internal sealed class ScrapeContextCompletePayloadType
        : ObjectType<ScrapeContextCompletePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<ScrapeContextCompletePayload> descriptor)
        {
            descriptor.Name(nameof(ScrapeContextCompletePayload))
                .Description("Describes the final step of the scrape context.")
                .Interface<ScrapeContextPayloadInterface>();
           
            descriptor.Field(s => s.ScrapeContext)
                .Name("context")
                .Type<ScrapeContextType>();
            descriptor.Field(s => s.JobID)
                .Name("jobId")
                .Type<NonNullType<UuidType>>();
        }
    }
}
