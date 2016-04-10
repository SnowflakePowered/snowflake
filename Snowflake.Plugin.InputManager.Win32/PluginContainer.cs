using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Service;

namespace Snowflake.Plugin.InputManager.Win32
{
    public class InputManagerContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            coreInstance.RegisterService<IInputManager>(new InputManager());
        }
    }
}
