using System.Linq;
using Snowflake.Configuration;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class RecordQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            //var stone = coreInstance.Get<IStoneProvider>();
            //var games = coreInstance.Get<IGameLibrary>();
            //var rootSchema = coreInstance.Get<IGraphQLService>();
            //var recordQueries = new RecordQueryBuilder(games, stone);
            //rootSchema.Register(recordQueries);
            //var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            //logger.Info("Registered Record GraphQL Queries.");
        }
    }
}
