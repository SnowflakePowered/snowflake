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
        public ControllerId ControllerID { get; }

        public InputDriver DriverType { get; }

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

        /// <summary>
        /// Initializes a <see cref="ControllerElementMappings"/> from an <see cref="IDeviceLayoutMapping"/>,
        /// that includes all mappings from the default layout.
        /// </summary>
        /// <param name="deviceName">The name of the physical device for this set of mappings.</param>
        /// <param name="controllerId">The Stone <see cref="ControllerID"/> this mapping is intended for.</param>
        /// <param name="driver">The <see cref="InputDriver"/> of the device instance for this set of mappings.</param>
        /// <param name="vendor">The vendor ID of the physical device for this set of mappings.</param>
        /// <param name="mapping">The device layout mapping provided by the device enumerator.</param>
        public ControllerElementMappings(string deviceName,
            ControllerId controllerId, InputDriver driver, int vendor,
            IDeviceLayoutMapping mapping)
            : this(deviceName, controllerId, driver, vendor)
        {
            foreach (var controllerElement in mapping)
            {
                this.Add(controllerElement);
            }
        }

        /// <summary>
        /// Initializes a<see cref= "ControllerElementMappings" /> from an <see cref="IDeviceLayoutMapping"/>,
        /// that includes only mappings that are assignable to the provided layout.
        /// </summary>
        /// <param name="deviceName">The name of the physical device for this set of mappings.</param>
        /// <param name="controller">The controller layout to assign device mappings to.</param>
        /// <param name="driver">The <see cref="InputDriver"/> of the device instance for this set of mappings.</param>
        /// <param name="vendor">The vendor ID of the physical device for this set of mappings.</param>
        /// <param name="mapping">The device layout mapping provided by the device enumerator.</param>
        public ControllerElementMappings(string deviceName,
           IControllerLayout controller, InputDriver driver, int vendor,
           IDeviceLayoutMapping mapping)
           : this(deviceName, controller.LayoutID, driver, vendor)
        {
            foreach (var (controllerElement, _) in controller.Layout)
            {
                if (mapping[controllerElement] == DeviceCapability.None) continue;
                this.Add(new MappedControllerElement(controllerElement, mapping[controllerElement]));
            }
        }

        public ControllerElementMappings(string deviceName,
            ControllerId controllerId, InputDriver driver, int vendor)
        {
            this.DeviceName = deviceName;
            this.ControllerID = controllerId;
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
