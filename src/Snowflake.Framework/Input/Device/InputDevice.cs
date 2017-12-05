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
        /// <inheritdoc/>
        public string DeviceId { get; }

        /// <inheritdoc/>
        public ILowLevelInputDevice DeviceInfo { get; }

        /// <inheritdoc/>
        public int? DeviceIndex { get; set; }

        /// <inheritdoc/>
        public string ControllerName { get; }

        /// <inheritdoc/>
        public InputApi DeviceApi { get; }

        /// <inheritdoc/>
        public IControllerLayout DeviceLayout { get; }

        public InputDevice(InputApi deviceApi, ILowLevelInputDevice deviceInfo, IControllerLayout deviceLayout)
        {
            this.DeviceId = deviceLayout.LayoutID;
            this.ControllerName = deviceLayout.FriendlyName;
            this.DeviceApi = deviceApi;
            this.DeviceInfo = deviceInfo;
            this.DeviceLayout = deviceLayout;
        }
    }
}
