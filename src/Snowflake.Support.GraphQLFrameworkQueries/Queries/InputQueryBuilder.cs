using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Input;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQL.Inputs.MappedControllerElement;
using Snowflake.Support.Remoting.GraphQL.Types.Configuration;
using Snowflake.Support.Remoting.GraphQL.Types.InputDevice;
using Snowflake.Support.Remoting.GraphQL.Types.InputDevice.Mapped;

namespace Snowflake.Support.Remoting.GraphQL.Queries
{
    public class InputQueryBuilder : QueryBuilder
    {
        public IInputManager Manager { get; }
        public IPluginManager Plugins { get; }
        public IEnumerable<IInputEnumerator> Enumerators => this.Plugins.Get<IInputEnumerator>();
        public IControllerElementMappingsStore MappedElementStore { get; }
        public IStoneProvider StoneProvider { get; }

        public InputQueryBuilder(IInputManager manager,
            IPluginManager pluginManager,
            IControllerElementMappingsStore mappedElementCollectionStore,
            IStoneProvider stoneProvider)
        {
            this.Manager = manager;
            this.Plugins = pluginManager;
            this.MappedElementStore = mappedElementCollectionStore;
            this.StoneProvider = stoneProvider;
        }

        [Connection("lowLevelInputDevices", "Gets all enumerated input devices on this computer.",
            typeof(LowLevelInputDeviceGraphType))]
        public IEnumerable<ILowLevelInputDevice> GetLLInputs()
        {
            return this.Manager.GetAllDevices();
        }

        [Connection("inputDevices", "Gets the sorted input devices on this computer.", typeof(InputDeviceGraphType))]
        public IEnumerable<IInputDevice> GetAllInputDevices()
        {
            return this.Enumerators.SelectMany(p => p.GetConnectedDevices());
        }

        [Field("mappedControllerProfile", "Gets a controller profile for the given Stone controller and real device",
            typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "controllerId", "The Stone Controller ID to map to.")]
        [Parameter(typeof(string), typeof(StringGraphType), "deviceId", "The real device to map from.")]
        [Parameter(typeof(string), typeof(StringGraphType), "profileName", "A profile name.", nullable: true)]
        public IControllerElementMappings GetProfile(string controllerId, string deviceId,
            string profileName = "default")
        {
            return this.MappedElementStore.GetMappings(controllerId, deviceId, profileName);
        }

        // todo: make this a mutation input object.
        [Field("defaultControllerProfile",
            "Gets the default controller profile for the given Stone controller and real device.",
            typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "controllerId", "The Stone Controller ID to map to.")]
        [Parameter(typeof(string), typeof(StringGraphType), "deviceId", "The real device to map from.")]
        [Parameter(typeof(string), typeof(StringGraphType), "profileName", "A profile name.", nullable: true)]
        public IControllerElementMappings GetDefaultProfile(string controllerId, string deviceId,
            string profileName = "default")
        {
            var emulatedController = this.StoneProvider.Controllers[controllerId];
            var realController = this.GetAllInputDevices().FirstOrDefault(p => p.DeviceId == deviceId)?.DeviceLayout;

            // todo: check for nulls
            return ControllerElementMappings.GetDefaultMappings(realController, emulatedController);
        }

        [Mutation("createControllerProfile",
            "Creates the default controller profile for the given Stone controller and real device.",
            typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(DefaultMappedControllerElementCollectionInputObject),
            typeof(DefaultMappedControllerElementCollectionInputType), "input", "The input")]
        public IControllerElementMappings CreateProfile(DefaultMappedControllerElementCollectionInputObject input)
        {
            var emulatedController = this.StoneProvider.Controllers[input.ControllerId];
            var realController = this.GetAllInputDevices().FirstOrDefault(p => p.DeviceId == input.DeviceId)
                ?.DeviceLayout;

            // todo: check for nulls
            var defaults = ControllerElementMappings.GetDefaultMappings(realController, emulatedController);
            this.MappedElementStore.AddMappings(defaults, input.ProfileName);
            return this.MappedElementStore.GetMappings(input.ControllerId, input.DeviceId, input.ProfileName);
        }

        [Mutation("setControllerProfile",
            "Sets the values of the given controller profile for the given Stone controller and real device.",
            typeof(MappedControllerElementCollectionGraphType))]
        [Parameter(typeof(MappedControllerElementCollectionInputObject),
            typeof(MappedControllerElementCollectionInputType), "input", "The input")]
        public IControllerElementMappings SetProfile(MappedControllerElementCollectionInputObject input)
        {
            var collection = this.MappedElementStore.GetMappings(input.ControllerId, input.DeviceId, input.ProfileName);

            foreach (var mapping in input.Mappings)
            {
                collection[mapping.LayoutElement] = mapping.DeviceElement;
            }

            this.MappedElementStore.UpdateMappings(collection, input.ProfileName);
            return this.MappedElementStore.GetMappings(input.ControllerId, input.DeviceId, input.ProfileName);
        }
    }
}
