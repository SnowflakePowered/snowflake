using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    internal sealed class ApplyScrapeResultsPayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public IGame Game { get; set; }
    }

    internal sealed class ApplyScrapeResultsPayloadType
        : ObjectType<ApplyScrapeResultsPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<ApplyScrapeResultsPayload> descriptor)
        {
            descriptor.Name(nameof(ApplyScrapeResultsPayload))
                .WithClientMutationId();

            descriptor.Field(c => c.Game)
               .Description("The game this scrape context was applied to.")
               .Type<NonNullType<GameType>>();

            descriptor.Field(c => c.ScrapeContext)
                .Name("context")
                .Type<NonNullType<ScrapeContextType>>();
        }
    }
}
