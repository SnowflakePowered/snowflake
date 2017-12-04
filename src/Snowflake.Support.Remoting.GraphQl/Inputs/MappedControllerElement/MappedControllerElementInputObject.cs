using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Inputs.MappedControllerElement
{
    public class MappedControllerElementInputObject
    {
        public ControllerElement LayoutElement { get; set; }
        public ControllerElement DeviceElement { get; set; }
    }
}
