using HotChocolate;
using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Services;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Input
{
    internal sealed class InputMutations
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.ExtendMutation();
            descriptor.Field("createInputProfile")
                .Description("Creates a new input profile.")
                .Argument("input", arg => arg.Type<NonNullType<CreateInputProfileInputType>>())
                .UseClientMutationId()
                .UseAutoSubscription()
                .Resolve(async ctx =>
                {
                    var input = ctx.ArgumentValue<CreateInputProfileInput>("input");
                    var devices = ctx.SnowflakeService<IDeviceEnumerator>().QueryConnectedDevices();
                    (int vendorId, string deviceName) = (default, default);

                    try
                    {
                        (vendorId, deviceName) = input.Device.RetrieveDeviceName(devices);
                    }
                    catch (ArgumentException e)
                    {
                        return ErrorBuilder.New()
                            .SetCode("INPT_INVALID_DEVICE")
                            .SetMessage(e.Message)
                            .Build();
                    }

                    var mappingStore = ctx.SnowflakeService<IControllerElementMappingProfileStore>();
                   
                    if (!ctx.SnowflakeService<IStoneProvider>().Controllers.TryGetValue(input.ControllerID, out var controllerLayout))
                        return ErrorBuilder.New()
                           .SetCode("INPT_NOTFOUND_CONTROLLER")
                           .SetMessage("The specified controller layout was not found")
                           .Build();

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

                    if (instance == null)
                        return ErrorBuilder.New()
                           .SetCode("INPT_NOTFOUND_INSTANCE")
                           .SetMessage("No connected instance of the specified input device and driver combination was found. " +
                        "A connected instance is required to create a new input profile.")
                           .Build();

                    var defaults = new ControllerElementMappingProfile(deviceName, controllerLayout,
                        input.InputDriver, vendorId, instance.DefaultLayout);
                    await mappingStore.AddMappingsAsync(defaults, input.ProfileName);
                    return new InputProfilePayload()
                    {
                        InputProfile = defaults,
                    };
                }).Type<NonNullType<InputProfilePayloadType>>();

            descriptor.Field("updateInputProfile")
                .Description("Updates an input profile with new controller element mappings. Mappings can not be 'unset', but can be set to the device capability NONE.")
                .UseClientMutationId()
                .UseAutoSubscription()
                .Argument("input", arg => arg.Type<NonNullType<UpdateInputProfileInputType>>())
                .Resolve(async ctx =>
                {
                    var input = ctx.ArgumentValue<UpdateInputProfileInput>("input");

                    var mappingStore = ctx.SnowflakeService<IControllerElementMappingProfileStore>();
                    var profile = await mappingStore
                        .GetMappingsAsync(input.ProfileID);

                    if (profile == null)
                        return ErrorBuilder.New()
                           .SetCode("INPT_NOTFOUND_PROFILE")
                           .SetMessage("The specified input profile was not found.")
                           .Build();

                    foreach (var mapping in input.Mappings)
                    {
                        profile[mapping.LayoutElement] = mapping.DeviceCapability;
                    }

                    await mappingStore.UpdateMappingsAsync(profile);
                    return new InputProfilePayload()
                    {
                        InputProfile = profile,
                    };
                }).Type<NonNullType<InputProfilePayloadType>>();

            descriptor.Field("deleteInputProfile")
                .Description("Deletes the specified input profile.")
                .Argument("input", arg => arg.Type<NonNullType<DeleteInputProfileInputType>>())
                .Resolve(async ctx =>
                {
                    var input = ctx.ArgumentValue<DeleteInputProfileInput>("input");
                    var mappingStore = ctx.SnowflakeService<IControllerElementMappingProfileStore>();

                    var profile = await mappingStore.GetMappingsAsync(input.ProfileID);
                    if (profile == null)
                        return ErrorBuilder.New()
                           .SetCode("INPT_NOTFOUND_PROFILE")
                           .SetMessage("The specified input profile was not found.")
                           .Build();

                    await mappingStore.DeleteMappingsAsync(input.ProfileID);
                    return new InputProfilePayload()
                    {
                        InputProfile = profile,
                    };
                }).Type<NonNullType<InputProfilePayloadType>>();
        }
    }
}
