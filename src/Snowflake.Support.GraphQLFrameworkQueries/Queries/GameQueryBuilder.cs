using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Framework.Scheduling;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Services;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class GameQueryBuilder : QueryBuilder
    {
        public GameQueryBuilder(IPluginCollection<IGameInstaller> installers,
            IStoneProvider stone, IAsyncJobQueue<ITaskResult> installQueue, IGameLibrary gameLibrary)
        {
            this.Installers = installers;
            this.Stone = stone;
            this.InstallQueue = installQueue;
            this.GameLibrary = gameLibrary;
        }

        IPluginCollection<IGameInstaller> Installers { get; }
        IStoneProvider Stone { get; }
        IAsyncJobQueue<ITaskResult> InstallQueue { get; }
        IGameLibrary GameLibrary { get; }
        //[Field("autoScrape", "Automatically results scrape to end.", typeof(ListGraphType<SeedGraphType>))]
        //[Parameter(typeof(string), typeof(StringGraphType), "platform", "platform")]
        //[Parameter(typeof(string), typeof(StringGraphType), "title", "title")]
        //[Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "scraperNames",
        //    "The scrapers to use for this job.")]
        //[Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "cullerNames",
        //    "The cullers to use for this job.")]
        //public async Task<IList<ISeed>> AutoScrape(string platform, string title,
        //    IEnumerable<string> scraperNames, IEnumerable<string> cullerNames)
        //{
        //    var job = this.ScrapeEngine.CreateJob(__(("platform", platform), ("search_title", title)),
        //        this.Scrapers.Where(s => scraperNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase)),
        //        this.Cullers.Where(s => cullerNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase)));
        //    while (await this.ScrapeEngine.ProceedJob(job))
        //    {
        //    }

        //    return this.ScrapeEngine.GetJobState(job).ToList();
        //}

        public Guid CreateGameInstallation(string platform, IEnumerable<string> files)
        {
            var filesysinfo = files.Select<string, FileSystemInfo>(s =>
            {
                if (File.Exists(s)) {
                    return new FileInfo(s);
                    }
                else if (Directory.Exists(s))
                {
                    return new DirectoryInfo(s);
                }
                return null;
            }).Where(f => f != null).ToList();

            var game = this.GameLibrary.CreateGame(platform);

            var installer = this.Installers.FirstOrDefault(p => p.SupportedPlatforms.Contains(platform));
            return this.InstallQueue.QueueJob(installer.Install(game, filesysinfo), game.Record.RecordId));
        }
    }
}
