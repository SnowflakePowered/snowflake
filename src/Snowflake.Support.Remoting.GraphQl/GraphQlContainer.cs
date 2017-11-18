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
            var query = new SnowflakeQuery(stone);
            var schema = new SnowflakeSchema(query);
            var platformQueries = new PlatformInfoQueryBuilder(stone);
            var pQuery = platformQueries.EnumerateFieldQueries().First();
            var madeQuery = platformQueries.MakeFieldQuery(pQuery.fieldMethod, pQuery.fieldAttr, pQuery.paramAttr);
            platformQueries.RegisterQuery(madeQuery, query);

            var pQueryE = platformQueries.EnumerateConnectionQueries().First();
            var madeQueryE = platformQueries.MakeConnectionQuery(pQueryE.fieldMethod, pQueryE.connectionAttr, pQueryE.paramAttr);
            platformQueries.RegisterQuery(madeQueryE, query);

            var webServer = new GraphQlServerWrapper(new GraphQlServer(new GraphQlExecuterProvider(schema)));
            webServer.Start();
            //register.RegisterService<ILocalWebService>(webServer); //todo should be plugin.

        }
    }
}
