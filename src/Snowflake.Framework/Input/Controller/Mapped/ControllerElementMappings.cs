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
            return this.controllerElements.GetEnumerator();
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
                return this.controllerElements.FirstOrDefault(l => l.LayoutElement == layoutElement)
                           .DeviceCapability;
            }
            set
            {
                var mappedElement = this.controllerElements.FirstOrDefault(l => l.LayoutElement == layoutElement);
                mappedElement.DeviceCapability = value;
            }
        }

        private readonly List<MappedControllerElement> controllerElements;

        public ControllerElementMappings(string deviceName,
            string controllerId, InputDriverType driver, int vendor)
        {
            this.DeviceName = deviceName;
            this.ControllerId = controllerId;
            this.DriverType = driver;
            this.VendorID = vendor;
            this.controllerElements = new List<MappedControllerElement>();
        }

        public void Add(MappedControllerElement controllerElement)
        {
            this.controllerElements.Add(controllerElement);
        }

        ///// <summary>
        ///// Gets default mappings for a real device to a virtual device
        ///// </summary>
        ///// <param name="realDevice">The button layout of the real controller device</param>
        ///// <param name="virtualDevice">The button layout of the defined controller device</param>
        ///// <returns></returns>
        //public static IControllerElementMappings GetDefaultMappings(IControllerLayout realDevice,
        //    IControllerLayout virtualDevice)
        //{
        //    // todo
        //    return new ControllerElementMappings("", "");
        //    //return realDevice.Layout.Keyboard == null
        //    //    ? ControllerElementMappings.GetDefaultDeviceMappings(realDevice, virtualDevice)
        //    //    : ControllerElementMappings.GetDefaultKeyboardMappings(realDevice, virtualDevice);
        //}

        //private static IControllerElementMappings GetDefaultKeyboardMappings(IControllerLayout realKeyboard,
        //    IControllerLayout virtualDevice)
        //{
        //    var mappedElements = from element in virtualDevice.Layout
        //        select new MappedControllerElement(element.Key,
        //            ControllerElementMappings.DefaultKeyboardMappings.ContainsKey(element.Key)
        //                ? ControllerElementMappings.DefaultKeyboardMappings[element.Key]
        //                : ControllerElement.KeyNone);
        //    var elementCollection = new ControllerElementMappings(realKeyboard.LayoutId, virtualDevice.LayoutId);
        //    foreach (var element in mappedElements)
        //    {
        //        elementCollection.Add(element);
        //    }

        //    return elementCollection;
        //}

        //private static IControllerElementMappings GetDefaultDeviceMappings(IControllerLayout realDevice,
        //    IControllerLayout virtualDevice)
        //{
        //    var mappedElements = from element in virtualDevice.Layout
        //        select new MappedControllerElement(element.Key,
        //            realDevice.Layout[element.Key] != null ? element.Key : ControllerElement.NoElement);

        //    var elementCollection = new ControllerElementMappings(realDevice.LayoutId, virtualDevice.LayoutId);

        //    foreach (var element in mappedElements)
        //    {
        //        elementCollection.Add(element);
        //    }

        //    return elementCollection;
        //}
    }
}
