using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Device;

namespace Snowflake.Input.Controller
{
    public class ControllerElementMappingProfile : IControllerElementMappingProfile
    {
        /// <inheritdoc/>
        public IEnumerator<ControllerElementMapping> GetEnumerator()
        {
            return controllerElements.Values.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public string DeviceName { get; }

        /// <inheritdoc/>
        public ControllerId ControllerID { get; }

        /// <inheritdoc />
        public InputDriver DriverType { get; }

        /// <inheritdoc />
        public int VendorID { get; }

        /// <inheritdoc />
        public Guid ProfileGuid { get; set; }

        public DeviceCapability this[ControllerElement layoutElement]
        {
            get
            {
                controllerElements.TryGetValue(layoutElement, out ControllerElementMapping map);
                return map.DeviceCapability;
            }
            set
            {
                controllerElements[layoutElement] = new ControllerElementMapping(layoutElement, value);
            }
        }

        private readonly Dictionary<ControllerElement, ControllerElementMapping> controllerElements;

        /// <summary>
        /// Initializes a <see cref="ControllerElementMappingProfile"/> from an <see cref="IDeviceLayoutMapping"/>,
        /// that includes all mappings from the default layout.
        /// </summary>
        /// <param name="deviceName">The name of the physical device for this set of mappings.</param>
        /// <param name="controllerId">The Stone <see cref="ControllerID"/> this mapping is intended for.</param>
        /// <param name="driver">The <see cref="InputDriver"/> of the device instance for this set of mappings.</param>
        /// <param name="vendor">The vendor ID of the physical device for this set of mappings.</param>
        /// <param name="mapping">The device layout mapping provided by the device enumerator.</param>
        /// <param name="profileGuid">The <see cref="Guid"/> of this mapping profile.</param>
        public ControllerElementMappingProfile(string deviceName,
            ControllerId controllerId, InputDriver driver, int vendor,
            IDeviceLayoutMapping mapping)
            : this(deviceName, controllerId, driver, vendor, mapping, Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a <see cref="ControllerElementMappingProfile"/> from an <see cref="IDeviceLayoutMapping"/>,
        /// that includes all mappings from the default layout.
        /// </summary>
        /// <param name="deviceName">The name of the physical device for this set of mappings.</param>
        /// <param name="controllerId">The Stone <see cref="ControllerID"/> this mapping is intended for.</param>
        /// <param name="driver">The <see cref="InputDriver"/> of the device instance for this set of mappings.</param>
        /// <param name="vendor">The vendor ID of the physical device for this set of mappings.</param>
        /// <param name="mapping">The device layout mapping provided by the device enumerator.</param>
        /// <param name="profileGuid">The <see cref="Guid"/> of this mapping profile.</param>
        public ControllerElementMappingProfile(string deviceName,
            ControllerId controllerId, InputDriver driver, int vendor,
            IDeviceLayoutMapping mapping, Guid profileGuid)
            : this(deviceName, controllerId, driver, vendor, profileGuid)
        {
            foreach (var controllerElement in mapping)
            {
                Add(controllerElement);
            }
        }

        /// <summary>
        /// Initializes a<see cref= "ControllerElementMappingProfile" /> from an <see cref="IDeviceLayoutMapping"/>,
        /// that includes only mappings that are assignable to the provided layout.
        /// </summary>
        /// <param name="deviceName">The name of the physical device for this set of mappings.</param>
        /// <param name="controller">The controller layout to assign device mappings to.</param>
        /// <param name="driver">The <see cref="InputDriver"/> of the device instance for this set of mappings.</param>
        /// <param name="vendor">The vendor ID of the physical device for this set of mappings.</param>
        /// <param name="mapping">The device layout mapping provided by the device enumerator.</param>
        public ControllerElementMappingProfile(string deviceName,
           IControllerLayout controller, InputDriver driver, int vendor,
           IDeviceLayoutMapping mapping)
           : this(deviceName, controller, driver, vendor, mapping, Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a<see cref= "ControllerElementMappingProfile" /> from an <see cref="IDeviceLayoutMapping"/>,
        /// that includes only mappings that are assignable to the provided layout.
        /// </summary>
        /// <param name="deviceName">The name of the physical device for this set of mappings.</param>
        /// <param name="controller">The controller layout to assign device mappings to.</param>
        /// <param name="driver">The <see cref="InputDriver"/> of the device instance for this set of mappings.</param>
        /// <param name="vendor">The vendor ID of the physical device for this set of mappings.</param>
        /// <param name="mapping">The device layout mapping provided by the device enumerator.</param>
        /// <param name="profileGuid">The <see cref="Guid"/> of this mapping profile.</param>
        public ControllerElementMappingProfile(string deviceName,
           IControllerLayout controller, InputDriver driver, int vendor,
           IDeviceLayoutMapping mapping, Guid profileGuid)
           : this(deviceName, controller.ControllerID, driver, vendor, profileGuid)
        {
            foreach (var (controllerElement, _) in controller.Layout)
            {
                if (mapping[controllerElement] == DeviceCapability.None) continue;
                Add(new ControllerElementMapping(controllerElement, mapping[controllerElement]));
            }
        }

        public ControllerElementMappingProfile(string deviceName,
            ControllerId controllerId, InputDriver driver, int vendor, Guid profileGuid)
        {
            DeviceName = deviceName;
            ControllerID = controllerId;
            DriverType = driver;
            VendorID = vendor;
            controllerElements = new Dictionary<ControllerElement, ControllerElementMapping>();
            ProfileGuid = profileGuid;
        }

        public void Add(ControllerElementMapping controllerElement)
        {
            controllerElements[controllerElement.LayoutElement] = controllerElement;
        }
    }
}
