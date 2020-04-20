using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class ScrapeContextStepPayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public IEnumerable<ISeed> Current { get; set; }
        public Guid JobID { get; set; }
        public bool HasNext { get; set; }
    }

    internal sealed class ScrapeContextStepPayloadType
        : ObjectType<ScrapeContextStepPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<ScrapeContextStepPayload> descriptor)
        {
            descriptor.Name(nameof(ScrapeContextStepPayload))
                .Description("Describes one step of the payload job.");

            descriptor.Field(s => s.ScrapeContext)
                .Type<ScrapeContextType>();
            descriptor.Field(s => s.JobID)
                .Name("jobId")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(s => s.Current)
                .Type<ListType<SeedType>>();
            descriptor.Field(s => s.HasNext)
                .Type<NonNullType<BooleanType>>();
        }
    }
}
