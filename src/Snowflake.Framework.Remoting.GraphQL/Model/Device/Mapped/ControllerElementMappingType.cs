using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;

namespace Snowflake.Remoting.GraphQL.Model.Device.Mapped
{
    public sealed class ControllerElementMappingType
        : ObjectType<ControllerElementMapping>
    {
        protected override void Configure(IObjectTypeDescriptor<ControllerElementMapping> descriptor)
        {
            descriptor.Name("ControllerElementMapping")
                .Description("Decribes a mapping between a capability on a real input device to an element on an emulated controller.");
            descriptor.Field(c => c.DeviceCapability)
                .Name("capability")
                .Description("The capability on the real input device that will map to the emulated controller element.")
                .Type<DeviceCapabilityEnum>();
            descriptor.Field(c => c.LayoutElement)
               .Name("element")
               .Description("The layout element on the defined controller layout that the device capability will map to.")
               .Type<ControllerElementEnum>();
        }
    }
}
