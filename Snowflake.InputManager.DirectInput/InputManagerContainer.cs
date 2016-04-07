using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Service;

namespace Snowflake.InputManager
{
    public class InputManagerContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            coreInstance.RegisterService<IInputManager>(new InputManager());
        }
    }
}
