using HotChocolate.Resolvers;
using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Scraping;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL;
using Snowflake.Model.Game;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using System.Linq;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping
{
    internal sealed class ScrapingSubscriptions
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            static async IAsyncEnumerable<ScrapeContextStepPayload> YieldSteps(IResolverContext ctx)
            {
                var input = ctx.Argument<NextScrapeContextStepInput>("input");
                
                var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                    .GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);
                if (!jobQueue.HasJob(input.JobID)) yield break;
                var scrapeContext = jobQueue.GetSource(input.JobID);

                var addSeeds = input.Seeds;
                if (addSeeds != null)
                {
                    foreach (var graft in addSeeds)
                    {
                        var seed = scrapeContext.Context[graft.SeedID];
                        if (seed == null) continue;
                        var seedTree = graft.Tree.ToSeedTree();
                        var seedGraft = seedTree.Collapse(seed, ISeed.ClientSource);
                        scrapeContext.Context.AddRange(seedGraft);
                    }
                }

                await foreach (IEnumerable<ISeed> step in jobQueue.GetSource(input.JobID))
                {
                    var payload = new ScrapeContextStepPayload
                    {
                        ScrapeContext = scrapeContext,
                        Current = step,
                        HasNext = true,
                        JobID = input.JobID
                    };
                    await ctx.SendEventMessage(new OnScrapeContextStepMessage(input.JobID, payload));
                    yield return payload;
                }

                // We need to yield the final result of the iterable
                var finalPayload = new ScrapeContextStepPayload
                {
                    ScrapeContext = scrapeContext,
                    Current = null,
                    HasNext = false,
                    JobID = input.JobID
                };
                await ctx.SendEventMessage(new OnScrapeContextStepMessage(input.JobID, finalPayload));
                yield return finalPayload;
            }

            static async IAsyncEnumerable<ApplyScrapeResultsPayload> YieldApplyResults(IResolverContext ctx)
            {
                var input = ctx.Argument<ApplyScrapeResultsInput>("input");
                var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                    .GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);
                var gameLibrary = ctx.SnowflakeService<IGameLibrary>();
                var pluginManager = ctx.SnowflakeService<IPluginManager>();
                var fileTraversers = pluginManager.GetCollection<IFileInstallationTraverser>()
                    .Where(c => input.FileTraversers.Contains(c.Name, StringComparer.InvariantCultureIgnoreCase));

                var metaTraversers = pluginManager.GetCollection<IGameMetadataTraverser>()
                    .Where(c => input.MetadataTraversers.Contains(c.Name, StringComparer.InvariantCultureIgnoreCase));

                var scrapeContext = jobQueue.GetSource(input.JobID);
                if (scrapeContext == null) throw new ArgumentException("The specified scrape context does not exist.");
                IGame? game = null;

                if (input.GameID == default)
                {
                    var recordSeed = scrapeContext.Context.GetAllOfType("scrapecontext_record").FirstOrDefault();
                    if (recordSeed == null) throw new ArgumentException("No valid game found to apply this scrape context.");
                    game = await gameLibrary.GetGameAsync(Guid.Parse(recordSeed.Content.Value));
                }
                else
                {
                    game = await gameLibrary.GetGameAsync(input.GameID);
                }

                if (game == null) throw new ArgumentException("The specified game does not exist.");


                foreach (var traverser in metaTraversers)
                {
                    await traverser.TraverseAll(game, scrapeContext.Context.Root, scrapeContext.Context);
                }

                foreach (var traverser in fileTraversers)
                {
                    await traverser.TraverseAll(game, scrapeContext.Context.Root, scrapeContext.Context);
                }

                jobQueue.TryRemoveSource(input.JobID, out var _);

                yield return new ApplyScrapeResultsPayload
                {
                    Game = game,
                    ScrapeContext = scrapeContext,
                    ClientMutationID = input.ClientMutationID,
                };
            }

            descriptor.Name("Subscription");

            descriptor.Field("onScrapeContextStep")
                .Type<NonNullType<ScrapeContextStepPayloadType>>()
                .Argument("jobId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnScrapeContextStepMessage>();
                    return message.Payload;
                });

            descriptor.Field("exhaustScrapeContextStepsAsync")
                .Description("Proceeds with the continuation of the scrape context until the iterator is exhausted.")
                .Type<NonNullType<ScrapeContextStepPayloadType>>()
                .Argument("input", arg => arg.Type<NonNullType<NextScrapeContextStepInputType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<ScrapeContextStepPayload>();
                    return message;
                })
                .Subscribe(ctx => YieldSteps(ctx));

            descriptor.Field("applyScrapeResultsAsync")
               .Description("Applies the specified scrape results to the specified game as-is.")
               .Type<NonNullType<ScrapeContextStepPayloadType>>()
               .Argument("input", arg => arg.Type<NonNullType<ApplyScrapeResultsInputType>>())
               .Resolver(ctx =>
               {
                   var message = ctx.GetEventMessage<ApplyScrapeResultsPayload>();
                   return message;
               })
               .Subscribe(ctx => YieldSteps(ctx));
        }
    }
}
