using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Device.Mapped;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL;
using HotChocolate;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices.Mapped
{
    public sealed class InputDeviceInstanceMappingProfileQueries
        : ObjectTypeExtension<IInputDeviceInstance>
    {
        protected override void Configure(IObjectTypeDescriptor<IInputDeviceInstance> descriptor)
        {
            descriptor.Name("InputDeviceInstance");
            descriptor.Field(c => c.DefaultLayout)
                .Name("defaultMapping")
                .Description("The default controller layout mapping for this device.")
                .Type<DeviceLayoutMappingType>();

            descriptor.Field("mappingProfiles")
                .Description("Fetches the mapping profile names for this device instance.")
                .Argument("controllerId", arg =>
                   arg.Type<NonNullType<ControllerIdType>>()
                    .Description("The Stone controller ID to get compatible mappings for."))
                .Resolve(context =>
                {
                    var deviceInstance = context.Parent<IInputDeviceInstance>();
                    var device = (IInputDevice)context.ScopedContextData.GetValueOrDefault(nameof(IInputDevice));
                    if (device == null)
                    {
                        throw new GraphQLException(ErrorBuilder.New()
                                .AddLocation(context.Field.SyntaxNode)
                                .SetMessage("InputDevice was not pushed down.")
                                .SetPath(context.Path)
                                .Build());
                    }
                    var store = context.SnowflakeService<IControllerElementMappingProfileStore>();
                    var controllerId = context.ArgumentValue<ControllerId>("controllerId");
                    return store.GetProfileNames(controllerId, deviceInstance.Driver, device.DeviceName, device.VendorID);
                })
                .Type<NonNullType<ListType<NonNullType<InputProfileType>>>>();

            descriptor.Field("mapping")
                .Description("Fetches a specific mapping profile for a specific device instance.")
                .Argument("profileId", arg =>
                   arg.Type<NonNullType<UuidType>>()
                    .Description("The profile GUID of the profile to fetch."))
                .Resolve(context =>
                {
                    var store = context.SnowflakeService<IControllerElementMappingProfileStore>();
                    var profileId = context.ArgumentValue<Guid>("profileId");
                    return store.GetMappings(profileId);
                })
                .Type<ControllerElementMappingProfileType>();
        }
    }
}
