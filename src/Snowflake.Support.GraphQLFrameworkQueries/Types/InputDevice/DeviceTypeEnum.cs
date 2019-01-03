using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Device;

namespace Snowflake.Support.Remoting.GraphQL.Types.InputDevice
{
    public class DeviceTypeEnum : EnumerationGraphType<DeviceType>
    {
        public DeviceTypeEnum()
        {
            Name = "DeviceType";
            Description = "Input Device Types.";
        }
    }
}
