using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Execution.Process;
using Snowflake.Execution.Saving;
using Snowflake.Execution.SystemFiles;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Support.Execution
{
    public class EmulatorExecutableSupportComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IModuleEnumerator))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IContentDirectoryProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var modules = serviceContainer.Get<IModuleEnumerator>();
            var logProvider = serviceContainer.Get<ILogProvider>();
            var cdp = serviceContainer.Get<IContentDirectoryProvider>();

            var loader = new EmulatorExecutableProvider(logProvider.GetLogger("EmulatorExecutableLoader"), modules);
            register.RegisterService<IEmulatorExecutableProvider>(loader);

            //var emucdp = new EmulatorTaskRootDirectoryProvider(cdp);
            //register.RegisterService<IEmulatorTaskRootDirectoryProvider>(emucdp);
        }
    }
}
