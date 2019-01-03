using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Support.Remoting.GraphQL.Inputs.InputDevice;

namespace Snowflake.Support.Remoting.GraphQL.Inputs.EmulatedController
{
    public class EmulatedControllerInputType : InputObjectGraphType<EmulatedControllerInputObject>
    {
        public EmulatedControllerInputType()
        {
            Name = "EmulatedControllerInput";
            Field(p => p.PortIndex)
                .Description("The port index of the emulated controller.");
            Field(p => p.ControllerProfile)
                .Description("The name of the desired mapping profile.")
                .DefaultValue("default");
            Field(p => p.TargetLayout)
                .Description("The name of the desired Stone controller to map the device to.");
            Field<InputDeviceInputType>("inputDevice",
                resolve: context => context.Source.InputDevice,
                description: "The physical input device for this emulated controller.");
        }
    }
}
