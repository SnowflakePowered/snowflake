using Snowflake.Services;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Configuration;
using Snowflake.Support.Remoting.GraphQl.Servers;
using Snowflake.Support.Remoting.GraphQl.Framework;
using Snowflake.Support.Remoting.GraphQl.Queries;
using System.Linq;
using Snowflake.Input.Device;

namespace Snowflake.Support.Remoting.GraphQl
{

    public class GraphQlContainer : IComposable
    {
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IGameLibrary))]
        [ImportService(typeof(IConfigurationCollectionStore))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IInputManager))]
        [ImportService(typeof(IPluginManager))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var games = coreInstance.Get<IGameLibrary>();
            var config = coreInstance.Get<IConfigurationCollectionStore>();
            var input = coreInstance.Get<IInputManager>();
            var plugin = coreInstance.Get<IPluginManager>();
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new SnowflakeSchema(root, mutation);
            var platformQueries = new PlatformInfoQueryBuilder(stone);
            var controllerQueries = new ControllerLayoutQueryBuilder(stone);
            var recordQueries = new RecordQueryBuilder(games, stone);
            var configQuery = new ConfigurationQueryBuilder(config);
            platformQueries.RegisterConnectionQueries(root);
            platformQueries.RegisterFieldQueries(root);
            controllerQueries.RegisterConnectionQueries(root);
            controllerQueries.RegisterFieldQueries(root);
            recordQueries.RegisterConnectionQueries(root);
            recordQueries.RegisterFieldQueries(root);
            recordQueries.RegisterMutationQueries(mutation);
            configQuery.RegisterConnectionQueries(root);
            var inputQuery = new InputQueryBuilder(input, plugin);
            inputQuery.RegisterConnectionQueries(root);
            var webServer = new GraphQlServerWrapper(new GraphQlServer(new GraphQlExecuterProvider(schema)));
            webServer.Start();
            //register.RegisterService<ILocalWebService>(webServer); //todo should be plugin.
        }
    }
}
