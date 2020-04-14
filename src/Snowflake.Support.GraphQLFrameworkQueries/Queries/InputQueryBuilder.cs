using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Inputs.MappedControllerElement;
using Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice;
using Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice.Mapped;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class InputQueryBuilder : QueryBuilder
    {
        public IDeviceEnumerator DeviceEnumerator { get; }
        public IControllerElementMappingsStore MappedElementStore { get; }
        public IStoneProvider StoneProvider { get; }

        public InputQueryBuilder(IDeviceEnumerator deviceEnumerator,
            IControllerElementMappingsStore mappedElementCollectionStore,
            IStoneProvider stoneProvider)
        {
            this.DeviceEnumerator = deviceEnumerator;
            this.MappedElementStore = mappedElementCollectionStore;
            this.StoneProvider = stoneProvider;
        }

        [Connection("inputDevices", "Gets all connected input devices on this computer.",
            typeof(InputDeviceGraphType))]
        public IEnumerable<IInputDevice> GetInputDevices()
        {
            return this.DeviceEnumerator.QueryConnectedDevices();
        }

        [Query("controllerProfile", "Gets a controller profile for the given Stone controller and real device",
            typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "deviceName", "The hardware device name of the device.")]
        [Parameter(typeof(int), typeof(IntGraphType), "vendorId", "The vendor ID number of the device.")]
        [Parameter(typeof(InputDriver), typeof(InputDriverEnum), "inputDriver", "The input driver of the instance for this controller profile.")]
        [Parameter(typeof(string), typeof(StringGraphType), "controllerId", "The Stone Controller ID that this mapping represents.")]

        [Parameter(typeof(string), typeof(StringGraphType), "profileName", "A profile name for the mapping.", nullable: true)]
        public IControllerElementMappings GetProfile(string deviceName, int vendorId, InputDriver inputDriver, string controllerId,
            string profileName = "default")
        {
            return this.MappedElementStore.GetMappings(controllerId, inputDriver, deviceName, vendorId, profileName ?? "default");
        }

        [Connection("controllerProfiles", "Gets all a controller profiles for the given real device",
            typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "deviceName", "The hardware device name of the device.")]
        [Parameter(typeof(int), typeof(IntGraphType), "vendorId", "The vendor ID number of the device.")]
        [Parameter(typeof(string), typeof(StringGraphType), "controllerId", "The Stone Controller ID that this mapping represents.")]
        public IEnumerable<IControllerElementMappings> GetProfiles(string deviceName, int vendorId, string controllerId)
        {
            return this.MappedElementStore.GetMappings(controllerId, deviceName, vendorId);
        }

        [Query("defaultLayout",
            "Gets the default mapping between Stone controller IDs and the provided device.",
            typeof(ListGraphType<MappedControllerElementGraphType>))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "deviceInstanceGuid", "The device instance to get the layout for.")]
        [Parameter(typeof(InputDriver), typeof(InputDriverEnum), "inputDriver", "The input driver of the instance for this controller profile.")]
        public IEnumerable<MappedControllerElement> GetDefaultProfile(Guid deviceInstanceGuid, InputDriver inputDriver)
        {
            var device = this.DeviceEnumerator.QueryConnectedDevices()
                .FirstOrDefault(i => i.InstanceGuid == deviceInstanceGuid)?.Instances
                .FirstOrDefault(i => i.Driver == inputDriver);
            return device.DefaultLayout;
        }

        [Mutation("createControllerProfile",
            "Creates the default controller profile for the given Stone controller and real device.",
            typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(DefaultMappedControllerElementCollectionInputObject),
            typeof(DefaultMappedControllerElementCollectionInputType), "input", "The input")]
        public IControllerElementMappings CreateProfile(DefaultMappedControllerElementCollectionInputObject input)
        {
            var device = this.DeviceEnumerator.QueryConnectedDevices()
                .FirstOrDefault(d => d.InstanceGuid == input.InstanceGuid);
            var instance = device?.Instances
                .FirstOrDefault(i => i.Driver == input.InputDriver);
            // todo : throw error here?
            if (instance == null) return null;
            if (!this.StoneProvider.Controllers.TryGetValue(input.ControllerId, out var controllerLayout)) return null;

            var defaults = new ControllerElementMappings(device!.DeviceName, controllerLayout,
                input.InputDriver, device.VendorID, instance.DefaultLayout);
            this.MappedElementStore.AddMappings(defaults, input.ProfileName);
            return this.MappedElementStore.GetMappings(input.ControllerId, instance.Driver,
                device.DeviceName, device.VendorID, input.ProfileName);
        }

        [Mutation("setControllerProfile",
            "Sets the values of the given controller profile for the given Stone controller and real device.",
            typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(MappedControllerElementCollectionInputObject),
            typeof(MappedControllerElementCollectionInputType), "input", "The input")]
        public IControllerElementMappings SetProfile(MappedControllerElementCollectionInputObject input)
        {
            var collection = this.MappedElementStore.GetMappings(input.ControllerId, input.InputDriver, 
                input.DeviceName, input.VendorID, input.ProfileName);

            foreach (var mapping in input.Mappings)
            {
                collection[mapping.LayoutElement] = mapping.DeviceCapability;
            }

            this.MappedElementStore.UpdateMappings(collection, input.ProfileName);
            return this.MappedElementStore.GetMappings(input.ControllerId, input.InputDriver,
                input.DeviceName, input.VendorID, input.ProfileName);
        }
    }
}
