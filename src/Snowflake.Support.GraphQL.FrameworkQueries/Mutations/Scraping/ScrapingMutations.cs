using HotChocolate;
using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        .Where(c => input.Cullers.Contains(c.Name, StringComparer.InvariantCulture));
                    var scrapers = plugins.GetCollection<IScraper>()
                        .Where(c => input.Scrapers.Contains(c.Name, StringComparer.InvariantCulture));
                    var game = await ctx.SnowflakeService<IGameLibrary>().GetGameAsync(input.GameID);
                    if (game == null)
                        return ErrorBuilder.New()
                           .SetCode("SCRP_NOTFOUND_GAME")
                           .SetMessage("The specified game does not exist.")
                           .Build();
                    var jobQueueFactory = ctx.SnowflakeService<IAsyncJobQueueFactory>();
                    var jobQueue = jobQueueFactory.GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);
                    var guid = await jobQueue.QueueJob(new GameScrapeContext(game, scrapers, cullers));
                    IScrapeContext context = jobQueue.GetSource(guid);
                    ctx.AssignGameGuid(game, guid);
                    return new CreateScrapeContextPayload()
                    {
                        ClientMutationID = input.ClientMutationID,
                        ScrapeContext = context,
                        Game = game,
                        JobID = guid,
                    };
                })
                .Type<NonNullType<CreateScrapeContextPayloadType>>();

            descriptor.Field("cancelScrapeContext")
                .Description("Requests cancellation of the specified scrape context. If this succeeds, the next iteration will halt the " +
                "scrape context regardless of the stage. There is no way to determine whether or not " +
                "the cancellation succeeded until the scrape context is moved to the next step. Only then will it be eligible for deletion.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<CancelScrapeContextInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<CancelScrapeContextInput>("input");

                    var jobQueueFactory = ctx.SnowflakeService<IAsyncJobQueueFactory>();
                    var jobQueue = jobQueueFactory.GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);

                    var jobId = input.JobID;
                    var scrapeContext = jobQueue.GetSource(jobId);
                    if (scrapeContext == null)
                        return ErrorBuilder.New()
                           .SetCode("SCRP_NOTFOUND_SCRAPECONTEXT")
                           .SetMessage("The specified scrape context does not exist.")
                           .Build();
                    jobQueue.RequestCancellation(jobId);

                    var payload = new CancelScrapeContextPayload()
                    {
                        ClientMutationID = input.ClientMutationID,
                        ScrapeContext = scrapeContext,
                        JobID = jobId,
                        Game = ctx.GetAssignedGame(jobId)
                    };
                    await ctx.SendEventMessage(new OnScrapeContextCancelMessage(payload));
                    return payload;
                })
                .Type<NonNullType<CancelScrapeContextPayloadType>>();

            descriptor.Field("deleteScrapeContext")
                .Description("Deletes a scrape context, halting its execution.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<DeleteScrapeContextInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<DeleteScrapeContextInput>("input");
                   
                    var jobQueueFactory = ctx.SnowflakeService<IAsyncJobQueueFactory>();
                    var jobQueue = jobQueueFactory.GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);

                    if (!jobQueue.HasJob(input.JobID))
                        return ErrorBuilder.New()
                           .SetCode("SCRP_NOTFOUND_SCRAPECONTEXT")
                           .SetMessage("The specified scrape context does not exist.")
                           .Build();

                    jobQueue.RequestCancellation(input.JobID);
                    await jobQueue.GetNext(input.JobID);
                    bool result = jobQueue.TryRemoveSource(input.JobID, out var scrapeContext);

                    var payload = new DeleteScrapeContextPayload()
                    {
                        ScrapeContext = scrapeContext,
                        JobID = input.JobID,
                        Success = result,
                        Game = ctx.GetAssignedGame(input.JobID)
                            .ContinueWith(g =>
                            {
                                if (result) ctx.RemoveAssignment(input.JobID);
                                return g;
                            }).Unwrap()
                    };
                    await ctx.SendEventMessage(new OnScrapeContextDeleteMessage(payload));
                    return payload;
                })
                .Type<NonNullType<DeleteScrapeContextPayloadType>>();

            descriptor.Field("nextScrapeContextStep")
                .Description("Proceeds to the next step of the specified scrape context. " +
                "Returns the output of the next step in the scrape context iterator, until " +
                "it is exhausted. If the iterator is exhausted, `current` will be null, and `hasNext` will be false . " +
                "If the specified scrape context does not exist, `context` and `current` will be null, and `hasNext` will be false. ")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<NextScrapeContextStepInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<NextScrapeContextStepInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);
                    var scrapeContext = jobQueue.GetSource(input.JobID);
                    if (scrapeContext == null)
                        return ErrorBuilder.New()
                           .SetCode("SCRP_NOTFOUND_SCRAPECONTEXT")
                           .SetMessage("The specified scrape context does not exist.")
                           .Build();

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

                    var (current, movedNext) = await jobQueue.GetNext(input.JobID);
                   
                    if (movedNext)
                    {
                        var payload = new ScrapeContextStepPayload
                        {
                            ScrapeContext = scrapeContext,
                            Current = current,
                            JobID = input.JobID,
                            Game = ctx.GetAssignedGame(input.JobID)
                        };
                        await ctx.SendEventMessage(new OnScrapeContextStepMessage(payload));
                        return payload;
                    }
                    var completePayload = new ScrapeContextCompletePayload
                    {
                        ScrapeContext = scrapeContext,
                        JobID = input.JobID,
                        Game = ctx.GetAssignedGame(input.JobID)
                    };

                    await ctx.SendEventMessage(new OnScrapeContextCompleteMessage(completePayload));
                    return completePayload;

                }).Type<NonNullType<ScrapeContextPayloadInterface>>();

            descriptor.Field("exhaustScrapeContextSteps")
                .Description("Exhausts the specified scrape context until completion. " +
                "Returns the output of the last step of the scrape context, when there are no more remaining left to continue with.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<NextScrapeContextStepInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<NextScrapeContextStepInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);
                    var scrapeContext = jobQueue.GetSource(input.JobID);
                    if (scrapeContext == null)
                        return ErrorBuilder.New()
                           .SetCode("SCRP_NOTFOUND_SCRAPECONTEXT")
                           .SetMessage("The specified scrape context does not exist.")
                           .Build();

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

                    await foreach (IEnumerable<ISeed> val in jobQueue.AsEnumerable(input.JobID)) { 
                        ScrapeContextStepPayload payload = new ScrapeContextStepPayload
                        {
                            ScrapeContext = scrapeContext,
                            Current = val,
                            JobID = input.JobID,
                            Game = ctx.GetAssignedGame(input.JobID)
                        };
                        await ctx.SendEventMessage(new OnScrapeContextStepMessage(payload));
                    }

                    var completePayload = new ScrapeContextCompletePayload
                    {
                        ScrapeContext = scrapeContext,
                        JobID = input.JobID,
                        Game = ctx.GetAssignedGame(input.JobID)
                    };

                    await ctx.SendEventMessage(new OnScrapeContextCompleteMessage(completePayload));
                    return completePayload;

                }).Type<NonNullType<ScrapeContextCompletePayloadType>>();

            descriptor.Field("applyScrapeContext")
                .Description("Applies the specified scrape results to the specified game as-is. Be sure to delete the scrape context afterwards.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<ApplyScrapeContextInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<ApplyScrapeContextInput>("input");
                    var jobQueue = ctx.SnowflakeService<IAsyncJobQueueFactory>()
                        .GetJobQueue<IScrapeContext, IEnumerable<ISeed>>(false);
                    var gameLibrary = ctx.SnowflakeService<IGameLibrary>();
                    var pluginManager = ctx.SnowflakeService<IPluginManager>();
                    var fileTraversers = pluginManager.GetCollection<IFileInstallationTraverser>()
                        .Where(c => input.FileTraversers.Contains(c.Name, StringComparer.InvariantCulture));
                    
                    var metaTraversers = pluginManager.GetCollection<IGameMetadataTraverser>()
                        .Where(c => input.MetadataTraversers.Contains(c.Name, StringComparer.InvariantCulture));

                    var scrapeContext = jobQueue.GetSource(input.JobID);
                    if (scrapeContext == null)
                        return ErrorBuilder.New()
                           .SetCode("SCRP_NOTFOUND_SCRAPECONTEXT")
                           .SetMessage("The specified scrape context does not exist.")
                           .Build();
                    IGame game;

                    if (input.GameID == default)
                    {
                        game = await ctx.GetAssignedGame(input.GameID);
                    }
                    else
                    {
                        game = await gameLibrary.GetGameAsync(input.GameID);
                    }

                    if (game == null)
                        return ErrorBuilder.New()
                           .SetCode("SCRP_NOTFOUND_GAME")
                           .SetMessage("The specified game does not exist.")
                           .Build();


                    foreach (var traverser in metaTraversers)
                    {
                        await traverser.TraverseAll(game, scrapeContext.Context.Root, scrapeContext.Context);
                    }

                    foreach (var traverser in fileTraversers)
                    {
                        await traverser.TraverseAll(game, scrapeContext.Context.Root, scrapeContext.Context);
                    }

                    var payload = new ApplyScrapeContextPayload
                    {
                        Game = game,
                        ScrapeContext = scrapeContext,
                    };
                    await ctx.SendEventMessage(new OnScrapeContextApplyMessage(input.JobID, payload));
                    return payload;
                }).Type<NonNullType<ApplyScrapeContextPayloadType>>();
        }
    }
}
