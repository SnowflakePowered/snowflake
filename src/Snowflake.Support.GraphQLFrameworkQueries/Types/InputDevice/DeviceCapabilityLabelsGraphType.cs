using GraphQL.Types;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice
{
    public class DeviceCapabilityLabelsGraphType : ObjectGraphType<IDeviceCapabilityLabels>
    {
        public DeviceCapabilityLabelsGraphType()
        {
            Name = "DeviceCapabilityLabels";
            Description = "A mapping of device capabilities to friendly string labels.";
            Field<ListGraphType<DeviceCapabilityLabelKeyValuePairGraphType>>("labels", description: "A list of labels in this device instance",
                resolve: c => c.Source);
            foreach (var (key, _) in DefaultDeviceCapabilityLabels.DefaultLabels)
            {
                Field<StringGraphType>(key.ToString(), resolve: c => c.Source[key] ?? key.ToString());
            }
        }
    }
}
