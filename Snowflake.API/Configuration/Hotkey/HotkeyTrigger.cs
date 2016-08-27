using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Hotkey
{
  
    public struct HotkeyTrigger 
    {
        public ControllerElement ControllerTrigger { get; }
        public KeyboardKey KeyboardTrigger { get; }

        public HotkeyTrigger(ControllerElement controllerElement)
        {
            this.ControllerTrigger = controllerElement;
            this.KeyboardTrigger = KeyboardKey.KeyNone;
        }

        public HotkeyTrigger(KeyboardKey keyboardKey)
        {
            this.ControllerTrigger = ControllerElement.NoElement;
            this.KeyboardTrigger = keyboardKey;
        }

        public HotkeyTrigger(KeyboardKey keyboardKey, ControllerElement controllerElement)
        {
            this.KeyboardTrigger = keyboardKey;
            this.ControllerTrigger = controllerElement;
        }

    }
}
