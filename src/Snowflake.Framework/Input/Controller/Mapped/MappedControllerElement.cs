using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller.Mapped
{
    public class MappedControllerElement : IMappedControllerElement
    {
        public ControllerElement LayoutElement { get; }
        public ControllerElement DeviceElement { get; set; }

        public MappedControllerElement(ControllerElement virtualElement)
        {
            this.LayoutElement = virtualElement;
        }
    }
}
