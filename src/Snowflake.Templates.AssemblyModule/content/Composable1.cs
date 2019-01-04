using System;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Templates.AssemblyModule
{
    public class Composable1 : IComposable
    {
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            serviceContainer.Get<ILogProvider>()
                .GetLogger(composableModule.Name)
                .Info("Hello World!");
        }
    }
}
