using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Remoting.GraphQL.Model.Device;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Input
{
    internal sealed class UpdateInputProfileInput
    {
        public DeviceIdentifierInput Device { get; set; }
        public InputDriver InputDriver { get; set; }
        public ControllerId ControllerID { get; set; }
        public string ProfileName { get; set; }
        public List<ControllerElementMappingInput> Mappings { get; set; }
    }

    internal sealed class UpdateInputProfileInputType
        : InputObjectType<UpdateInputProfileInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateInputProfileInput> descriptor)
        {
            descriptor.Field(i => i.Device)
                .Name("device")
                .Description("Identifies the device or device archetype to create this profile for.")
                .Type<NonNullType<DeviceIdentifierInputType>>();
            descriptor.Field(i => i.InputDriver)
                .Description("The input driver this profile is for.")
                .Type<NonNullType<InputDriverEnum>>();
            descriptor.Field(i => i.ControllerID)
                .Name("controllerId")
                .Description("The controllerId of the Stone controller layout this profile is for.")
                .Type<NonNullType<ControllerIdType>>();
            descriptor.Field(i => i.ProfileName)
                .Description("The name of the profile to update.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(i => i.Mappings)
                .Description("The updated mappings to apply.")
                .Type<NonNullType<ListType<NonNullType<ControllerElementMappingInputType>>>>();
        }
    }
}
