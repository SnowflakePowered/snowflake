using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Remoting.GraphQL.Model.Queueing;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Queueing
{
    internal sealed class ScrapingJobType
        : ObjectType<(IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>, Guid)>
    {
        protected override void Configure(IObjectTypeDescriptor<(IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>, Guid)> descriptor)
        {
            descriptor.Name("ScrapingJob")
                .Description("Describes a single scraping job.")
                .Interface<QueuableJobInterface>();
            descriptor.Field("jobId")
              .Type<NonNullType<UuidType>>()
              .Resolver(ctx =>
              {
                  var (_, token) = ctx.Parent<(IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>, Guid)>();
                  return token;
              });
            descriptor.Field("current")
                .Type<ListType<SeedType>>()
                .Resolver(ctx =>
                {
                    var (queue, token) = ctx.Parent<(IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>, Guid)>();
                    return queue.GetCurrent(token);
                });
            descriptor.Field("context")
                .Type<ScrapeContextType>()
                .Resolver(ctx =>
                {
                    var (queue, token) = ctx.Parent<(IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>, Guid)>();
                    return queue.GetSource(token);
                });
        }
    }
}
