using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Plugin.Emulators.TestEmulator.Configuration;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Inputs.MappedControllerElement;
using Snowflake.Support.Remoting.GraphQl.Types.Configuration;
using Snowflake.Support.Remoting.GraphQl.Types.InputDevice;
using Snowflake.Support.Remoting.GraphQl.Types.InputDevice.Mapped;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class InputQueryBuilder : QueryBuilder
    {
        public IInputManager Manager { get; }
        public IPluginManager Plugins { get; }
        public IEnumerable<IInputEnumerator> Enumerators => this.Plugins.Get<IInputEnumerator>();
        public IMappedControllerElementCollectionStore MappedElementStore { get; }
        public IStoneProvider StoneProvider { get; }

        public InputQueryBuilder(IInputManager manager,
            IPluginManager pluginManager,
            IMappedControllerElementCollectionStore mappedElementCollectionStore,
            IStoneProvider stoneProvider)
        {
            this.Manager = manager;
            this.Plugins = pluginManager;
            this.MappedElementStore = mappedElementCollectionStore;
            this.StoneProvider = stoneProvider;
        }

        [Connection("lowLevelInputDevices", "Gets all enumerated input devices on this computer.", typeof(LowLevelInputDeviceGraphType))]
        public IEnumerable<ILowLevelInputDevice> GetLLInputs()
        {
            return this.Manager.GetAllDevices();
        }

        [Connection("inputDevices", "Gets the sorted input devices on this computer.", typeof(InputDeviceGraphType))]
        public IEnumerable<IInputDevice> GetAllInputDevices()
        {
            return this.Enumerators.SelectMany(p => p.GetConnectedDevices());
        }

        [Field("mappedControllerProfile", "Gets a controller profile for the given Stone controller and real device", typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "controllerId", "The Stone Controller ID to map to.")]
        [Parameter(typeof(string), typeof(StringGraphType), "deviceId", "The real device to map from.")]
        [Parameter(typeof(string), typeof(StringGraphType), "profileName", "A profile name.", nullable: true)]
        public IMappedControllerElementCollection GetProfile(string controllerId, string deviceId, string profileName = "default")
        {
            return this.MappedElementStore.GetMappingProfile(controllerId, deviceId, profileName);
        }

        // todo: make this a mutation input object.
        [Field("defaultControllerProfile", "Gets the default controller profile for the given Stone controller and real device.", typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "controllerId", "The Stone Controller ID to map to.")]
        [Parameter(typeof(string), typeof(StringGraphType), "deviceId", "The real device to map from.")]
        [Parameter(typeof(string), typeof(StringGraphType), "profileName", "A profile name.", nullable: true)]
        public IMappedControllerElementCollection GetDefaultProfile(string controllerId, string deviceId, string profileName = "default")
        {
            var emulatedController = this.StoneProvider.Controllers[controllerId];
            var realController = this.GetAllInputDevices().FirstOrDefault(p => p.DeviceId == deviceId)?.DeviceLayout;

            // todo: check for nulls
            return MappedControllerElementCollection.GetDefaultMappings(realController, emulatedController);
        }

        [Mutation("createControllerProfile", "Creates the default controller profile for the given Stone controller and real device.", typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(DefaultMappedControllerElementCollectionInputObject), typeof(DefaultMappedControllerElementCollectionInputType), "input", "The input")]
        public IMappedControllerElementCollection CreateProfile(DefaultMappedControllerElementCollectionInputObject input)
        {
            var emulatedController = this.StoneProvider.Controllers[input.ControllerId];
            var realController = this.GetAllInputDevices().FirstOrDefault(p => p.DeviceId == input.DeviceId)?.DeviceLayout;

            // todo: check for nulls
            var defaults = MappedControllerElementCollection.GetDefaultMappings(realController, emulatedController);
            this.MappedElementStore.SetMappingProfile(defaults, input.ProfileName);
            return this.MappedElementStore.GetMappingProfile(input.ControllerId, input.DeviceId, input.ProfileName);
        }

        [Mutation("setControllerProfile", "Creates the default controller profile for the given Stone controller and real device.", typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(MappedControllerElementCollectionInputObject), typeof(MappedControllerElementCollectionInputType), "input", "The input")]
        public IMappedControllerElementCollection SetProfile(MappedControllerElementCollectionInputObject input)
        {
            var collection = new MappedControllerElementCollection(input.DeviceId, input.ControllerId);
            foreach (var mapping in input.Mappings)
            {
                collection.Add(new MappedControllerElement(mapping.LayoutElement) { DeviceElement = mapping.DeviceElement });
            }

            this.MappedElementStore.SetMappingProfile(collection, input.ProfileName);
            return this.MappedElementStore.GetMappingProfile(input.ControllerId, input.DeviceId, input.ProfileName);
        }
    }
}
