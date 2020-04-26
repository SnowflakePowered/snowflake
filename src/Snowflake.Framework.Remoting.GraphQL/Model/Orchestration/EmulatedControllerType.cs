using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL.Model.Device;
using Snowflake.Remoting.GraphQL.Model.Device.Mapped;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Orchestration
{
    public sealed class EmulatedControllerType
        : ObjectType<IEmulatedController>
    {
        protected override void Configure(IObjectTypeDescriptor<IEmulatedController> descriptor)
        {
            descriptor.Name("EmulatedController")
                .Description("An association of a real input device to a virtual emulated device being used for an emulation instance.");
            descriptor.Field(e => e.LayoutMapping)
                .Description("The controller layout mapping associated with this emulated controller. If this is null, then the layout" +
                " was previously deleted, and this emulated controller can not be used.")
                .Type<ControllerElementMappingProfileType>();
            descriptor.Field(e => e.PortIndex)
                .Description("The zero-indexed emulated port index; i.e. Player 1 is represented by a port index of 0.")
                .Type<NonNullType<IntType>>();
            descriptor.Field(e => e.TargetLayout)
                .Description("The target controller layout that is being emulated. If this is null, then the virtual port has" +
                "an invalid controller ID.")
                .Type<ControllerLayoutType>();
            descriptor.Field(e => e.PhysicalDevice)
                .Description("The physical input device that provides inputs as the emulated controller." +
                " If this is null, the physical device is disconnected.")
                .Type<InputDeviceType>();
            descriptor.Field(e => e.PhysicalDeviceInstance)
                .Description("The device instance of the physical input device that provides inputs as the emulated controller.")
                .Type<InputDeviceInstanceType>();
        }
    }
}
