using GraphQL.Types;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.InputDevice
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
