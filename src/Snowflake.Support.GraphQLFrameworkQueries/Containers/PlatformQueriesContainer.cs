using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Configuration;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone;
using Snowflake.Framework.Remoting.GraphQL.Schema;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Game;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.PlatformInfo;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class PlatformQueriesContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IGraphQLSchemaRegistrationProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var game = coreInstance.Get<IGameLibrary>();
            var hotChocolate = coreInstance.Get<IGraphQLSchemaRegistrationProvider>();

      
            var platformQueries = new PlatformInfoQueryBuilder(stone);
            var gameQueries = new GameQueryBuilder(game);

            hotChocolate.AddQuery<PlatformQueries, PlatformInfoQueryBuilder>(platformQueries);
            hotChocolate.AddQuery<GameQueries, GameQueryBuilder>(gameQueries);
        }
    }
}
