using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Device.Mapped;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices.Mapped.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices.Mapped
{
    public sealed class InputDeviceMappingProfilesQueries
        : ObjectTypeExtension<IInputDevice>
    {
        protected override void Configure(IObjectTypeDescriptor<IInputDevice> descriptor)
        {
            descriptor.Name("InputDevice");
            descriptor.Field("mappings")
                .Argument("controllerID", arg =>
                   arg.Type<NonNullType<ControllerIdType>>()
                    .Description("The Stone controller ID to get compatible mappings for."))
                .Resolver(context =>
                {
                    var device = context.Parent<IInputDevice>();
                    var store = context.Service<IControllerElementMappingProfileStore>();
                    var controllerId = context.Argument<ControllerId>("controllerID");
                    return store.GetMappings(controllerId, device.DeviceName, device.VendorID);
                })
                .Type<NonNullType<ListType<ControllerElementMappingProfileType>>>()
                .UseFiltering<ControllerElementMappingProfileFilterInputType>();
        }
    }
}
