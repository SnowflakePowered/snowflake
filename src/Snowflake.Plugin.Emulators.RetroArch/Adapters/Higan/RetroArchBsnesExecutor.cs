using Snowflake.Configuration.Input;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Saving;
using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Orchestration.Process;
using Newtonsoft.Json;
using Snowflake.Filesystem;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Configuration;
using Snowflake.Configuration;

namespace Snowflake.Adapters.Higan
{
    [Plugin("RetroArch-BsnesExecutor")]
    public class RetroArchBsnesExecutor : EmulatorOrchestrator
    {
        public RetroArchBsnesExecutor(IEmulatorExecutable retroArchExecutable, IPluginProvision provision)
            : base(provision)
        {
            this.RetroArchExecutable = retroArchExecutable;
            var mapping = JsonConvert.DeserializeObject<IDeviceInputMapping>
                (this.Provision.CommonResourceDirectory.OpenFile("inputmappings.json").ReadAllText());
            this.Mappings = new Dictionary<InputDriverType, IDeviceInputMapping>()
                {
                    {InputDriverType.Keyboard, mapping },
                    {InputDriverType.DirectInput, mapping },
                    {InputDriverType.XInput, mapping },
                };
        }

        private Dictionary<InputDriverType, IDeviceInputMapping> Mappings { get; }
        private IEmulatorExecutable RetroArchExecutable { get; }

        public override IConfigurationCollection CreateGameConfiguration(IGame game, string profile)
        {
            return game.WithConfigurations()
                .CreateNewProfile<HiganRetroArchConfiguration>(nameof(RetroArchBsnesExecutor), profile);
        }

        public override IConfigurationCollection GetGameConfigurationValues(IGame game, string profile)
        {
            return game.WithConfigurations()
                .GetProfile<HiganRetroArchConfiguration>(nameof(RetroArchBsnesExecutor), profile);
        }

        public override IGameEmulation 
            ProvisionEmulationInstance(IGame game, IEnumerable<IEmulatedController> controllerPorts, 
            string configurationProfileName, ISaveGame initialSave)
        {
            var configuration = game.WithConfigurations()
                .GetProfile<HiganRetroArchConfiguration>(nameof(RetroArchBsnesExecutor), 
                configurationProfileName);
            var gameEmulation = new RetroArchBsnesGameEmulation(game,
                configuration.Configuration,
                controllerPorts,
                initialSave,
                this.Mappings,
                this.RetroArchExecutable);
            return gameEmulation;
        }
    }
}
