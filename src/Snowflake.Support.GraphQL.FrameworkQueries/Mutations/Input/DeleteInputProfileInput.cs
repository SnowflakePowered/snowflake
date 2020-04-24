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
    internal sealed class DeleteInputProfileInput
    {
        public DeviceIdentifierInput Device { get; set; }
        public InputDriver InputDriver { get; set; }
        public ControllerId ControllerID { get; set; }
        public string ProfileName { get; set; }
    }

    internal sealed class DeleteInputProfileInputType
        : InputObjectType<DeleteInputProfileInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeleteInputProfileInput> descriptor)
        {
            descriptor.Field(i => i.Device)
                .Name("device")
                .Description("Identifies the device or device archetype to delete this profile for.")
                .Type<NonNullType<DeviceIdentifierInputType>>();
            descriptor.Field(i => i.InputDriver)
                .Description("The input driver of the profile to delete.")
                .Type<NonNullType<InputDriverEnum>>();
            descriptor.Field(i => i.ControllerID)
                .Name("controllerId")
                .Description("The controllerId of the Stone controller layout this profile is for.")
                .Type<NonNullType<ControllerIdType>>();
            descriptor.Field(i => i.ProfileName)
                .Description("The name of the profile to delete.")
                .Type<NonNullType<StringType>>();
        }
    }
}
