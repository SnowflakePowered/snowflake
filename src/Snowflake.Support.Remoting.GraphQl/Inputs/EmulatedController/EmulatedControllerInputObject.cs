using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Support.Remoting.GraphQL.Inputs.InputDevice;

namespace Snowflake.Support.Remoting.GraphQL.Inputs.EmulatedController
{
    public class EmulatedControllerInputObject
    {
        public InputDeviceInputObject InputDevice { get; set; }
        public int PortIndex { get; set; }
        public string TargetLayout { get; set; }
        public string ControllerProfile { get; set; }
    }
}
