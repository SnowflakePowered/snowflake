using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Mapping;

namespace Snowflake.Emulator.Configuration
{
    public interface IInputConfiguration
    {
        KeyboardMapping keyboardMapping { get; }
        GamepadMapping gamepadMapping { get; }
    }
}
