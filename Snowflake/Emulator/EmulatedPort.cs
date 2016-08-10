using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Service;

namespace Snowflake.Emulator
{
    public class EmulatedPort : IEmulatedPort
    {
        public int EmulatedPortNumber { get; }
        public IInputDevice PluggedDevice { get; }
        public IControllerLayout EmulatedController { get; }
        public IMappedControllerElementCollection MappedElementCollection { get; }

        public EmulatedPort(int emulatedPortNumber, string emulatedLayoutId, string pluggedDeviceLayout, int pluggedDeviceIndex, 
            IEnumerable<IInputEnumerator> inputEnumerators, IMappedControllerElementCollectionStore store, IStoneProvider stoneProvider, string profileName = "default")
        {
            var inputEnumerator = (from enumerator in inputEnumerators
                where enumerator.ControllerLayout.LayoutID == pluggedDeviceLayout
                select enumerator).FirstOrDefault();
            if (inputEnumerator == null) throw new InvalidOperationException("Input layout not found.");

            var pluggedDevice = (from device in inputEnumerator.GetConnectedDevices()
                where device.DeviceIndex == pluggedDeviceIndex
                select device).FirstOrDefault();
            if(pluggedDevice == null)
                throw new InvalidOperationException("The specified input device does not exist");
            this.PluggedDevice = pluggedDevice;
            
            this.EmulatedController = stoneProvider.Controllers[emulatedLayoutId];
            this.EmulatedPortNumber = emulatedPortNumber;

            this.MappedElementCollection = store.GetMappingProfile(emulatedLayoutId, pluggedDevice.DeviceId, profileName);
        }
    }
}
