using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Remoting.GraphQL.Model.Queueing;
using Snowflake.Remoting.GraphQL.Model.Records;
using Snowflake.Remoting.GraphQL.Model.Scraping;
using Snowflake.Scraping;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations;
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
                .Implements<QueuableJobInterface>();
            descriptor.Field("jobId")
              .Type<NonNullType<UuidType>>()
              .Resolve(ctx =>
              {
                  var (_, token) = ctx.Parent<(IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>, Guid)>();
                  return token;
              });
            descriptor.Field("current")
                .Type<ListType<SeedType>>()
                .Resolve(ctx =>
                {
                    var (queue, token) = ctx.Parent<(IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>, Guid)>();
                    return queue.GetCurrent(token);
                });
            descriptor.Field("context")
                .Type<ScrapeContextType>()
                .Resolve(ctx =>
                {
                    var (queue, token) = ctx.Parent<(IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>, Guid)>();
                    return queue.GetSource(token);
                });
            descriptor.Field("game")
                .Description("The game associated with this job, if any.")
                .Type<GameType>()
                .Resolve(ctx =>
                {
                    var (_, token) = ctx.Parent<(IAsyncJobQueue<IScrapeContext, IEnumerable<ISeed>>, Guid)>();
                    
                    return ctx.GetAssignedGame(token);
                });
        }
    }
}
