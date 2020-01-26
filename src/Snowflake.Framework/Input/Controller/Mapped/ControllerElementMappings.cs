using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Device;

namespace Snowflake.Input.Controller.Mapped
{
    public class ControllerElementMappings : IControllerElementMappings
    {
        /// <inheritdoc/>
        public IEnumerator<MappedControllerElement> GetEnumerator()
        {
            return this.controllerElements.Values.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public string DeviceName { get; }

        /// <inheritdoc/>
        public ControllerId ControllerId { get; }

        public InputDriverType DriverType { get; }

        public int VendorID { get; }

        public DeviceCapability this[ControllerElement layoutElement]
        {
            get
            {
                this.controllerElements.TryGetValue(layoutElement, out MappedControllerElement map);
                return map.DeviceCapability;
            }
            set
            {
                this.controllerElements[layoutElement] = new MappedControllerElement(layoutElement, value);
            }
        }

        private readonly Dictionary<ControllerElement, MappedControllerElement> controllerElements;

        public ControllerElementMappings(string deviceName,
            ControllerId controllerId, InputDriverType driver, int vendor,
            IDeviceLayoutMapping mapping)
            : this(deviceName, controllerId, driver, vendor)
        {
            foreach (var (key, value) in mapping)
            {
                this.Add(new MappedControllerElement(key, value));
            }
        }

        public ControllerElementMappings(string deviceName,
            ControllerId controllerId, InputDriverType driver, int vendor)
        {
            this.DeviceName = deviceName;
            this.ControllerId = controllerId;
            this.DriverType = driver;
            this.VendorID = vendor;
            this.controllerElements = new Dictionary<ControllerElement, MappedControllerElement>();
        }

        public void Add(MappedControllerElement controllerElement)
        {
            this.controllerElements[controllerElement.LayoutElement] = controllerElement;
        }
    }
}
