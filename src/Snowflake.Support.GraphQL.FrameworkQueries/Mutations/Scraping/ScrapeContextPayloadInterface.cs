using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    public sealed class ScrapeContextPayloadInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("ScrapeContextPayload")
                .WithClientMutationId();

            descriptor.Field("context")
                .Description("The scrape context that was updated.")
                .Type<ScrapeContextType>();
            descriptor.Field("jobId")
                .Description("The `jobId` of the scrape context that can be used to query or update the scrape context.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field("game")
                .Description("The game the scrape context was created for.")
                .Type<NonNullType<GameType>>();
        }
    }
}
