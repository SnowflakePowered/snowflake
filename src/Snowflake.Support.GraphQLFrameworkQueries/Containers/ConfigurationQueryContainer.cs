using System.Linq;
using Snowflake.Extensibility.Configuration;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class ConfigurationQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IPluginManager))]
        //   [ImportService(typeof(IConfigurationCollectionStore))]
        [ImportService(typeof(IPluginConfigurationStore))]
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            //         var gameConfig = coreInstance.Get<IConfigurationCollectionStore>();
            var plugin = coreInstance.Get<IPluginManager>();
            var pluginConfig = coreInstance.Get<IPluginConfigurationStore>();

            var rootSchema = coreInstance.Get<IGraphQLService>();
            var configQuery = new ConfigurationQueryBuilder(pluginConfig, plugin);
            rootSchema.Register(configQuery);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Configuration GraphQL Queries.");
        }
    }
}
