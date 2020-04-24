using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.Electron;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Support.Remoting.Electron.ThemeProvider
{
    public class ThemeProviderComposable : IComposable
    {
        [ImportService(typeof(IModuleEnumerator))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var modules = serviceContainer.Get<IModuleEnumerator>();
            var log = serviceContainer.Get<ILogProvider>().GetLogger("electrontheme");
            var registry = serviceContainer.Get<IServiceRegistrationProvider>();

            //var packageProvider = new ElectronPackageProvider(log, modules);
            //var endpoint = new ElectronPackageQueries(packageProvider);

            //queryEndpoint.Register(endpoint);
            //registry.RegisterService<IElectronPackageProvider>(packageProvider);
        }
    }
}
