using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Device;
using Snowflake.Support.GraphQLFrameworkQueries.Types.ControllerLayout;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice
{
    public class InputDeviceGraphType : ObjectGraphType<IInputDevice>
    {
        public InputDeviceGraphType()
        {
            Name = "InputDevice";
            Description = "Describes an enumerated input device with a determined layout.";
            Field<ListGraphType<InputDeviceInstanceGraphType>>("instances",
                description: "The enumerated driver instances of this device.",
                resolve: context => context.Source.Instances);
            Field<IntGraphType>("vendorId",
                description: "The Vendor ID of the device.",
                resolve: context => context.Source.VendorID);
            Field<IntGraphType>("productId",
                description: "The product ID of the device.",
                resolve: context => context.Source.ProductID);
            Field<GuidGraphType>("instanceGuid",
                description: "The instance GUID of the device. This can be used to refer to the device later on.",
                resolve: context => context.Source.InstanceGuid);
            Field<StringGraphType>("friendlyName",
                description: "A user-friendly name to refer to this device.",
                resolve: context => context.Source.FriendlyName);
            Field<StringGraphType>("deviceName",
                description: "The hardware-defined name of this device.",
                resolve: context => context.Source.DeviceName);
            Field<StringGraphType>("devicePath",
                description: "The path to this device instance.",
                resolve: context => context.Source.DevicePath);
        }
    }
}
