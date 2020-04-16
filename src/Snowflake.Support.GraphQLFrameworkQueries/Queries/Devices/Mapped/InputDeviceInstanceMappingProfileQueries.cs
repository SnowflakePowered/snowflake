using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Device.Mapped;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

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
                .Argument("controllerID", arg =>
                   arg.Type<NonNullType<ControllerIdType>>()
                    .Description("The Stone controller ID to get compatible mappings for."))
                .Resolver(context =>
                {
                    var deviceInstance = (IInputDeviceInstance)context.Source.Peek();
                    var device = (IInputDevice)context.Source.Pop().Peek();
                    var store = context.Service<IControllerElementMappingProfileStore>();
                    var controllerId = context.Argument<ControllerId>("controllerID");
                    return store.GetProfileNames(controllerId, deviceInstance.Driver, device.DeviceName, device.VendorID);
                });

            descriptor.Field("mapping")
                .Description("Fetches a specific mapping profile for a specific device instance.")
                .Argument("controllerID", arg => 
                   arg.Type<NonNullType<ControllerIdType>>()
                    .Description("The Stone controller ID to get compatible mappings for."))
                .Argument("profileName", arg =>
                   arg.Type<NonNullType<StringType>>()
                    .Description("The profile to fetch."))
                .Resolver(context =>
                {

                    var deviceInstance = (IInputDeviceInstance)context.Source.Peek();
                    var device = (IInputDevice)context.Source.Pop().Peek();
                    var store = context.Service<IControllerElementMappingProfileStore>();
                    var controllerId = context.Argument<ControllerId>("controllerID");
                    string profileName = context.Argument<string>("profileName");
                    return store.GetMappings(controllerId, deviceInstance.Driver, device.DeviceName, device.VendorID, profileName);
                })
                .Type<ControllerElementMappingProfileType>();
        }
    }
}
