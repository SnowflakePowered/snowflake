using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Orchestration.Process;
using Snowflake.Orchestration.Saving;
using Snowflake.Orchestration.SystemFiles;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Input.Controller;

namespace Snowflake.Support.Execution
{
    public class OrchestrationProviderComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IModuleEnumerator))]
        [ImportService(typeof(ILogProvider))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IEmulatedPortStore))]
        [ImportService(typeof(IDeviceEnumerator))]
        [ImportService(typeof(IStoneProvider))]
        [ImportService(typeof(IControllerElementMappingProfileStore))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var modules = serviceContainer.Get<IModuleEnumerator>();
            var logProvider = serviceContainer.Get<ILogProvider>();

            var portsStore = serviceContainer.Get<IEmulatedPortStore>();
            var deviceEnumerator = serviceContainer.Get<IDeviceEnumerator>();
            var stoneProvider = serviceContainer.Get<IStoneProvider>();
            var mappingsStore = serviceContainer.Get<IControllerElementMappingProfileStore>();

            var loader = new EmulatorExecutableProvider(logProvider.GetLogger("EmulatorExecutableLoader"), modules);
            register.RegisterService<IEmulatorExecutableProvider>(loader);

            register.RegisterService<IEmulatedPortsManager>(new EmulatedPortsManager(portsStore, deviceEnumerator,
                mappingsStore, stoneProvider));
        }
    }
}
