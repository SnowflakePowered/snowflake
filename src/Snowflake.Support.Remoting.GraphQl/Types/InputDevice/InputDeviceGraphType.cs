﻿using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Device;
using Snowflake.Support.Remoting.GraphQl.Types.ControllerLayout;

namespace Snowflake.Support.Remoting.GraphQl.Types.InputDevice
{
    public class InputDeviceGraphType : ObjectGraphType<IInputDevice>
    {
        public InputDeviceGraphType()
        {
            Name = "InputDevice";
            Description = "Describes an enumerated input device with a determined layout.";
            Field<InputApiEnum>("deviceApi",
                description: "The input API of this device.",
                resolve: context => context.Source.DeviceApi);
            Field<ControllerLayoutGraphType>("deviceLayout",
               description: "The Stone layout for this device.",
               resolve: context => context.Source.DeviceLayout);
            Field<LowLevelInputDeviceGraphType>("deviceInfo",
                description: "The low level device information for this enumerated input device",
                resolve: context => context.Source.DeviceInfo);
            Field(i => i.ControllerName).Description("The human-readable name for this controller.");
            Field(i => i.DeviceId).Description("The unique name for this type of controller.");
            Field<IntGraphType>("deviceIndex",
                description: "The 0 - indexed controller number for multiple controllers.",
                resolve: context => context.Source.DeviceIndex);
        }
    }
}
