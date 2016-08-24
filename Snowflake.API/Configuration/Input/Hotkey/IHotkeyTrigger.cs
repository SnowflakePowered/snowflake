using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input.Hotkey
{
    public interface IHotkeyTrigger
    {
        /// <summary>
        /// The controller trigger for this hotkey
        /// </summary>
        ControllerElement ControllerTrigger { get; set; }

        /// <summary>
        /// The keyboard key trigger
        /// </summary>
        KeyboardKey KeyboardTrigger { get; set; }
    }
}
