using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Device;

namespace Snowflake.Support.Remoting.GraphQL.Inputs.InputDevice
{
    public class InputDeviceInputObject
    {
        public string DeviceId { get; set; }
        public int? DeviceIndex { get; set; }
        public InputApi DeviceApi { get; set; }
    }
}
