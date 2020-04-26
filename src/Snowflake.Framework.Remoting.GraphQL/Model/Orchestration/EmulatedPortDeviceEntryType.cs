using HotChocolate.Types;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Remoting.GraphQL.Model.Device;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Orchestration
{
    public sealed class EmulatedPortDeviceEntryType
        : ObjectType<IEmulatedPortDeviceEntry>
    {
        protected override void Configure(IObjectTypeDescriptor<IEmulatedPortDeviceEntry> descriptor)
        {
            descriptor.Name("EmulatedPortDeviceEntry")
                .Description("Describes the input device 'plugged into' the emulated port of the device, including the emulated controller layout, as well " +
                "as the physical input device being used as the emulated controller.");
            descriptor.Field(e => e.ControllerID)
                .Name("controllerId")
                .Description("The controller ID of the Stone controller layout represented in this port.")
                .Type<NonNullType<ControllerIdType>>();
            descriptor.Field(e => e.PlatformID)
                .Name("platformId")
                .Description("The platform ID of the Stone platform that implements the set of virtual ports this device entry belongs to.")
                .Type<NonNullType<PlatformIdType>>();
            descriptor.Field(e => e.PortIndex)
                .Description("The zero-indexed port index that this emulated device is plugged into.")
                .Type<NonNullType<IntType>>();
            descriptor.Field(e => e.InstanceGuid)
                .Description("The instance GUID of the physical device that provides input to the emulated port.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(e => e.ProfileGuid)
                .Description("The profile GUID of the controller mapping profile that maps the physical input device to the inputs of the emulated controller")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(e => e.Driver)
                .Description("The driver of the device instance of the physical device that provides input to the emulated port.")
                .Type<NonNullType<InputDriverEnum>>();
        }
    }
}
