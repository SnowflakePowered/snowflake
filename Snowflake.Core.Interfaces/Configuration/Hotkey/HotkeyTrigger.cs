using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Hotkey
{
  
    /// <summary>
    /// Represents the trigger for a hotkey
    /// </summary>
    public struct HotkeyTrigger 
    {
        /// <summary>
        /// The trigger key on a controller
        /// </summary>
        public ControllerElement ControllerTrigger { get; }
        /// <summary>
        /// The trigger key on a keyboard
        /// </summary>
        public KeyboardKey KeyboardTrigger { get; }

        /// <summary>
        /// Instantiates a HotkeyTrigger with a controller element and
        /// no keyboard key.
        /// </summary>
        /// <param name="controllerElement">The controller element</param>
        public HotkeyTrigger(ControllerElement controllerElement)
        {
            this.ControllerTrigger = controllerElement;
            this.KeyboardTrigger = KeyboardKey.KeyNone;
        }

        /// <summary>
        /// Instantiates a HotkeyTrigger with a keyboard key and
        /// no controller element.
        /// </summary>
        /// <param name="keyboardKey">The keyboard trigger</param>
        public HotkeyTrigger(KeyboardKey keyboardKey)
        {
            this.ControllerTrigger = ControllerElement.NoElement;
            this.KeyboardTrigger = keyboardKey;
        }

        /// <summary>
        /// Instantiates a HotkeyTrigger
        /// </summary>
        /// <param name="keyboardKey">The keyboard trigger</param>
        /// <param name="controllerElement">The controller trigger</param>
        public HotkeyTrigger(KeyboardKey keyboardKey, ControllerElement controllerElement)
        {
            this.KeyboardTrigger = keyboardKey;
            this.ControllerTrigger = controllerElement;
        }

    }
}
