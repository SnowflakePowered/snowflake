using HotChocolate.Types;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Device
{
    public sealed class DeviceCapabilityLabelsType
        : ObjectType<IDeviceCapabilityLabels>
    {
        protected override void Configure(IObjectTypeDescriptor<IDeviceCapabilityLabels> descriptor)
        {
            descriptor.Name("DeviceCapabilityLabels")
                .Description("A mapping of device capabilities to friendly string labels.");
            descriptor.Field("mappings")
                .Resolve(c => c.Parent<IDeviceCapabilityLabels>())
                .Description("The list of mappings from label to capability for this device instance.")
                .Type<NonNullType<ListType<NonNullType<DeviceCapabilityLabelElementType>>>>();
            foreach (var (key, _) in DefaultDeviceCapabilityLabels.DefaultLabels)
            {
                descriptor.Field(key.ToString())
                    .Resolve(c => c.Parent<IDeviceCapabilityLabels>()[key] ?? key.ToString())
                    .Type<NonNullType<StringType>>();
            }
        }
    }
}
