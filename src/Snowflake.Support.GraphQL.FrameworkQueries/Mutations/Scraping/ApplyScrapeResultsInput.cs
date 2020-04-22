using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class ApplyScrapeResultsInput
        : RelayMutationBase
    {
        public Guid GameID { get; set; }
        public Guid JobID { get; set; }
        public List<string> FileTraversers { get; set; }
        public List<string> MetadataTraversers { get; set; }

    }

    internal sealed class ApplyScrapeResultsInputType
        : InputObjectType<ApplyScrapeResultsInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ApplyScrapeResultsInput> descriptor)
        {
            descriptor.Name(nameof(ApplyScrapeResultsInput))
                .WithClientMutationId();

            descriptor.Field(g => g.GameID)
                .Name("gameId")
                .Description("The GUID of the game to apply the results to." +
                " If not specified, applies the results to the game this scrape context was originally created for.")
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
