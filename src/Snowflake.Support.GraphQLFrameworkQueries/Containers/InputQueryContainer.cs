using System.Linq;
using Snowflake.Configuration;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Queries;

namespace Snowflake.Support.GraphQLFrameworkQueries.Containers
{
    public class InputQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IDeviceEnumerator))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IControllerElementMappingProfileStore))]
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IGameLibrary))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var input = coreInstance.Get<IDeviceEnumerator>();
            var mappedController = coreInstance.Get<IControllerElementMappingProfileStore>();
            var rootSchema = coreInstance.Get<IGraphQLService>();

            var inputQuery = new InputQueryBuilder(input, mappedController, stone);
            rootSchema.Register(inputQuery);
            var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            logger.Info("Registered Input GraphQL Queries.");

            //    var gameLib = coreInstance.Get<IGameLibrary>();

            //    var rootSchema = coreInstance.Get<IGraphQLService>();
            //    var inputQuery = new InputQueryBuilder(input, plugin, mappedController, stone);
            //    var controllerQueries = new ControllerLayoutQueryBuilder(stone);
            //    var emuQuery = new EmulationQueryBuilder(plugin.GetCollection<IEmulator>(), stone, gameLib,
            //        inputQuery, controllerQueries);

            //    rootSchema.Register(controllerQueries);
            //    rootSchema.Register(inputQuery);
            //    rootSchema.Register(emuQuery);
            //    var logger = coreInstance.Get<ILogProvider>().GetLogger("graphql");
            //    logger.Info("Registered Controller GraphQL Queries.");
            //    logger.Info("Registered Emulation GraphQL Queries.");
        }
    }
}
