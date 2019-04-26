using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Snowflake.Extensibility;
using Snowflake.Filesystem;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Framework.Scheduling;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQL.Types.Model;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class GameQueryBuilder : QueryBuilder
    {
        public GameQueryBuilder(IPluginCollection<IGameInstaller> installers,
            IStoneProvider stone, IAsyncJobQueue<TaskResult<IFile>> installQueue, IGameLibrary gameLibrary)
        {
            this.Installers = installers;
            this.Stone = stone;
            this.InstallQueue = installQueue;
            this.GameLibrary = gameLibrary;
        }

        IPluginCollection<IGameInstaller> Installers { get; }
        IStoneProvider Stone { get; }
        IAsyncJobQueue<TaskResult<IFile>> InstallQueue { get; }
        IGameLibrary GameLibrary { get; }

        [Mutation("beginInstallGame", "Creates a game install job. Returns the UUID of the created game", typeof(GuidGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "platform", "The platform to install this game for", false)]
        [Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "files",
            "A list of filenames as part of the game", false)]
        public async Task<Guid> CreateGameInstallation(string platform, IEnumerable<string> files)
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
            if (installer == null) throw new KeyNotFoundException("Platform Not Found");
            return await this.InstallQueue.QueueJob(installer.Install(game, filesysinfo), game.Record.RecordId);
        }

        [Mutation("installNextStep", "Proceeds with the next step of the installation process", typeof(FileGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid","The GUID of the created game to proceed with.", false)]
        public async Task<IFile> InstallNextStep(Guid gameGuid)
        {
            (var file, bool hasNext) = await this.InstallQueue.GetNext(gameGuid);
            if (file.Error != null)
            {
                throw file.Error;
            }
            return await file;
        }

        [Mutation("installAllSteps", "Proceeds with all steps of the installation process", typeof(FileGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The GUID of the created game to proceed with.", false)]
        public async Task<IEnumerable<IFile>> InstallAllSteps(Guid gameGuid)
        {
            IList<IFile> results = new List<IFile>();
            for((var file, bool hasNext) = await this.InstallQueue.GetNext(gameGuid); 
                hasNext; (file, hasNext) = await this.InstallQueue.GetNext(gameGuid))
            {
                if (file.Error != null) throw file.Error;
                results.Add(await file);
            }

            return results;
        }
    }
}
