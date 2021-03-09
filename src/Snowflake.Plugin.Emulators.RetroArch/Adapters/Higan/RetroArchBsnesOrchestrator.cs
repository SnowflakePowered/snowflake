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
using Snowflake.Filesystem;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Configuration;
using Snowflake.Configuration;
using System.Linq;
using System.Text.Json;

namespace Snowflake.Adapters.Higan
{
    [Plugin("RetroArch-Bsnes")]

    public class RetroArchBsnesOrchestrator : EmulatorOrchestrator
    {
        public RetroArchBsnesOrchestrator(IEmulatorExecutable retroArchExecutable, IPluginProvision provision)
            : base(provision)
        {
            this.RetroArchExecutable = retroArchExecutable;
            var mapping = JsonSerializer.Deserialize<DictionaryInputMapping>
                (this.Provision.CommonResourceDirectory.OpenFile("inputmappings.json").ReadAllText());
            this.Mappings = new Dictionary<InputDriver, IDeviceInputMapping>()
                {
                    {InputDriver.Keyboard, mapping },
                    {InputDriver.DirectInput, mapping },
                    {InputDriver.XInput, mapping },
                };
        }

        private Dictionary<InputDriver, IDeviceInputMapping> Mappings { get; }
        private IEmulatorExecutable RetroArchExecutable { get; }

        public override EmulatorCompatibility CheckCompatibility(IGame game)
        {
            // todo check stuff
            return EmulatorCompatibility.Ready;
        }

        public override IConfigurationCollection CreateGameConfiguration(IGame game, string profile)
        {
            return game.WithConfigurations()
                .CreateNewProfile<HiganRetroArchConfiguration>(nameof(RetroArchBsnesOrchestrator), profile);
        }

        public override IEnumerable<(string, Guid)> GetConfigurationProfiles(IGame game)
        {
            return game.WithConfigurations().GetProfileNames()
                .FirstOrDefault(g => g.Key == nameof(RetroArchBsnesOrchestrator))
                ?? Enumerable.Empty<(string, Guid)>();
        }

        public override IConfigurationCollection GetGameConfiguration(IGame game, Guid profile)
        {
            return game.WithConfigurations()
                .GetProfile<HiganRetroArchConfiguration>(nameof(RetroArchBsnesOrchestrator), profile);
        }

        public override IGameEmulation 
            ProvisionEmulationInstance(IGame game, IEnumerable<IEmulatedController> controllerPorts,
            Guid configurationProfileGuid, ISaveProfile saveProfile)
        {
            var configuration = game.WithConfigurations()
                .GetProfile<HiganRetroArchConfiguration>(nameof(RetroArchBsnesOrchestrator), 
                configurationProfileGuid);
            var gameEmulation = new RetroArchBsnesGameEmulation(game,
                configuration,
                controllerPorts,
                saveProfile,
                this.Mappings,
                this.RetroArchExecutable);
            return gameEmulation;
        }
    }
}
