using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Extensibility.Extensions;
using Snowflake.Remoting.GraphQL;
using Snowflake.Remoting.GraphQL.Model.Orchestration;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Game.Orchestration
{
    internal sealed class ConnectedEmulatedControllerType
        : ObjectType<IEmulatedPortDeviceEntry>
    {
        protected override void Configure(IObjectTypeDescriptor<IEmulatedPortDeviceEntry> descriptor)
        {
            descriptor.Name("ConnectedEmulatorController")
                .Description("Describe the connection state of the physical device plugged into an emulated port.");
            descriptor.Field("isConnected")
                .Description("Whether or not the physical device of the emulated controller is connected.")
                .Resolve(ctx =>
                {
                    var devices = ctx.SnowflakeService<IDeviceEnumerator>();
                    var entry = ctx.Parent<IEmulatedPortDeviceEntry>();
                    return devices.IsPortDeviceConnected(entry);
                })
                .Type<NonNullType<BooleanType>>();
            descriptor.Field("controller")
                .Description("The emulated controller plugged into the port.")
                .Resolve(async ctx =>
                {
                    var deviceEntry = ctx.Parent<IEmulatedPortDeviceEntry>();
                    var inputConfig = ctx.SnowflakeService<IControllerElementMappingProfileStore>();
                    var devices = ctx.SnowflakeService<IDeviceEnumerator>();
                    var stone = ctx.SnowflakeService<IStoneProvider>();
                    stone.Controllers.TryGetValue(deviceEntry.ControllerID, out var controllerLayout);
                    IInputDevice physicalDevice = devices.GetPortDevice(deviceEntry);
                    IInputDeviceInstance deviceInstance = devices.GetPortDeviceInstance(deviceEntry);
                    IControllerElementMappingProfile mappingProfile = await inputConfig.GetMappingsAsync(deviceEntry.ProfileGuid);
  
                    return new EmulatedController(deviceEntry.PortIndex, physicalDevice, deviceInstance, controllerLayout, mappingProfile);
                })
                .Type<NonNullType<EmulatedControllerType>>();
            descriptor.Field(c => c.PortIndex)
              .Description("The port index.")
              .Type<NonNullType<IntType>>();
            descriptor.Field("portDeviceEntry")
              .Description("The emulated port device entry.")
              .Resolve(ctx => ctx.Parent<IEmulatedPortDeviceEntry>())
              .Type<NonNullType<EmulatedPortDeviceEntryType>>();
        }
    }
}
