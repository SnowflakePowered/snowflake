using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Snowflake.Execution.Extensibility;
using Snowflake.Execution.Saving;
using Snowflake.Extensibility;
using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Inputs.EmulatedController;
using Snowflake.Support.Remoting.GraphQl.Types.Execution;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class EmulationQueryBuilder : QueryBuilder
    {
        public IPluginCollection<IEmulator> Emulators { get; }
        public IStoneProvider Stone { get; }
        public ISaveLocationProvider SaveLocationProvider { get; }
        public InputQueryBuilder InputQueryApi { get; }
        public ControllerLayoutQueryBuilder ControllerQueryApi { get; }
        public EmulationQueryBuilder(IPluginCollection<IEmulator> emulators,
            IStoneProvider stone,
            ISaveLocationProvider saveLocationProvider,
            InputQueryBuilder inputQueryBuilder,
            ControllerLayoutQueryBuilder controllerLayoutQueryBuilder)
        {
            this.Emulators = emulators;
            this.Stone = stone;
            this.SaveLocationProvider = saveLocationProvider;
            this.InputQueryApi = inputQueryBuilder;
            this.ControllerQueryApi = controllerLayoutQueryBuilder;
        }

        [Field("testEmuTask", "test", typeof(EmulatorTaskResultGraphType))]
        [Parameter(typeof(IList<EmulatedControllerInputObject>), typeof(ListGraphType<EmulatedControllerInputType>), "controllers", "The emulated controller input")]
        public async Task<IEmulatorTaskResult> TestTask(IList<EmulatedControllerInputObject> controllers)
        {
            var emu = this.Emulators.First();
            var game = new GameRecord(this.Stone.Platforms["NINTENDO_SNES"], "TestGame");
            var saveLocation = await this.SaveLocationProvider.CreateSaveLocationAsync(game, "sram");
            var task = emu.CreateTask(game, saveLocation, controllers.Select(c => this.ParseController(c)).ToList());
            return await emu.Runner.ExecuteEmulationAsync(task);
        }

        [Field("emulatedController", "Gets the emulated controller object", typeof(EmulatedControllerGraphType))]
        [Parameter(typeof(EmulatedControllerInputObject), typeof(EmulatedControllerInputType), "input", "The emulated controller input")]
        public IEmulatedController ParseController(EmulatedControllerInputObject input)
        {
            var controller = this.ControllerQueryApi.GetControllerLayout(input.TargetLayout);
            var device = this.InputQueryApi.GetAllInputDevices()
                .Where(i => i.DeviceApi == input.InputDevice.DeviceApi
                && i.DeviceId == input.InputDevice.DeviceId
                && i.DeviceIndex == input.InputDevice.DeviceIndex)
                                .FirstOrDefault();
            var mapping = this.InputQueryApi.GetProfile(input.TargetLayout, input.InputDevice.DeviceId, input.ControllerProfile);
            return new EmulatedController(input.PortIndex, device, controller, mapping);
        }
    }
}
