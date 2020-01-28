//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using GraphQL.Types;
//using Snowflake.Execution.Extensibility;
//using Snowflake.Extensibility;
//using Snowflake.Framework.Remoting.GraphQL.Attributes;
//using Snowflake.Framework.Remoting.GraphQL.Query;
//using Snowflake.Model.Game;
//using Snowflake.Services;
//using Snowflake.Support.GraphQLFrameworkQueries.Inputs.EmulatedController;
//using Snowflake.Support.GraphQLFrameworkQueries.Types.Execution;

//namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
//{
//    public class EmulationQueryBuilder : QueryBuilder
//    {
//        public IPluginCollection<IEmulator> Emulators { get; }
//        public IStoneProvider Stone { get; }
//        public IGameLibrary GameLibrary { get; }
//        public InputQueryBuilder InputQueryApi { get; }
//        public ControllerLayoutQueryBuilder ControllerQueryApi { get; }

//        public EmulationQueryBuilder(IPluginCollection<IEmulator> emulators,
//            IStoneProvider stone,
//            IGameLibrary library,
//            InputQueryBuilder inputQueryBuilder,
//            ControllerLayoutQueryBuilder controllerLayoutQueryBuilder)
//        {
//            this.Emulators = emulators;
//            this.Stone = stone;
//            this.InputQueryApi = inputQueryBuilder;
//            this.ControllerQueryApi = controllerLayoutQueryBuilder;
//            this.GameLibrary = library;
//        }

//        [Field("testEmuTask", "test", typeof(EmulatorTaskResultGraphType))]
//        [Parameter(typeof(IList<EmulatedControllerInputObject>), typeof(ListGraphType<EmulatedControllerInputType>),
//            "controllers", "The emulated controller input")]
//        public async Task<IEmulatorTaskResult> TestTask(IList<EmulatedControllerInputObject> controllers)
//        {
//            //var emu = this.Emulators.First();
//            //var game = new GameRecord(this.Stone.Platforms["NINTENDO_SNES"], "TestGame");
//            //var saveLocation = await this.SaveLocationProvider.CreateSaveLocationAsync(game, "sram");
//            //var task = emu.CreateTask(game, saveLocation, controllers.Select(c => this.ParseController(c)).ToList());
//            //return await emu.Runner.ExecuteEmulationAsync(task);
//            return null;
//        }

//        [Field("launchEmulatorTask", "Launches an emulator task", typeof(EmulatorTaskResultGraphType))]
//        [Parameter(typeof(IList<EmulatedControllerInputObject>), typeof(ListGraphType<EmulatedControllerInputType>),
//            "controllers", "The emulated controller input")]
//        [Parameter(typeof(string), typeof(StringGraphType), "emulator", "The name of the emulator to launch.")]
//        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The GUID of the game to launch.")]
//        public async Task<IEmulatorTaskResult> LaunchEmulatorTask(
//            string emulator,
//            Guid gameGuid,
//            IList<EmulatedControllerInputObject> controllers)
//        {
//            //var emu = this.Emulators.Where(e => e.Name.Equals(emulator, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
//            //var game = this.GameLibrary.Get(gameGuid);
//            //var saveLocation = await this.SaveLocationProvider.CreateSaveLocationAsync(game, emu.Properties.SaveFormat);
//            //var task = emu.CreateTask(game, saveLocation, controllers.Select(c => this.ParseController(c)).ToList());
//            //return await emu.Runner.ExecuteEmulationAsync(task);
//            return null;
//        }

//        [Field("emulatedController", "Gets the emulated controller object", typeof(EmulatedControllerGraphType))]
//        [Parameter(typeof(EmulatedControllerInputObject), typeof(EmulatedControllerInputType), "input",
//            "The emulated controller input")]
//        public IEmulatedController ParseController(EmulatedControllerInputObject input)
//        {
//            //var controller = this.ControllerQueryApi.GetControllerLayout(input.TargetLayout);
//            //var device = this.InputQueryApi.GetAllInputDevices()
//            //    .Where(i => i.DeviceApi == input.InputDevice.DeviceApi
//            //                && i.DeviceId == input.InputDevice.DeviceId
//            //                && i.DeviceIndex == input.InputDevice.DeviceIndex)
//            //    .FirstOrDefault();
//            //var mapping = this.InputQueryApi.GetProfile(input.TargetLayout, input.InputDevice.DeviceId,
//            //    input.ControllerProfile);
//            //return new EmulatedController(input.PortIndex, device, controller, mapping);
//        }
//    }
//}
