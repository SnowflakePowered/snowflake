using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Model.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
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
                .Argument("input", arg => arg.Type<CreateScrapeContextInputType>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<CreateScrapeContextInput>("input");
                    var plugins = ctx.Service<IPluginManager>();
                    var cullers = plugins.GetCollection<ICuller>()
                        .Where(c => input.Cullers.Contains(c.Name, StringComparer.InvariantCultureIgnoreCase));
                    var scrapers = plugins.GetCollection<IScraper>()
                        .Where(c => input.Scrapers.Contains(c.Name, StringComparer.InvariantCultureIgnoreCase));
                    var game = await ctx.Service<IGameLibrary>().GetGameAsync(input.GameID);
                    if (game == null) throw new ArgumentException("The specified game does not exist.");
                    var jobQueueFactory = ctx.Service<IAsyncJobQueueFactory>();
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
                .Type<CreateScrapeContextPayloadType>();
        }
    }
}
