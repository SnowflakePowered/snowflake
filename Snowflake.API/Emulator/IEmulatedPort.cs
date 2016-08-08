using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Emulator
{
    public interface IEmulatedPort
    {
        int EmulatedPortNumber { get; }
        IInputDevice PluggedDevice { get; }
        IControllerLayout EmulatedController { get; }
        IMappedControllerElementCollection MappedElementCollection { get; }
    }
}