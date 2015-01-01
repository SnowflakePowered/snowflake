using System;
using System.Collections.Generic;
namespace Snowflake.Emulator.Input
{
    public interface IControllerTemplate
    {
        string ControllerID { get; }
        string EmulatorID { get; }
        IReadOnlyDictionary<string, IControllerMapping> GamepadControllerMappings { get; }
        IReadOnlyDictionary<string, IControllerMapping> KeyboardControllerMappings { get; }
        string PlatformID { get; }
    }
}
