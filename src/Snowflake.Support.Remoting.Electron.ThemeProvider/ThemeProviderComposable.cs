using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.Electron;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Remoting.GraphQL;
using Snowflake.Support.Remoting.Electron.ThemeProvider.GraphQL;

namespace Snowflake.Support.Remoting.Electron.ThemeProvider
{
    public class ThemeProviderComposable : IComposable
    {
        [ImportService(typeof(IModuleEnumerator))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IGraphQLSchemaRegistrationProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var modules = serviceContainer.Get<IModuleEnumerator>();
            var log = serviceContainer.Get<ILogProvider>().GetLogger("electrontheme");
            var registry = serviceContainer.Get<IServiceRegistrationProvider>();
            var packageProvider = new ElectronPackageProvider(log, modules);
            var graphQl = serviceContainer.Get<IGraphQLSchemaRegistrationProvider>();
            
            registry.RegisterService<IElectronPackageProvider>(packageProvider);
            graphQl.AddObjectTypeExtension<ElectronQueries>();
        }
    }
}
