using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Device;

namespace Snowflake.Support.Remoting.GraphQl.Types.InputDevice
{
    public class LowLevelInputDeviceGraphType : ObjectGraphType<ILowLevelInputDevice>
    {
        public LowLevelInputDeviceGraphType()
        {
            Name = "LowLevelInputDevice";
            Description = "Describes an attached input device at the input API level.";
            Field<StringGraphType>("di_InstanceName",
                description: "DirectInput Instance Name.",
                resolve: context => context.Source.DI_InstanceName);
            Field<StringGraphType>("di_InterfacePath",
                description: "DirectInput Interface Path.",
                resolve: context => context.Source.DI_InterfacePath);
            Field<StringGraphType>("di_ProductName",
                description: "DirectInput Product Name.",
                resolve: context => context.Source.DI_ProductName);
            Field<GuidGraphType>("di_ProductGuid",
                description: "DirectInput ProductGuid.",
                resolve: context => context.Source.DI_ProductGUID);
            Field<GuidGraphType>("di_InstanceGuid",
                description: "DirectInput InstanceGuid.",
                resolve: context => context.Source.DI_InstanceGUID);
            Field<IntGraphType>("di_EnumerationNumber",
                description: "DirectInput Enumeration Number.",
                resolve: context => context.Source.DI_EnumerationNumber);
            Field<IntGraphType>("di_JoystickID",
                description: "DirectInput Joystick ID.",
                resolve: context => context.Source.DI_JoystickID);
            Field<IntGraphType>("di_ProductID",
                description: "DirectInput Product ID",
                resolve: context => context.Source.DI_ProductID);
            Field<IntGraphType>("di_VendorID",
                description: "DirectInput Vendor ID",
                resolve: context => context.Source.DI_VendorID);
            Field<InputApiEnum>("discoveryApi",
                description: "Discovery API.",
                resolve: context => context.Source.DiscoveryApi);
            Field<BooleanGraphType>("xi_IsXinput",
                description: "XInput Is XInput Device.",
                resolve: context => context.Source.XI_IsXInput);
            Field<IntGraphType>("xi_GamepadIndex",
                description: "XInput Gamepad ID.",
                resolve: context => context.Source.XI_GamepadIndex);
            Field<BooleanGraphType>("xi_IsConnected",
                description: "XInput Is Connected.",
                resolve: context => context.Source.XI_IsConnected);
            Field<StringGraphType>("udev_Vendor",
                description: "UDEV Vendor.",
                resolve: context => context.Source.UDEV_Vendor);
            Field<StringGraphType>("udev_MountPath",
                description: "UDEV MountPath.",
                resolve: context => context.Source.UDEV_MountPath);
            Field<StringGraphType>("udev_Model",
                description: "UDEV Model.",
                resolve: context => context.Source.UDEV_Model);
        }
    }
}
