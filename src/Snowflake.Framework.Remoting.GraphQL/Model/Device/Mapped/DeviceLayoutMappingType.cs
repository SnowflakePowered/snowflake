using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Device.Mapped;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Device.Mapped
{
    public sealed class DeviceLayoutMappingType
        : ObjectType<IDeviceLayoutMapping>
    {
        protected override void Configure(IObjectTypeDescriptor<IDeviceLayoutMapping> descriptor)
        {
            descriptor.Name("DeviceLayoutMapping")
                .Description("Defines the default mapping between a controller element on an unspecified layout to a " +
                "device capability on the input device.");
            descriptor.Field("mappings")
                .Description("The set of mappings that map each element on the unspecified layout to a capability on the real device.")
                .Type<NonNullType<ListType<NonNullType<ControllerElementMappingType>>>>()
                .Resolve(ctx => ctx.Parent<IDeviceLayoutMapping>());
        }
    }
}
