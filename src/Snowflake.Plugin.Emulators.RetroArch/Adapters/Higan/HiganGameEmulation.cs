using Snowflake.Configuration.Input;
using Snowflake.Configuration.Serialization;
using Snowflake.Configuration.Serialization.Serializers.Implementations;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Saving;
using Snowflake.Filesystem;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Plugin.Emulators.RetroArch;
using Snowflake.Plugin.Emulators.RetroArch.Adapters.Higan.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Adapters.Higan
{
    public class HiganGameEmulation : GameEmulation
    {
        public HiganGameEmulation(IGame game, 
            Guid guid,
            IDictionary<InputDriverType, IDeviceInputMapping> inputMappings) : base(game, guid)
        {
            this.InputMappings = inputMappings;
            this.Scratch = this.Game.WithFiles().GetRuntimeLocation();
        }

        public IDictionary<InputDriverType, IDeviceInputMapping> InputMappings { get; }
        public SaveGame InitialSave { get; }
        private IDirectory Scratch { get; }

        public override async Task PersistSaveGame(IDirectory targetDirectory)
        {
            var saveDirectory = this.Scratch.OpenDirectory("save");
            foreach (var file in saveDirectory.EnumerateFilesRecursive())
            {
                await targetDirectory.CopyFromAsync(file);
            }
        }

        public override Task RestoreSaveGame(SaveGame targetDirectory)
        {
            
            throw new NotImplementedException();
        }

        public override CancellationToken RunGame()
        {
            throw new NotImplementedException();
        }

        public override Task SetupEnvironment()
        {
            throw new NotImplementedException();
        }

        public async override Task CompileConfiguration()
        {
            var serializer = new SimpleCfgConfigurationSerializer();
            var tokenizer = new ConfigurationTraversalContext();
            var config = this.Game.WithConfigurations()
                .GetProfile<HiganRetroArchConfiguration>(nameof(HiganGameEmulation),
                this.ConfigurationProfile).Configuration;

            var node = tokenizer.TraverseCollection(config);
            var retroArchNode = node["#retroarch"];
            StringBuilder configContents = new StringBuilder();

            configContents.Append(serializer.Transform(retroArchNode));

            var configFile = this.Scratch.OpenDirectory("config")
                    .OpenFile("retroarch.cfg");

            foreach (var port in this.ControllerPorts)
            {
                var mappings = this.InputMappings[port.PhysicalDeviceInstance.Driver];
                var template = new InputTemplate<RetroPadTemplate>(port.LayoutMapping, port.PortIndex);

                template.Template.Configuration.InputJoypadIndex = port.PhysicalDeviceInstance.EnumerationIndex;

                var inputNode = tokenizer.TraverseInputTemplate(template, mappings, port.PortIndex);
                configContents.Append(serializer.Transform(retroArchNode));
            }
            await configFile.WriteAllTextAsync(configContents.ToString());
        }

        protected override void TeardownGame()
        {
            throw new NotImplementedException();
        }
    }
}
