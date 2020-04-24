using System.Linq;
using Snowflake.Configuration;
using Snowflake.Remoting.GraphQL;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class OrchestrationQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IDeviceEnumerator))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IControllerElementMappingProfileStore))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IEmulatedPortsManager))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var input = coreInstance.Get<IDeviceEnumerator>();
            var plugin = coreInstance.Get<IPluginManager>();
            var library = coreInstance.Get<IGameLibrary>();
            var ports = coreInstance.Get<IEmulatedPortsManager>();

        }
    }
}
