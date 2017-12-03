using Snowflake.Services;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Configuration;
using Snowflake.Support.Remoting.GraphQl.Framework;
using Snowflake.Support.Remoting.GraphQl.Queries;
using System.Linq;
using Snowflake.Input.Device;
using Snowflake.Input;

namespace Snowflake.Support.Remoting.GraphQl
{

    public class GraphQlContainer : IComposable
    {
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IConfigurationCollectionStore))]
        [ImportService(typeof(IGraphQlRootSchema))]
        [ImportService(typeof(IInputManager))]
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IMappedControllerElementCollectionStore))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var games = coreInstance.Get<IGameLibrary>();
            var config = coreInstance.Get<IConfigurationCollectionStore>();
            var input = coreInstance.Get<IInputManager>();
            var plugin = coreInstance.Get<IPluginManager>();
            var mapp = coreInstance.Get<IMappedControllerElementCollectionStore>();

            var rootSchema = coreInstance.Get<IGraphQlRootSchema>();
            var platformQueries = new PlatformInfoQueryBuilder(stone);
            var controllerQueries = new ControllerLayoutQueryBuilder(stone);
            var recordQueries = new RecordQueryBuilder(games, stone);
            var configQuery = new ConfigurationQueryBuilder(config);
  
            var inputQuery = new InputQueryBuilder(input, plugin, mapp, stone);

            rootSchema.Register(platformQueries);
            rootSchema.Register(controllerQueries);
            rootSchema.Register(recordQueries);
            rootSchema.Register(configQuery);
            rootSchema.Register(inputQuery);

        }
    }
}
