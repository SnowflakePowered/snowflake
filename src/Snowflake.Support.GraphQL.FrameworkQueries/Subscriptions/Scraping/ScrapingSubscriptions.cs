using HotChocolate.Resolvers;
using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Scraping;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL;

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

            descriptor.Name("Subscription");

            descriptor.Field("onScrapeContextStep")
                .Type<NonNullType<ScrapeContextStepPayloadType>>()
                .Argument("jobId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnScrapeContextStepMessage>();
                    return message.Payload;
                });
            descriptor.Field("allScrapeContextSteps")
                .Description("Proceeds with the continuation of the scrape context until the iterator is exhausted.")
                .Type<NonNullType<ScrapeContextStepPayloadType>>()
                .Argument("input", arg => arg.Type<NonNullType<NextScrapeContextStepInputType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<ScrapeContextStepPayload>();
                    return message;
                })
                .Subscribe(ctx => YieldSteps(ctx));
        }
    }
}
