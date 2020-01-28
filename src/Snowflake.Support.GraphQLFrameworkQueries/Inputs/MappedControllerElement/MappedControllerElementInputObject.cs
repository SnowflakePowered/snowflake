using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Support.GraphQLFrameworkQueries.Inputs.MappedControllerElement
{
    public class MappedControllerElementInputObject
    {
        public ControllerElement LayoutElement { get; set; }
        public DeviceCapability DeviceCapability { get; set; }
    }
}
