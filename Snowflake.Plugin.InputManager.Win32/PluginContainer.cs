using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Services;

namespace Snowflake.Plugin.InputManager.Win32
{
    [ContainerLoadPriority(ContainerLoadPriority.Service)]
    public class InputManagerContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            coreInstance.RegisterService<IInputManager>(new InputManager());
        }
    }
}
