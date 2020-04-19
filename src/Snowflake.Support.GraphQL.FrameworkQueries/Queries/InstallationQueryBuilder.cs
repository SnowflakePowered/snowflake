using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using Snowflake.Extensibility;
using Snowflake.Filesystem;
using Snowflake.Remoting.GraphQL.Attributes;
using Snowflake.Remoting.GraphQL.Query;
using Snowflake.Installation;
using Snowflake.Installation.Extensibility;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Support.GraphQLFrameworkQueries.Types.Installable;
using Snowflake.Support.GraphQLFrameworkQueries.Types.Model;
using Snowflake.Extensibility.Queueing;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class InstallationQueryBuilder : QueryBuilder
    {
        public InstallationQueryBuilder(IPluginCollection<IGameInstaller> installers,
            IPluginCollection<IEmulatorOrchestrator> orchestrators,
            IAsyncJobQueue<TaskResult<IFile>> installQueue, 
            IGameLibrary gameLibrary)
        {
            this.Orchestrators = orchestrators;
            this.Installers = installers;
            this.InstallQueue = installQueue;
            this.GameLibrary = gameLibrary;
        }

        IPluginCollection<IGameInstaller> Installers { get; }
        IPluginCollection<IEmulatorOrchestrator> Orchestrators { get; }

        IAsyncJobQueue<TaskResult<IFile>> InstallQueue { get; }
        IGameLibrary GameLibrary { get; }

        [Connection("installables", "Retrieves the installable for the given set of files.", typeof(InstallableGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "platform", "The platform to install this game for.", false)]
        [Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "files",
            "A list of filenames as part of the game to install.", false)]
        public IEnumerable<InstallableGraphObject> GetInstallables(string platform, IEnumerable<string> files)
        {
            var filesysinfo = files.Select<string, FileSystemInfo>(s =>
                (File.Exists(s), Directory.Exists(s)) switch
                {
                    (true, _) => new FileInfo(s),
                    (_, true) => new DirectoryInfo(s),
                    (false, false) => null
                }).Where(f => f != null).ToList();

            return from installer in this.Installers
                   let installables = installer.GetInstallables(platform, filesysinfo)
                   from installable in installables
                   select new InstallableGraphObject(installer, installable);
        }

        [Mutation("createGame", "Creates a new game entry", typeof(GuidGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "platformId", "The platform of the game entry.", false)]
        public async Task<Guid> CreateGame(string platformId)
        {
            var game = await this.GameLibrary.CreateGameAsync(platformId);
            return game.Record.RecordID;
        }

        [Mutation("createGameInstall", "Creates a game install job. Returns a handle to the installation job. ", typeof(GuidGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "installerName", "The name of the installer to use.", false)]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The game for which to install the files.", false)]
        [Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "files",
            "A list of filenames as part of the game", false)]
        public async Task<Guid> CreateGameInstallation(string installerName, Guid gameGuid, IEnumerable<string> files)
        {
            var filesysinfo = files.Select<string, FileSystemInfo>(s => 
                (File.Exists(s), Directory.Exists(s)) switch
                {
                     (true, _) => new FileInfo(s),
                     (_, true) => new DirectoryInfo(s),
                     (false, false) => null
                }).Where(f => f != null).ToList();

            var game = await this.GameLibrary.GetGameAsync(gameGuid);
            //todo better excepion here
            if (game == null) throw new KeyNotFoundException("Game Not Found");

            var installer = this.Installers.FirstOrDefault(p => p.Name == installerName 
                && p.SupportedPlatforms.Contains(game.Record.PlatformID));

            if (installer == null) throw new KeyNotFoundException("Installer Not Found");
           
            return await this.InstallQueue.QueueJob(installer.Install(game, filesysinfo));
        }

        [Mutation("installNextStep", "Proceeds with the next step of the installation process", typeof(FileGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid","The GUID of the job to proceed with.", false)]
        public async Task<IFile> InstallNextStep(Guid jobGuid)
        {
            (var file, bool _) = await this.InstallQueue.GetNext(jobGuid);
            if (file.Error != null)
            {
                throw file.Error;
            }
            return await file;
        }

        [Mutation("createGameVerification",
            "Creates a new installation job from the orchestrator verification. Returns a handle to the installation job.",
            typeof(GuidGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The GUID of the created game to proceed with.", false)]
        [Parameter(typeof(string), typeof(StringGraphType), "orchestratorName", "The GUID of the created game to proceed with.", false)]

        public async Task<Guid> CreateGameVerification(string orchestratorName, Guid gameGuid)
        {
            var orchestrator = this.Orchestrators[orchestratorName];
            if (orchestrator == null) throw new KeyNotFoundException("Installer Not Found");
            var game = await this.GameLibrary.GetGameAsync(gameGuid);
            if (game == null) throw new KeyNotFoundException("Game Not Found");
            return await this.InstallQueue.QueueJob(orchestrator.ValidateGamePrerequisites(game));
        }

        [Mutation("installAllSteps", "Proceeds with all steps of the installation process", typeof(ListGraphType<FileGraphType>))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the job to proceed with.", false)]
        public async Task<IEnumerable<IFile>> InstallAllSteps(Guid jobGuid)
        {
           
            IList<IFile> results = new List<IFile>();

            await foreach (var file in this.InstallQueue.AsEnumerable(jobGuid))
            {
                if (file.Error != null) throw file.Error;
                results.Add(await file);
            }

            return results;
        }
    }
}
