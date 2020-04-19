using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.Kestrel;
using Snowflake.Loader;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.Server
{
    public sealed class GraphQLServicesComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IKestrelWebServerService))]
        [ImportService(typeof(IPrivilegedServiceContainerAccessor))]
        [ImportService(typeof(ILogProvider))]

        public void Compose(IModule composableModule, IServiceRepository serviceRepository)
        {
            var logger = serviceRepository.Get<ILogProvider>().GetLogger("HotChocolate");
            var register = serviceRepository.Get<IServiceRegistrationProvider>();
            var kestrel = serviceRepository.Get<IKestrelWebServerService>();
            var services = serviceRepository.Get<IPrivilegedServiceContainerAccessor>();

            var schema = new GraphQLSchemaRegistrationProvider(logger);
            var hotChocolate = new HotChocolateKestrelIntegration(schema,
                services.GetServiceContainerAsServiceProvider());

            register.RegisterService<IGraphQLSchemaRegistrationProvider>(schema);
            kestrel.AddService(hotChocolate);
        }
    }
}
