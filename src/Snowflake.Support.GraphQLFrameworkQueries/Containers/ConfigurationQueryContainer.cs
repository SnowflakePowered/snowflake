using System.Linq;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQL.Queries;

namespace Snowflake.Support.Remoting.GraphQL.Containers
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
