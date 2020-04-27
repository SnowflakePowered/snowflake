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
    internal sealed class ControllerElementMappingInput
    {
        public ControllerElement LayoutElement { get; set; }
        public DeviceCapability DeviceCapability { get; set; }
    }

    internal sealed class ControllerElementMappingInputType
        : InputObjectType<ControllerElementMappingInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<ControllerElementMappingInput> descriptor)
        {
            descriptor.Name(nameof(ControllerElementMappingInput));
            descriptor.Field(i => i.LayoutElement)
                .Description("The controller layout element that will be emulated.")
                .Type<NonNullType<ControllerElementEnum>>();
            descriptor.Field(i => i.DeviceCapability)
                .Description("The capability on the physical input device that will act as the layout element.")
                .Type<NonNullType<DeviceCapabilityEnum>>();
        }
    }
}
