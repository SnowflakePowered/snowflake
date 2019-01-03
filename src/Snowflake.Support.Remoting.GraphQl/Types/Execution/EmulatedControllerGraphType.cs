using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Execution.Extensibility;
using Snowflake.Support.Remoting.GraphQL.Types.ControllerLayout;
using Snowflake.Support.Remoting.GraphQL.Types.InputDevice;
using Snowflake.Support.Remoting.GraphQL.Types.InputDevice.Mapped;

namespace Snowflake.Support.Remoting.GraphQL.Types.Execution
{
    public class EmulatedControllerGraphType : ObjectGraphType<IEmulatedController>
    {
        public EmulatedControllerGraphType()
        {
            Name = "EmulatedController";
            Description =
                "Represents an emulated controller definition with a real input device, a target layout, and a mapping.";
            Field(p => p.PortIndex)
                .Description("The index of the emulated port for this emulated controller.");
            Field<InputDeviceGraphType>("inputDevice",
                resolve: context => context.Source.PhysicalDevice,
                description: "The physical input device for this emulated controller");
            Field<MappedControllerElementCollectionGraphType>("layoutMapping",
                resolve: context => context.Source.LayoutMapping,
                description: "The layout mapping for this emulated controller.");
            Field<ControllerLayoutGraphType>("targetLayout",
                resolve: context => context.Source.TargetLayout,
                description: "The target layout for this emulated controller.");
        }
    }
}
