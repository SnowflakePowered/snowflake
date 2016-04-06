using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Input.Device
{
    public class InputDevice : IInputDevice
    {

        public string DeviceId { get; }
        public ILowLevelInputDevice DeviceInfo { get; }
        public int? DeviceIndex { get; set; }
        public string ControllerName { get; }
        public InputApi DeviceApi { get; }
        public IControllerLayout DeviceLayout { get; }

        public InputDevice(string deviceId, string controllerName, InputApi deviceApi, ILowLevelInputDevice deviceInfo, IControllerLayout deviceLayout)
        {
            this.DeviceId = deviceId;
            this.ControllerName = controllerName;
            this.DeviceApi = deviceApi;
            this.DeviceInfo = deviceInfo;
            this.DeviceLayout = deviceLayout;
        }


    }
}
