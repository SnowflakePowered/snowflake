using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Controller
{
    public class GamepadAbstraction : IGamepadAbstraction
    {
        public string DeviceName { get; private set; }

        public GamepadAbstraction(string deviceName)
        {
            this.DeviceName = deviceName;
        }
        public string  L1 { get; set; }
        public string  L2 { get; set; }
        public string  L3 { get; set; }
        public string  R1 { get; set; }
        public string  R2 { get; set; }
        public string  R3 { get; set; }

        public string  DpadUp { get; set; }
        public string  DpadDown { get; set; }
        public string  DpadLeft { get; set; }
        public string  DpadRight { get; set; }

        public string  RightAnalogXLeft { get; set; }
        public string  RightAnalogXRight { get; set; }
        public string  RightAnalogYUp { get; set; }
        public string  RightAnalogYDown { get; set; }

        public string  LeftAnalogXLeft { get; set; }
        public string  LeftAnalogXRight { get; set; }
        public string  LeftAnalogYUp { get; set; }
        public string  LeftAnalogYDown { get; set; }

        public string  Select { get; set; }
        public string  Start { get; set; }
    }
}
