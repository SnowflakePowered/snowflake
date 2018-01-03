using System.Linq;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Saving;
using Snowflake.Framework.Remoting.GraphQl;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Queries;

namespace Snowflake.Support.Remoting.GraphQl.Containers
{
    public class EmulationQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IInputManager))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IMappedControllerElementCollectionStore))]
        [ImportService(typeof(ISaveLocationProvider))]
        [ImportService(typeof(IGraphQlRootSchema))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var input = coreInstance.Get<IInputManager>();
            var plugin = coreInstance.Get<IPluginManager>();
            var mappedController = coreInstance.Get<IMappedControllerElementCollectionStore>();
            var saves = coreInstance.Get<ISaveLocationProvider>();

            var rootSchema = coreInstance.Get<IGraphQlRootSchema>();
            var inputQuery = new InputQueryBuilder(input, plugin, mappedController, stone);
            var controllerQueries = new ControllerLayoutQueryBuilder(stone);
            var emuQuery = new EmulationQueryBuilder(plugin.GetCollection<IEmulator>(), stone, saves,
                inputQuery, controllerQueries);

            rootSchema.Register(controllerQueries);
            rootSchema.Register(inputQuery);
            rootSchema.Register(emuQuery);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Controller GraphQL Queries.");
            logger.Info("Registered Input GraphQL Queries.");
            logger.Info("Registered Emulation GraphQL Queries.");
        }
    }
}
