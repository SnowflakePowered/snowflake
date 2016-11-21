using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Device;

namespace Snowflake.Input.Controller.Mapped
{
    public class MappedControllerElementCollection : IMappedControllerElementCollection
    {
        private static readonly IDictionary<ControllerElement, ControllerElement> DefaultKeyboardMappings =
            new Dictionary<ControllerElement, ControllerElement>()
            {
                {ControllerElement.Button0, ControllerElement.Key0},
                {ControllerElement.Button1, ControllerElement.Key1},
                {ControllerElement.Button2, ControllerElement.Key2},
                {ControllerElement.Button3, ControllerElement.Key3},
                {ControllerElement.Button4, ControllerElement.Key4},
                {ControllerElement.Button5, ControllerElement.Key5},
                {ControllerElement.Button6, ControllerElement.Key6},
                {ControllerElement.Button7, ControllerElement.Key7},
                {ControllerElement.Button8, ControllerElement.Key8},
                {ControllerElement.Button9, ControllerElement.Key9},
                {ControllerElement.ButtonStart, ControllerElement.KeySpacebar},
                {ControllerElement.ButtonSelect, ControllerElement.KeyEnter},
                {ControllerElement.ButtonA, ControllerElement.KeyZ},
                {ControllerElement.ButtonB, ControllerElement.KeyX},
                {ControllerElement.ButtonX, ControllerElement.KeyC},
                {ControllerElement.ButtonY, ControllerElement.KeyV},
                {ControllerElement.ButtonC, ControllerElement.KeyC},
                {ControllerElement.ButtonL, ControllerElement.KeyQ},
                {ControllerElement.ButtonR, ControllerElement.KeyE},
                {ControllerElement.DirectionalN, ControllerElement.KeyUp},
                {ControllerElement.DirectionalE, ControllerElement.KeyRight},
                {ControllerElement.DirectionalS, ControllerElement.KeyDown},
                {ControllerElement.DirectionalW, ControllerElement.KeyLeft},
                {ControllerElement.AxisLeftAnalogPositiveY, ControllerElement.KeyW},
                {ControllerElement.AxisLeftAnalogNegativeX, ControllerElement.KeyA},
                {ControllerElement.AxisLeftAnalogNegativeY, ControllerElement.KeyS},
                {ControllerElement.AxisLeftAnalogPositiveX, ControllerElement.KeyD},
                {ControllerElement.AxisRightAnalogPositiveY, ControllerElement.KeyI},
                {ControllerElement.AxisRightAnalogNegativeX, ControllerElement.KeyJ},
                {ControllerElement.AxisRightAnalogNegativeY, ControllerElement.KeyK},
                {ControllerElement.AxisRightAnalogPositiveX, ControllerElement.KeyL}
            };

        public IEnumerator<IMappedControllerElement> GetEnumerator()
        {
            return this.controllerElements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public string DeviceId { get; }
        public string ControllerId { get; }

        private readonly List<IMappedControllerElement> controllerElements;

        public MappedControllerElementCollection(string deviceId, string controllerId)
        {
            this.DeviceId = deviceId;
            this.ControllerId = controllerId;
            this.controllerElements = new List<IMappedControllerElement>();
        }

        public void Add(IMappedControllerElement controllerElement)
        {
            this.controllerElements.Add(controllerElement);
        }

        /// <summary>
        /// Gets default mappings for a real device to a virtual device
        /// </summary>
        /// <param name="realDevice">The button layout of the real controller device</param>
        /// <param name="virtualDevice">The button layout of the defined controller device</param>
        /// <returns></returns>
        public static IMappedControllerElementCollection GetDefaultMappings(IControllerLayout realDevice, IControllerLayout virtualDevice)
        {
            return realDevice.Layout.Keyboard == null
                ? MappedControllerElementCollection.GetDefaultDeviceMappings(realDevice, virtualDevice)
                : MappedControllerElementCollection.GetDefaultKeyboardMappings(realDevice, virtualDevice);
        }

        private static IMappedControllerElementCollection GetDefaultKeyboardMappings(IControllerLayout realKeyboard, IControllerLayout virtualDevice)
        {
            var mappedElements = from element in virtualDevice.Layout
                                 select new MappedControllerElement(element.Key)
                                 {
                                     DeviceElement = MappedControllerElementCollection.DefaultKeyboardMappings.ContainsKey(element.Key) 
                                     ? MappedControllerElementCollection.DefaultKeyboardMappings[element.Key]
                                 : ControllerElement.KeyNone
                                 };
            var elementCollection = new MappedControllerElementCollection(realKeyboard.LayoutID, virtualDevice.LayoutID);
            foreach (var element in mappedElements)
            {
                elementCollection.Add(element);
            }

            return elementCollection;
        }

        private static IMappedControllerElementCollection GetDefaultDeviceMappings(IControllerLayout realDevice, IControllerLayout virtualDevice)
        {
            var mappedElements = (from element in virtualDevice.Layout
                                  select new MappedControllerElement(element.Key)
                                  {
                                      DeviceElement = realDevice.Layout[element.Key] != null ? element.Key : ControllerElement.NoElement
                                  }
                );
            var elementCollection = new MappedControllerElementCollection(realDevice.LayoutID, virtualDevice.LayoutID);
            foreach (var element in mappedElements)
            {
                elementCollection.Add(element);
            }

            return elementCollection;
        }

    }
}
