using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Snowflake.Extensibility;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Saving;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Types.PlatformInfo;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class OrchestrationQueryBuilder : QueryBuilder
    {
        public IPluginCollection<IEmulatorOrchestrator> Orchestrators { get; }
        public IStoneProvider Stone { get; }
        public IGameLibrary GameLibrary { get; }
        public IEmulatedPortsManager Ports { get; }
        private ConcurrentDictionary<Guid, IGameEmulation> GameEmulationCache { get; }

        public OrchestrationQueryBuilder(IPluginCollection<IEmulatorOrchestrator> orchestrators,
            IStoneProvider stone,
            IGameLibrary library,
            IEmulatedPortsManager ports)
        {
            this.Orchestrators = orchestrators;
            this.Stone = stone;
            this.GameLibrary = library;
            this.Ports = ports;
        }

        [Query("missingSystemFiles", "Gets missing system files for the given game that the given emulator requires to run", 
            typeof(ListGraphType<SystemFileGraphType>))]
        [Parameter(typeof(string), typeof(StringGraphType), "orchestratorName", "The name of the orchestrator plugin to use.")]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The GUID of the game to launch.")]
        public IEnumerable<ISystemFile> GetMissingSystemFiles(string orchestratorName, Guid gameGuid)
        {
            var orchestrator = this.Orchestrators[orchestratorName];
            var game = this.GameLibrary.GetGame(gameGuid);
            return orchestrator.CheckMissingSystemFiles(game);
        }

        [Mutation("createEmulation", "Creates an emulation instance for a game, returning a unique handle for the instance.", typeof(GuidGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "orchestratorName", "The name of the orchestrator plugin to use.")]
        [Parameter(typeof(string), typeof(StringGraphType), "configurationProfile", "The name of the configuration profile to use.")]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The GUID of the game to launch.")]
        [Parameter(typeof(Guid?), typeof(GuidGraphType), "saveGuid", "The GUID of the save profile to use.")]
        [Parameter(typeof(bool), typeof(BooleanGraphType), "verify", "Whether or not to verify the game files before launching.")]
        public async Task<Guid> CreateGameEmulationInstance(string orchestratorName, string configurationProfile, 
            Guid gameGuid, Guid saveGuid, bool verify = false)
        {
            var orchestrator = this.Orchestrators[orchestratorName];
            var game = await this.GameLibrary.GetGameAsync(gameGuid);
            var platform = this.Stone.Platforms[game.Record.PlatformID];
            var controllers = (from i in Enumerable.Range(0, platform.MaximumInputs)
                               select this.Ports.GetControllerAtPort(orchestrator, platform.PlatformID, i)).ToList();
            if (orchestrator.CheckMissingSystemFiles(game).Any())
            {
                throw new InvalidOperationException($"Game {gameGuid} is missing BIOS prerequesites!");
            }
            if (verify)
            {
                await foreach (var x in orchestrator.ValidateGamePrerequisites(game)) ;
            }

            var save = game.WithFiles().WithSaves().GetProfile(saveGuid);
            if (save == null) throw new KeyNotFoundException($"Unable to find save profile with GUID {saveGuid}");
            var instance = orchestrator.ProvisionEmulationInstance(game, controllers, configurationProfile, save);
            var guid = Guid.NewGuid();
            if (this.GameEmulationCache.TryAdd(guid, instance)) return guid;
            throw new InvalidOperationException("Unable to cache the created game emulation instance!");
        }

        [Mutation("prepareEmulation", "Calls all hooks for the emulation instance and prepares it for start.", typeof(GuidGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "emulationHandle", "The GUID handle of a created emulation..")]
        public async Task<Guid> PrepareGameEmulation(Guid emulationHandle)
        {
            if (!this.GameEmulationCache.TryGetValue(emulationHandle, out IGameEmulation emulation))
                throw new KeyNotFoundException("Unable to find the emulation");
            await emulation.RestoreSaveGame();
            await emulation.SetupEnvironment();
            await emulation.CompileConfiguration();
            return emulationHandle;
        }

        [Mutation("startEmulation", "Starts an emulation.", typeof(GuidGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "emulationHandle", "The GUID handle of a created emulation..")]
        public Guid StartGameEmulation(Guid emulationHandle)
        {
            // todo save cancellation handle.
            if (!this.GameEmulationCache.TryGetValue(emulationHandle, out IGameEmulation emulation))
                throw new KeyNotFoundException("Unable to find the emulation");
            var cancel = emulation.StartEmulation();
            cancel.Register(async () => await emulation.DisposeAsync());
            return emulationHandle;
        }

        [Mutation("stopEmulation", "Safely halts an emulation.", typeof(GuidGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "emulationHandle", "The GUID handle of a created emulation.")]
        public Guid StopGameEmulation(Guid emulationHandle)
        {
            // todo save cancellation handle.
            if (!this.GameEmulationCache.TryGetValue(emulationHandle, out IGameEmulation emulation))
                throw new KeyNotFoundException("Unable to find the emulation");
            emulation.StopEmulation();
            return emulationHandle;
        }


    }
}
