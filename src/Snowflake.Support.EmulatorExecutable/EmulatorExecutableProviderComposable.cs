using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Execution.Process;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Support.EmulatorExecutable
{
    public class EmulatorExecutableProviderComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IModuleEnumerator))]
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var modules = serviceContainer.Get<IModuleEnumerator>();
            var logProvider = serviceContainer.Get<ILogProvider>();

            var loader = new EmulatorExecutableProvider(logProvider.GetLogger("EmulatorExecutableLoader"), modules);
            register.RegisterService<IEmulatorExecutableProvider>(loader);
        }
    }
}
