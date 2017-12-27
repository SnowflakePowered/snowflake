using System.Linq;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Saving;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework;
using Snowflake.Support.Remoting.GraphQl.Queries;

namespace Snowflake.Support.Remoting.GraphQl.Containers
{
    public class ConfigurationQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IConfigurationCollectionStore))]
        [ImportService(typeof(IGraphQlRootSchema))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var config = coreInstance.Get<IConfigurationCollectionStore>();
            var plugin = coreInstance.Get<IPluginManager>();

            var rootSchema = coreInstance.Get<IGraphQlRootSchema>();
            var configQuery = new ConfigurationQueryBuilder(config, plugin);
            rootSchema.Register(configQuery);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Configuration GraphQL Queries.");
        }
    }
}
