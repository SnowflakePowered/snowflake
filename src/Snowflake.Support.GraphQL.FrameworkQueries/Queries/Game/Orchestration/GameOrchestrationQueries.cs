using HotChocolate.Types;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Game.Orchestration
{
    public sealed class GameOrchestrationQueries
        : ObjectTypeExtension<IGame>
    {
        protected override void Configure(IObjectTypeDescriptor<IGame> descriptor)
        {
            descriptor.ExtendGame();
            descriptor.Field("orchestration")
                .Description("Provides access to game orchestration information for orchestrators that support this game.")
                .Resolve(ctx =>
                {
                    var orchestrators = ctx.SnowflakeService<IPluginManager>()
                        .GetCollection<IEmulatorOrchestrator>();
                    var game = ctx.Parent<IGame>();

                    return orchestrators
                        .Where(o => o.CheckCompatibility(game) != EmulatorCompatibility.Unsupported)
                        .Select(orchestrator => (game, orchestrator));
                })
                .Type<NonNullType<ListType<NonNullType<GameOrchestratorType>>>>();
        }
    }
}
