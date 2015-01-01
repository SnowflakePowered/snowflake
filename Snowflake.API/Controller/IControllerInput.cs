using System;
namespace Snowflake.Controller
{
    public interface IControllerInput
    {
        string GamepadDefault { get; }
        string InputName { get; }
        bool IsAnalog { get; }
        string KeyboardDefault { get; }
    }
}
