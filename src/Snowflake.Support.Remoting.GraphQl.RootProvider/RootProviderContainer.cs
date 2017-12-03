using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework;
using Snowflake.Support.Remoting.GraphQl.Servers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.RootProvider
{
    public class RootProviderContainer : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var registry = serviceContainer.Get<IServiceRegistrationProvider>();
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            registry.RegisterService<IGraphQlRootSchema>(schema);
            var webServer = new GraphQlServerWrapper(new GraphQlServer(new GraphQlExecuterProvider(schema)));
            webServer.Start();
        }
    }
}
