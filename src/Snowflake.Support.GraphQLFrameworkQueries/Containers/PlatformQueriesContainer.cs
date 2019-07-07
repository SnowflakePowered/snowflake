using System.Linq;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQL.Queries;

namespace Snowflake.Support.Remoting.GraphQL.Containers
{
    public class PlatformQueriesContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var rootSchema = coreInstance.Get<IGraphQLService>();
            var platformQueries = new PlatformInfoQueryBuilder(stone);
            rootSchema.Register(platformQueries);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Platform GraphQL Queries.");
        }
    }
}
