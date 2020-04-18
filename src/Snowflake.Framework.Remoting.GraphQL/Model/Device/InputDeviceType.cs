using System;
using System.Collections.Generic;
using System.Text;
using HotChocolate.Types;
using Snowflake.Input.Device;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Device
{
    public class InputDeviceType : ObjectType<IInputDevice>
    {
        protected override void Configure(IObjectTypeDescriptor<IInputDevice> descriptor)
        {
            descriptor.Name("InputDevice")
                .Description("Describes an enumerated input device with a determined layout.");
            descriptor.Field(c => c.Instances)
                .Description("The enumerated driver instances of this device.")
                .Type<NonNullType<ListType<InputDeviceInstanceType>>>();
            descriptor.Field(c => c.VendorID)
                .Name("vendorId")
                .Description("The Vendor ID of the device.");
            descriptor.Field(c => c.ProductID)
                .Name("productId")
                .Description("The Product ID of the device.");
            descriptor.Field(c => c.InstanceGuid)
                .Description("The Instance GUID of the device.")
                .Name("instanceId")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(c => c.FriendlyName)
                .Description("A user-friendly name to refer to this device.");
            descriptor.Field(c => c.DeviceName)
                .Description("The hardware-defined name of this device.");
            descriptor.Field(c => c.DevicePath)
                .Description("The path to this device instance.");
        }
    }
}
