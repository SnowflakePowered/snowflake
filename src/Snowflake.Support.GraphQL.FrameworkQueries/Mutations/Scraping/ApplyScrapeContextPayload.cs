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
    internal sealed class ApplyScrapeContextPayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public IGame Game { get; set; }
    }

    internal sealed class ApplyScrapeContextPayloadType
        : ObjectType<ApplyScrapeContextPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<ApplyScrapeContextPayload> descriptor)
        {
            descriptor.Name(nameof(ApplyScrapeContextPayload))
                .WithClientMutationId();

            descriptor.Field(c => c.Game)
               .Description("The game this scrape context was applied to.")
               .Type<NonNullType<GameType>>();

            descriptor.Field(c => c.ScrapeContext)
                .Name("context")
                .Description("The scrape context that was used.")
                .Type<NonNullType<ScrapeContextType>>();
        }
    }
}
