using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Support.Remoting.GraphQL.Types.ControllerLayout;

namespace Snowflake.Support.Remoting.GraphQL.Types.InputDevice.Mapped
{
    public class MappedControllerElementGraphType : ObjectGraphType<IMappedControllerElement>
    {
        public MappedControllerElementGraphType()
        {
            Name = "MappedControllerElement";
            Description =
                "Decribes a mapping between an element on a real input device to an element on an emulated controller.";
            Field<ControllerElementEnum>("deviceElement",
                description: "The element on the real input device.",
                resolve: context => context.Source.DeviceElement);
            Field<ControllerElementEnum>("layoutElement",
                description: "The element on the emulated input device the real device element maps to.",
                resolve: context => context.Source.LayoutElement);
        }
    }
}
