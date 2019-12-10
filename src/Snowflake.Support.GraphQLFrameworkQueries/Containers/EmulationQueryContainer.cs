using System.Linq;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Framework.Remoting.GraphQL;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQL.Queries;

namespace Snowflake.Support.Remoting.GraphQL.Containers
{
    public class EmulationQueryContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        //[ImportService(typeof(IInputManager))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IControllerElementMappingsStore))]
        [ImportService(typeof(IGraphQLService))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IGameLibrary))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            //    var stone = coreInstance.Get<IStoneProvider>();
            //    var input = coreInstance.Get<IInputManager>();
            //    var plugin = coreInstance.Get<IPluginManager>();
            //    var mappedController = coreInstance.Get<IControllerElementMappingsStore>();
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
            //    logger.Info("Registered Input GraphQL Queries.");
            //    logger.Info("Registered Emulation GraphQL Queries.");
        }
    }
}
