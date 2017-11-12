using Snowflake.Services;
using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Configuration;
using Snowflake.Support.Remoting.GraphQl.Servers;
using Snowflake.Support.Remoting.GraphQl.Framework;
using Snowflake.Support.Remoting.GraphQl.Queries;

namespace Snowflake.Support.Remoting.GraphQl
{

    public class GraphQlContainer : IComposable
    {
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            var stone = coreInstance.Get<IStoneProvider>();
            var query = new SnowflakeQuery(coreInstance.Get<IStoneProvider>());
            var schema = new SnowflakeSchema(query);

            var webServer = new GraphQlServerWrapper(new GraphQlServer(new GraphQlExecuterProvider(schema)));
            webServer.Start();
            //register.RegisterService<ILocalWebService>(webServer); //todo should be plugin.

        }
    }
}
