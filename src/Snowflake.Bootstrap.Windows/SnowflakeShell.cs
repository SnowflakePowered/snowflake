using System;
using System.Diagnostics;
using System.Linq;
using Snowflake.Bootstrap.Windows.Utility;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Process;
using Snowflake.Services;
using Snowflake.Services.AssemblyLoader;

namespace Snowflake.Shell.Windows
{
    internal class SnowflakeShell
    {
        private readonly string appDataDirectory =
            PathUtility.GetApplicationDataPath().CreateSubdirectory("snowflake").FullName;

        private IServiceContainer loadedCore;

        internal SnowflakeShell()
        {
            this.StartCore();
        }

        public void StartCore()
        {
            this.loadedCore = new ServiceContainer(this.appDataDirectory);
            var loader = this.loadedCore.Get<IModuleEnumerator>();
            var composer = new AssemblyComposer(this.loadedCore, loader);
            composer.Compose();
            this.RunTestHook();
        }


        private void RunTestHook()
        {
            var devices = this.loadedCore.Get<IDeviceEnumerator>();
            var keyboard = devices.QueryConnectedDevices().First(d => d.InstanceGuid == IDeviceEnumerator.KeyboardInstanceGuid);
            var kbdInstance = keyboard.Instances.First();
            var snesLayout = this.loadedCore.Get<IStoneProvider>().Controllers["SNES_CONTROLLER"];
            var mapcol = new ControllerElementMappings(keyboard.DeviceName,
                           "SNES_CONTROLLER",
                           kbdInstance.Driver,
                           keyboard.VendorID,
                           kbdInstance.DefaultLayout);
            var controller = new EmulatedController(0, keyboard, kbdInstance, snesLayout, mapcol);
            //todo need persistence of real to emulated controllers!!
        }

        public void RestartCore()
        {
            this.ShutdownCore();
            this.StartCore();
        }

        public void ShutdownCore()
        {
            this.loadedCore.Dispose();
            GC.WaitForPendingFinalizers();
        }
    }
}
