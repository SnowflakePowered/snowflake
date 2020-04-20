using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class ApplyScrapeResultsInput
    {
        public Guid GameID { get; set; }
        public Guid JobID { get; set; }
        public List<string> FileTraversers { get; }
        public List<string> MetadataTraversers { get; }

    }

    internal sealed class ApplyScrapeResultsInputType
        : InputObjectType<ApplyScrapeResultsInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ApplyScrapeResultsInput> descriptor)
        {
            descriptor.Name(nameof(ApplyScrapeResultsInput));
            descriptor.Field(g => g.GameID)
                .Name("gameId")
                .Description("The GUID of the game to apply the results to." +
                " If not specified, tries to apply the results to the GUID of the seed `scrapecontext_record`.")
                .Type<UuidType>();
            descriptor.Field(g => g.JobID)
                .Name("jobId")
                .Description("The Job GUID of the scrape context")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.FileTraversers)
               .Type<NonNullType<ListType<NonNullType<StringType>>>>();
            descriptor.Field(i => i.MetadataTraversers)
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();
        }
    }
}
