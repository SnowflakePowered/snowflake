using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Controller
{
    public class ControllerInput : IControllerInput
    {
        public bool IsAnalog { get; private set; }
        public string InputName { get; private set; }
        public string GamepadAbstraction { get; private set; }

        public ControllerInput(string inputName, string gamepadAbstraction, bool isAnalog = false)
        {
            this.InputName = inputName;
            this.IsAnalog = isAnalog;
            this.GamepadAbstraction = gamepadAbstraction;
        }
    }
}
