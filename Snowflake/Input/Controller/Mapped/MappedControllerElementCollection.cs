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
        private static readonly IDictionary<ControllerElement, KeyboardKey> DefaultKeyboardMappings =
            new Dictionary<ControllerElement, KeyboardKey>()
            {
                {ControllerElement.Button0, KeyboardKey.Key0},
                {ControllerElement.Button1, KeyboardKey.Key1},
                {ControllerElement.Button2, KeyboardKey.Key2},
                {ControllerElement.Button3, KeyboardKey.Key3},
                {ControllerElement.Button4, KeyboardKey.Key4},
                {ControllerElement.Button5, KeyboardKey.Key5},
                {ControllerElement.Button6, KeyboardKey.Key6},
                {ControllerElement.Button7, KeyboardKey.Key7},
                {ControllerElement.Button8, KeyboardKey.Key8},
                {ControllerElement.Button9, KeyboardKey.Key9},
                {ControllerElement.ButtonStart, KeyboardKey.KeySpacebar},
                {ControllerElement.ButtonSelect, KeyboardKey.KeyEnter},
                {ControllerElement.ButtonA, KeyboardKey.KeyZ},
                {ControllerElement.ButtonB, KeyboardKey.KeyX},
                {ControllerElement.ButtonX, KeyboardKey.KeyC},
                {ControllerElement.ButtonY, KeyboardKey.KeyV},
                {ControllerElement.ButtonC, KeyboardKey.KeyC},
                {ControllerElement.ButtonL, KeyboardKey.KeyQ},
                {ControllerElement.ButtonR, KeyboardKey.KeyE},
                {ControllerElement.DirectionalN, KeyboardKey.KeyUp},
                {ControllerElement.DirectionalE, KeyboardKey.KeyRight},
                {ControllerElement.DirectionalS, KeyboardKey.KeyDown},
                {ControllerElement.DirectionalW, KeyboardKey.KeyLeft},
                {ControllerElement.AxisLeftAnalogPositiveY, KeyboardKey.KeyW},
                {ControllerElement.AxisLeftAnalogNegativeX, KeyboardKey.KeyA},
                {ControllerElement.AxisLeftAnalogNegativeY, KeyboardKey.KeyS},
                {ControllerElement.AxisLeftAnalogPositiveX, KeyboardKey.KeyD},
                {ControllerElement.AxisRightAnalogPositiveY, KeyboardKey.KeyI},
                {ControllerElement.AxisRightAnalogNegativeX, KeyboardKey.KeyJ},
                {ControllerElement.AxisRightAnalogNegativeY, KeyboardKey.KeyK},
                {ControllerElement.AxisRightAnalogPositiveX, KeyboardKey.KeyL}
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
                                     DeviceKeyboardKey =
                                   MappedControllerElementCollection.DefaultKeyboardMappings.ContainsKey(element.Key)
                                 ? MappedControllerElementCollection.DefaultKeyboardMappings[element.Key]
                                 : KeyboardKey.KeyNone,
                                     DeviceElement =
                                       element.Key != ControllerElement.Pointer2D ? ControllerElement.Keyboard
                                       : ControllerElement.Pointer2D
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
