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
    public class ExtensibilityQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IGraphQlRootSchema))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IModuleEnumerator))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var plugin = coreInstance.Get<IPluginManager>();
            var rootSchema = coreInstance.Get<IGraphQlRootSchema>();
            var moduleEnumerator = coreInstance.Get<IModuleEnumerator>();
            var moduleQuery = new ExtensibilityQueryBuilder(moduleEnumerator, plugin);
            rootSchema.Register(moduleQuery);

            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Extensibiity Introspection GraphQL Queries.");
        }
    }
}
