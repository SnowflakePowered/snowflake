using System;
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

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class EmulationQueryBuilder : QueryBuilder
    {
        public IPluginCollection<IEmulatorOrchestrator> Orchestrators { get; }
        public IStoneProvider Stone { get; }
        public IGameLibrary GameLibrary { get; }
        public IEmulatedPortsManager Ports { get; }
        public InputQueryBuilder InputQueryApi { get; }
        public ControllerLayoutQueryBuilder ControllerQueryApi { get; }

        public EmulationQueryBuilder(IPluginCollection<IEmulatorOrchestrator> orchestrators,
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

        public Guid CreateGameEmulationInstance(string emulatorName, string configurationProfile, Guid gameGuid)
        {
            var orchestrator = this.Orchestrators[emulatorName];
            var game = this.GameLibrary.GetGame(gameGuid);
            var platform = this.Stone.Platforms[game.Record.PlatformID];
            var controllers = (from i in Enumerable.Range(0, platform.MaximumInputs)
                               select this.Ports.GetControllerAtPort(orchestrator, platform.PlatformID, i)).ToList();
            orchestrator.ProvisionEmulationInstance(game, controllers, configurationProfile, null);
            return new Guid();
        }
        
    }
}
