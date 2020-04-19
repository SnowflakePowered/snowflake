using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Remoting.GraphQL.Attributes;
using Snowflake.Remoting.GraphQL.Query;
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
        public IControllerElementMappingProfileStore MappedElementStore { get; }
        public IStoneProvider StoneProvider { get; }

        public InputQueryBuilder(IDeviceEnumerator deviceEnumerator,
            IControllerElementMappingProfileStore mappedElementCollectionStore,
            IStoneProvider stoneProvider)
        {
            this.DeviceEnumerator = deviceEnumerator;
            this.MappedElementStore = mappedElementCollectionStore;
            this.StoneProvider = stoneProvider;
        }

        [Mutation("createControllerProfile",
            "Creates the default controller profile for the given Stone controller and real device.",
            typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(DefaultMappedControllerElementCollectionInputObject),
            typeof(DefaultMappedControllerElementCollectionInputType), "input", "The input")]
        public IControllerElementMappingProfile CreateProfile(DefaultMappedControllerElementCollectionInputObject input)
        {
            var device = this.DeviceEnumerator.QueryConnectedDevices()
                .FirstOrDefault(d => d.InstanceGuid == input.InstanceGuid);
            var instance = device?.Instances
                .FirstOrDefault(i => i.Driver == input.InputDriver);
            // todo : throw error here?
            if (instance == null) return null;
            if (!this.StoneProvider.Controllers.TryGetValue(input.ControllerId, out var controllerLayout)) return null;

            var defaults = new ControllerElementMappingProfile(device!.DeviceName, controllerLayout,
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
        public IControllerElementMappingProfile SetProfile(MappedControllerElementCollectionInputObject input)
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
