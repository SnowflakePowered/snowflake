using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping
{
    public sealed class ScrapingMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            descriptor.Field("createScrapeContext")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<CreateScrapeContextInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<CreateScrapeContextInput>("input");
                    var plugins = ctx.SnowflakeService<IPluginManager>();
                    var cullers = plugins.GetCollection<ICuller>()
                        .Where(c => input.Cullers.Contains(c.Name, StringComparer.InvariantCultureIgnoreCase));
                    var scrapers = plugins.GetCollection<IScraper>()
                        .Where(c => input.Scrapers.Contains(c.Name, StringComparer.InvariantCultureIgnoreCase));
                    var game = await ctx.SnowflakeService<IGameLibrary>().GetGameAsync(input.GameID);
                    if (game == null) throw new ArgumentException("The specified game does not exist.");
                    var jobQueueFactory = ctx.SnowflakeService<IAsyncJobQueueFactory>();
                    var jobQueue = jobQueueFactory.GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);
                    var guid = await jobQueue.QueueJob(new GameScrapeContext(game, scrapers, cullers));
                    IScrapeContext context = jobQueue.GetSource(guid);
                    return new CreateScrapeContextPayload()
                    {
                        ClientMutationID = input.ClientMutationID,
                        ScrapeContext = context,
                        JobID = guid,
                    };
                })
                .Type<NonNullType<CreateScrapeContextPayloadType>>();

            descriptor.Field("deleteScrapeContext")
                .Description("Deletes a scrape context once it has completed. If it has not completed, the deletion will fail.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<DeleteScrapeContextInputType>())
                .Resolver(ctx =>
                {
                    var input = ctx.Argument<DeleteScrapeContextInput>("input");
                   
                    var jobQueueFactory = ctx.SnowflakeService<IAsyncJobQueueFactory>();
                    var jobQueue = jobQueueFactory.GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);

                    var jobId = input.JobID;
                    bool result = jobQueue.TryRemoveSource(jobId, out IScrapeContext scrapeContext);

                    return new DeleteScrapeContextPayload()
                    {
                        ScrapeContext = scrapeContext,
                        JobID = jobId,
                        Success = result,
                    };
                })
                .Type<NonNullType<DeleteScrapeContextPayloadType>>();

        }
    }
}
