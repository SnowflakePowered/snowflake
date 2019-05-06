using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Snowflake.Extensibility;
using Snowflake.Framework.Extensibility;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Model.Game;
using Snowflake.Model.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Support.Remoting.GraphQL.Inputs.Scraping;
using Snowflake.Support.Remoting.GraphQL.Types.Model;
using Snowflake.Support.Remoting.GraphQL.Types.Scraping;
using static Snowflake.Scraping.Extensibility.SeedBuilder;

namespace Snowflake.Support.Remoting.GraphQL.Queries
{
    public class ScrapingQueryBuilder : QueryBuilder
    {
        public IGameLibrary GameLibrary { get; }
        private IPluginCollection<IScraper> Scrapers { get; }
        private IPluginCollection<ICuller> Cullers { get; }
        private IAsyncJobQueue<IEnumerable<ISeed>> GameScrapeContextJobQueue { get; }
        private IGameMetadataTraverser GameMetadataTraverser { get; }
        private IFileInstallationTraverser FileMetadataTraverser { get; }

        public ScrapingQueryBuilder(IGameLibrary gameLibrary,
            IPluginCollection<IScraper> scrapers,
            IPluginCollection<ICuller> cullers)
        {
            this.GameLibrary = gameLibrary;
            this.Scrapers = scrapers;
            this.Cullers = cullers;
            this.GameScrapeContextJobQueue = new AsyncJobQueue<IEnumerable<ISeed>>(false);
        }

        [Mutation("scrapeGameWithAllScrapersAuto", "Automatically scrape context with all scrapers and all cullers. " +
            "Returns the result. This is only for testing purposes, as the state of the scraping context is discarded.",
            typeof(ListGraphType<SeedGraphType>))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The GUID of the created game to proceed with.", false)]
        public async Task<IEnumerable<ISeed>> CreateAutoScrapeContextAsync(Guid gameGuid)
        {
            var game = this.GameLibrary.GetGame(gameGuid);
            return await new GameScrapeContext(game, this.Scrapers, this.Cullers);
        }

        [Mutation("beginScrapeGame", "Creates a game scraping cintext. Returns the UUID of the context.", typeof(GuidGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The game to source this scraping context  from", false)]
        [Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "scraperNames",
            "The scrapers to use for this job.")]
        [Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "cullerNames",
             "The cullers to use for this job.")]
        public async Task<Guid> CreateGameScrapeContext(Guid gameGuid, IEnumerable<string> scraperNames,
            IEnumerable<string> cullerNames)
        {
            var scrapers = this.Scrapers.Where(s => scraperNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase));
            var cullers = this.Cullers.Where(s => cullerNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase));
            var game = this.GameLibrary.GetGame(gameGuid);
            var context = new GameScrapeContext(game, scrapers.ToList(), cullers.ToList());

            return await this.GameScrapeContextJobQueue.QueueJob(context);
        }

        [Mutation("scrapeNextStep", "Proceeds with the next step of the scraping process", typeof(ListGraphType<SeedGraphType>))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the created scraping context to proceed with.", false)]
        public async Task<IEnumerable<ISeed>> ScrapeNextStep(Guid jobGuid)
        {
            (var seeds, bool hasNext) = await this.GameScrapeContextJobQueue.GetNext(jobGuid);
            return seeds;
        }

        [Mutation("scrapeAllSteps", "Proceeds with the remaining steps of the scraping process", typeof(ListGraphType<SeedGraphType>))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the created scraping context to proceed with.", false)]
        public async Task<IEnumerable<ISeed>> ScrapeAllSteps(Guid jobGuid)
        {
            return await ((GameScrapeContext)this.GameScrapeContextJobQueue.GetSource(jobGuid));
        }

        [Field("discardScrapeResults", "Discards the completed scrape results.", typeof(ListGraphType<SeedGraphType>))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the created scraping context to proceed with.", false)]
        public async Task<IEnumerable<ISeed>> DiscardScrapeResults(Guid jobGuid)
        {
            var returnVal =  await ((GameScrapeContext)this.GameScrapeContextJobQueue.GetSource(jobGuid));
            this.GameScrapeContextJobQueue.TryRemoveSource(jobGuid, out var _);
            return returnVal;
        }

        [Mutation("applyScrapeResults", "Saves the scrape result as metadata of the game.", typeof(GameGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The GUID of the game.", false)]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the created scraping context to proceed with.", false)]
        public async Task<IGame> ApplyScrapeResults(Guid gameGuid, Guid jobGuid)
        {
            // todo throw if source is gone.
            var game = this.GameLibrary.GetGame(gameGuid);
            if (game == null) throw new KeyNotFoundException("Game with the given GUID was not found.");

            var context = (GameScrapeContext)this.GameScrapeContextJobQueue.GetSource(jobGuid);
            await context; // force it to result seeds.

            await foreach (var metadata in this.GameMetadataTraverser.Traverse(game, context.Context.Root, context.Context))
            {
                // empty for loop to apply all side effects.
            }

            await foreach (var metadata in this.FileMetadataTraverser.Traverse(game, context.Context.Root, context.Context))
            {
                // empty for loop to apply all side effects.
            }

            this.GameScrapeContextJobQueue.TryRemoveSource(jobGuid, out var _);
            return game;
        }
    }
}
