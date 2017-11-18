using Snowflake.Services;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Configuration;
using Snowflake.Support.Remoting.GraphQl.Servers;
using Snowflake.Support.Remoting.GraphQl.Framework;
using Snowflake.Support.Remoting.GraphQl.Queries;
using System.Linq;

namespace Snowflake.Support.Remoting.GraphQl
{

    public class GraphQlContainer : IComposable
    {
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var root = new RootQuery();
            var schema = new SnowflakeSchema(root);
            var platformQueries = new PlatformInfoQueryBuilder(stone);
            platformQueries.RegisterConnectionQueries(root);
            platformQueries.RegisterFieldQueries(root);
            var webServer = new GraphQlServerWrapper(new GraphQlServer(new GraphQlExecuterProvider(schema)));
            webServer.Start();
            //register.RegisterService<ILocalWebService>(webServer); //todo should be plugin.
        }
    }
}
