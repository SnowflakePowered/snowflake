﻿using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Support.GraphQLFrameworkQueries.Types.ControllerLayout;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice.Mapped
{
    public class MappedControllerElementGraphType : ObjectGraphType<MappedControllerElement>
    {
        public MappedControllerElementGraphType()
        {
            Name = "MappedControllerElement";
            Description =
                "Decribes a mapping between a capability on a real input device to an element on an emulated controller.";
            Field<ControllerElementEnum>("deviceCapability",
                description: "The device capability that will represent the emulated element.",
                resolve: context => context.Source.DeviceCapability);
            Field<ControllerElementEnum>("layoutElement",
                description: "The element on the emulated input device the real device element maps to.",
                resolve: context => context.Source.LayoutElement);
        }
    }
}
