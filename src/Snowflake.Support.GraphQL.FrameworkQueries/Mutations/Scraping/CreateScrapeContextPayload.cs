﻿using HotChocolate.Types;
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
    internal sealed class CreateScrapeContextPayload
        : RelayMutationBase
    {
        public IScrapeContext ScrapeContext { get; set; }
        public Guid JobID { get; set; }
        public IGame Game { get; set; }
    }

    internal sealed class CreateScrapeContextPayloadType
        : ObjectType<CreateScrapeContextPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<CreateScrapeContextPayload> descriptor)
        {
            descriptor.Name(nameof(CreateScrapeContextPayload))
                .WithClientMutationId();
            descriptor.Field("jobContextField")
                .Description("The subfield of the `job` Query that this job can be accessed from.")
                .Resolve("scraping");

            descriptor.Field(c => c.JobID)
                .Name("jobId")
                .Description("The `jobId` of the scrape context that can be used to query or update the scrape context.")
                .Type<NonNullType<UuidType>>();

            descriptor.Field(c => c.ScrapeContext)
                .Name("context")
                .Description("The scrape context that was created.")
                .Type<NonNullType<ScrapeContextType>>();

            descriptor.Field(c => c.Game)
               .Description("The game this scrape context was created for.")
               .Type<NonNullType<GameType>>();
        }
    }
}
