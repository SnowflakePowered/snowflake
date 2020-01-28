using GraphQL.Types;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice
{
    public class DeviceCapabilityEnum : EnumerationGraphType<DeviceCapability>
    {
        public DeviceCapabilityEnum()
        {
            Name = "DeviceCapability";
            Description = "Capabilities offered by the input device instance";
        }
    }
}
