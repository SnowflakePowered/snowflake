using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input.Hotkey;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Hotkey
{
    public struct HotkeyTrigger : IHotkeyTrigger
    {
        public ControllerElement ControllerTrigger { get; set; }
        public KeyboardKey KeyboardTrigger { get; set; }

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
    }
}
