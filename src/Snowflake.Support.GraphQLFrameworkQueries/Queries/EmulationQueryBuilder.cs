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
        public InputQueryBuilder InputQueryApi { get; }
        public ControllerLayoutQueryBuilder ControllerQueryApi { get; }

        public EmulationQueryBuilder(IPluginCollection<IEmulatorOrchestrator> orchestrators,
            IStoneProvider stone,
            IGameLibrary library,
            InputQueryBuilder inputQueryBuilder,
            ControllerLayoutQueryBuilder controllerLayoutQueryBuilder)
        {
            this.Orchestrators = orchestrators;
            this.Stone = stone;
            this.InputQueryApi = inputQueryBuilder;
            this.ControllerQueryApi = controllerLayoutQueryBuilder;
            this.GameLibrary = library;
        }

        public Guid CreateGameEmulationInstance(string emulatorName)
        {
            var orchestrator = this.Orchestrators[emulatorName];
            return new Guid();
        }
        
    }
}
