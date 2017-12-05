using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.MappedControllerElement
{
    public class MappedControllerElementInputObject
    {
        public ControllerElement LayoutElement { get; set; }
        public ControllerElement DeviceElement { get; set; }
    }
}
