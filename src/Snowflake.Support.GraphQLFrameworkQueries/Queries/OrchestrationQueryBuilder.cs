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
using Snowflake.Orchestration.Extensibility;
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
        public InputQueryBuilder InputQueryApi { get; }
        public ControllerLayoutQueryBuilder ControllerQueryApi { get; }

        private ConcurrentDictionary<Guid, IGameEmulation> GameEmulationCache { get; }

        public OrchestrationQueryBuilder(IPluginCollection<IEmulatorOrchestrator> orchestrators,
            IStoneProvider stone,
            IGameLibrary library,
            IEmulatedPortsManager ports,
            InputQueryBuilder inputQueryBuilder,
            ControllerLayoutQueryBuilder controllerLayoutQueryBuilder)
        {
            this.Orchestrators = orchestrators;
            this.Stone = stone;
            this.InputQueryApi = inputQueryBuilder;
            this.ControllerQueryApi = controllerLayoutQueryBuilder;
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
        [Parameter(typeof(bool), typeof(BooleanGraphType), "verify", "Whether or not to verify the game files before launching.", true)]
        public async Task<Guid> CreateGameEmulationInstance(string orchestratorName, string configurationProfile, 
            Guid gameGuid, bool verify = false)
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
            var instance = orchestrator.ProvisionEmulationInstance(game, controllers, configurationProfile, null);
            var guid = Guid.NewGuid();
            if (this.GameEmulationCache.TryAdd(guid, instance)) return guid;
            throw new InvalidOperationException("Unable to cache the created game emulation instance!");
        }
    }
}
