using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Input.Mapping;

namespace Snowflake.Emulator.Input
{
    public interface IInputConfiguration
    {
        KeyboardMapping keyboardMapping { get; }
        GamepadMapping gamepadMapping { get; }
    }
}
