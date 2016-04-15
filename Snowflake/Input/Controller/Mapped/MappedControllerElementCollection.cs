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
            var mappedElements = (from element in virtualDevice.Layout
                                  where realDevice.Layout[element.Key] != null
                                  select new MappedControllerElement(element.Key) { DeviceElement = element.Key }
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
