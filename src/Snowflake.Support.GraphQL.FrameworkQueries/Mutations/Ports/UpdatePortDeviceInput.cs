using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.Model.Device;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Ports
{
    internal sealed class UpdatePortDeviceInput
    {
        public string Orchestrator { get; set; }
        public ControllerId ControllerID { get; set; }
        public Guid InstanceID { get; set; }
        public int PortIndex { get; set; }
        public Guid ProfileID { get; set; }
        public PlatformId PlatformID { get; set; }
        public InputDriver Driver {get;set;}
    }

    internal sealed class UpdatePortDeviceInputType
        : InputObjectType<UpdatePortDeviceInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdatePortDeviceInput> descriptor)
        {
            descriptor.Name(nameof(UpdatePortDeviceInput));
            descriptor.Field(i => i.Orchestrator)
                .Description("The name of the emulator orchestrator that will have its set of virtual ports changed.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(i => i.PortIndex)
                .Description("The zero-indexed port index to update.")
                .Type<NonNullType<IntType>>();
            descriptor.Field(i => i.PlatformID)
                .Name("platformId")
                .Description("The platform ID of the set of ports that will be updated.")
                .Type<NonNullType<PlatformIdType>>();
            descriptor.Field(i => i.ControllerID)
                .Name("controllerId")
                .Description("The controller ID of the controller that will be emulated on this port.")
                .Type<NonNullType<ControllerIdType>>();
            descriptor.Field(i => i.InstanceID)
                .Name("instanceId")
                .Description("The Instance GUID of the physical device that will act as the emulated controller on this port.")
                .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.ProfileID)
               .Name("profileId")
               .Description("The Profile GUID of the controller layout mapping that maps the inputs from the physical device to the " +
               "emulated inputs that will be used.")
               .Type<NonNullType<UuidType>>();
            descriptor.Field(i => i.Driver)
                .Description("The driver of the device instance of the physical device.")
                .Type<NonNullType<InputDriverEnum>>();
        }
    }
}
