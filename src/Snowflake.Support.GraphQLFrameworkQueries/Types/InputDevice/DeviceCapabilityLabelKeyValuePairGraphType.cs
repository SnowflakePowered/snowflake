using GraphQL.Types;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice
{
    public class DeviceCapabilityLabelKeyValuePairGraphType : ObjectGraphType<KeyValuePair<DeviceCapability, string>>
    {
        public DeviceCapabilityLabelKeyValuePairGraphType()
        {
            Name = "DeviceCapabilityLabelKeyValuePair";
            Description = "A label of a device capability";
            Field<DeviceCapabilityEnum>("capability", description: "The capability this label describes.", resolve: c => c.Source.Key);
            Field<StringGraphType>("label", description: "The label of the capability.", resolve: c => c.Source.Value);
        }
    }
}
