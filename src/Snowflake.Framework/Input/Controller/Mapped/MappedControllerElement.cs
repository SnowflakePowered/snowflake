using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller.Mapped
{
    public struct MappedControllerElement : IMappedControllerElement
    {
        /// <inheritdoc/>
        public ControllerElement LayoutElement { get; }

        /// <inheritdoc/>
        public ControllerElement DeviceElement { get; set; }

        public MappedControllerElement(ControllerElement virtualElement, ControllerElement deviceElement)
        {
            this.LayoutElement = virtualElement;
            this.DeviceElement = deviceElement;
        }
    }
}
