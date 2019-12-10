using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Support.InputEnumerators.Windows;

namespace Snowflake.Plugin.InputManager.Win32
{
    public class InputManagerContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, IServiceRepository coreInstance)
        {
            coreInstance.Get<IServiceRegistrationProvider>()
                .RegisterService<IDeviceEnumerator>(new WindowsDeviceEnumerator());
        }
    }
}
