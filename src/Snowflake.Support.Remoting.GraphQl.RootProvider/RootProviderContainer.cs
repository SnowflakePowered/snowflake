using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Framework.Remoting.GraphQl;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Servers;

namespace Snowflake.Support.Remoting.GraphQl.RootProvider
{
    public class RootProviderContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var registry = serviceContainer.Get<IServiceRegistrationProvider>();
            var logger = serviceContainer.Get<ILogProvider>();
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            registry.RegisterService<IGraphQlRootSchema>(schema);
            var webServer = new GraphQlServerWrapper(new GraphQlServer(new GraphQlExecuterProvider(schema),
                logger.GetLogger("GraphQLServer")));
            webServer.Start();
        }
    }
}
