using HotChocolate.Types;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Device
{
    public sealed class DeviceCapabilityLabelElementType
       : ObjectType<KeyValuePair<DeviceCapability, string>>
    {
        protected override void Configure(IObjectTypeDescriptor<KeyValuePair<DeviceCapability, string>> descriptor)
        {
            descriptor.Name("DeviceCapabilityElement")
                .Description("A single mapping of device capability to friendly string label.");

            descriptor.Field(context => context.Key)
                .Name("capability")
                .Description("The capability described by the label.")
                .Type<DeviceCapabilityEnum>();

            descriptor.Field(context => context.Value)
                .Name("label")
                .Description("The label that describes the capability.")
                .Type<StringType>();
        }
    }
}
