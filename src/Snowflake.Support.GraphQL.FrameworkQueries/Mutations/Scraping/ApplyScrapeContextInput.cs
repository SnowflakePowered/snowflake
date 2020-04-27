using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class ApplyScrapeContextInput
        : RelayMutationBase
    {
        public Guid GameID { get; set; }
        public Guid JobID { get; set; }
        public List<string> FileTraversers { get; set; }
        public List<string> MetadataTraversers { get; set; }

    }

    internal sealed class ApplyScrapeContextInputType
        : InputObjectType<ApplyScrapeContextInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ApplyScrapeContextInput> descriptor)
        {
            descriptor.Name(nameof(ApplyScrapeContextInput))
                .WithClientMutationId();

            descriptor.Field(g => g.GameID)
                .Name("gameId")
                .Description("The `gameId` GUID of the game to apply the results to." +
                " If not specified, applies the results to the game this scrape context was originally created for.")
                .Type<UuidType>();
            descriptor.Field(g => g.JobID)
                .Name("jobId")
                .Description("The `jobId` of the scrape context that can be used to query or update the scrape context.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(i => i.FileTraversers)
                .Description("The names of the file traversers used to retrieve file metadata when traversing the scrape context.")
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();
            descriptor.Field(i => i.MetadataTraversers)
                .Description("The names of the metadata traversers used to retrieve information/string based metadata when traversing the scrape context.")
                .Type<NonNullType<ListType<NonNullType<StringType>>>>();
        }
    }
}
