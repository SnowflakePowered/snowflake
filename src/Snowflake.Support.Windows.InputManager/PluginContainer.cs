using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Plugin.InputManager.Win32
{
    public class InputManagerContainer : IComposer
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IServiceContainer coreInstance)
        {
            coreInstance.Get<IServiceRegistrationProvider>()
                .RegisterService<IInputManager>(new InputManager());
        }
    }
}
