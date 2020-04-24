using HotChocolate.Types;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Remoting.GraphQL;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Input
{
    internal sealed class InputMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Mutation");
            descriptor.Field("createInputProfile")
                .Argument("input", arg => arg.Type<NonNullType<CreateInputProfileInputType>>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<CreateInputProfileInput>("input");
                    var devices = ctx.SnowflakeService<IDeviceEnumerator>().QueryConnectedDevices();
                    var (vendorId, deviceName) = input.Device.RetrieveDeviceName(devices);
                    var mappingStore = ctx.SnowflakeService<IControllerElementMappingProfileStore>();
                    if (!ctx.SnowflakeService<IStoneProvider>().Controllers.TryGetValue(input.ControllerID, out var controllerLayout))
                    {
                        throw new ArgumentException("The specified controller layout was not found.");
                    }

                    IInputDeviceInstance instance = null;
                    if (input.Device.InstanceID.HasValue)
                    {
                        instance = devices.FirstOrDefault(i => i.InstanceGuid == input.Device.InstanceID.Value)?
                            .Instances.FirstOrDefault(i => i.Driver == input.InputDriver);
                    }
                    else
                    {
                        instance = devices.FirstOrDefault(i => i.VendorID == vendorId && i.DeviceName == deviceName)?
                            .Instances.FirstOrDefault(i => i.Driver == input.InputDriver);
                    }

                    if (instance == null) throw new ArgumentException("No connected instance of the specified input device and driver combination was found. " +
                        "A connected instance is required to create a new input profile.");

                    var defaults = new ControllerElementMappingProfile(deviceName, controllerLayout,
                        input.InputDriver, vendorId, instance.DefaultLayout);
                    await mappingStore.AddMappingsAsync(defaults, input.ProfileName);
                    return defaults;
                });

            descriptor.Field("updateInputProfile")
                .Argument("input", arg => arg.Type<NonNullType<UpdateInputProfileInputType>>())
                .Resolver(async ctx =>
                {
                    var input = ctx.Argument<UpdateInputProfileInput>("input");
                    var devices = ctx.SnowflakeService<IDeviceEnumerator>().QueryConnectedDevices();

                    var (vendorId, deviceName) = input.Device.RetrieveDeviceName(devices);
                    var mappingStore = ctx.SnowflakeService<IControllerElementMappingProfileStore>();

                    var profile = await mappingStore.GetMappingsAsync(input.ControllerID, input.InputDriver, deviceName, vendorId, input.ProfileName);
                    
                    foreach (var mapping in input.Mappings)
                    {
                        profile[mapping.LayoutElement] = mapping.DeviceCapability;
                    }

                    await mappingStore.UpdateMappingsAsync(profile, input.ProfileName);
                    return profile;
                });
        }
    }
}
