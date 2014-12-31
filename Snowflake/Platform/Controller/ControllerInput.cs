using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Platform.Controller
{
    public class ControllerInput
    {
        public bool IsAnalog { get; private set; }
        public string InputName { get; private set; }
        public string KeyboardDefault { get; private set; }
        public string GamepadDefault { get; private set; }

        public ControllerInput(string inputName, string keyboardDefault, string gamepadDefault, bool isAnalog = false)
        {
            this.InputName = inputName;
            this.IsAnalog = isAnalog;
            this.KeyboardDefault = keyboardDefault;
            this.GamepadDefault = gamepadDefault;
        }
    }
}
