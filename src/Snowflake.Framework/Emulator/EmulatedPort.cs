using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Emulator
{
    public class EmulatedPort : IEmulatedPort
    {
        public int EmulatedPortNumber { get; }
        public IInputDevice PluggedDevice { get; }
        public IControllerLayout EmulatedController { get; }
        public IMappedControllerElementCollection MappedElementCollection { get; }

        public EmulatedPort(int emulatedPort, IControllerLayout emulatedController, IInputDevice pluggedDevice,
            IMappedControllerElementCollection mappedControllerElementCollection)
        {
            this.EmulatedPortNumber = emulatedPort;
            this.PluggedDevice = pluggedDevice;
            this.EmulatedController = emulatedController;
            this.MappedElementCollection = mappedControllerElementCollection;

        }
    }
}
