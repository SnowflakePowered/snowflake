﻿using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.Model.Queueing;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Queueing
{
    internal sealed class ScrapingJobQueueType
        : ObjectType<IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>>
    {
        protected override void Configure(IObjectTypeDescriptor<IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>> descriptor)
        {
            descriptor.Name("ScrapingJobQueue")
                .Description("Provides access to values in the scraping job queue")
                .UseJobQueue();

            descriptor.Field("job")
                .Argument("jobId", arg => arg.Type<NonNullType<UuidType>>()
                    .Description("The `jobId` of the scrape context that can be used to query the scrape context."))
                .Resolve(ctx =>
                {
                    var context = ctx.Parent<IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>>();
                    return (context, ctx.ArgumentValue<Guid>("jobId"));
                })
                .Type<ScrapingJobType>();
        }
    }
}
