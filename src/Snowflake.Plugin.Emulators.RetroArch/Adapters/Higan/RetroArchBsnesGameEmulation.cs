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
using System.Linq;
using Snowflake.Orchestration.Process;
using System.Diagnostics;
using System.Threading.Tasks.Sources;

#nullable enable

namespace Snowflake.Adapters.Higan
{
    public class RetroArchBsnesGameEmulation : GameEmulation
    {
        public RetroArchBsnesGameEmulation(IGame game,
            string configurationProfile,
            IEnumerable<IEmulatedController> controllerPorts,
            ISaveGame? initialSave,
            IDictionary<InputDriverType, IDeviceInputMapping> inputMappings,
            IEmulatorExecutable retroarchExecutable) : base(game, configurationProfile, controllerPorts, initialSave)
        {
            this.InputMappings = inputMappings;
            this.Executable = retroarchExecutable;
            this.Scratch = this.Game.WithFiles().GetRuntimeLocation();
        }

        public IDictionary<InputDriverType, IDeviceInputMapping> InputMappings { get; }
        public IEmulatorExecutable Executable { get; }
        private IDirectory Scratch { get; }

        private CancellationTokenSource? ProcessCancellationSource { get; set; }
        private Process? RunningProcess { get; set; }

        public override Task<ISaveGame> PersistSaveGame()
        {
            var tags = this.InitialSave?.Tags ?? Enumerable.Empty<string>();
            return this.Game.WithFiles().WithSaves().CreateSave("sram", tags, async targetDirectory =>
            {
                var saveDirectory = this.Scratch.OpenDirectory("save");
                foreach (var file in saveDirectory.EnumerateFilesRecursive())
                {
                    await targetDirectory.CopyFromAsync(file);
                }
            });
        }

        public override async Task RestoreSaveGame()
        {
            if (this.InitialSave == null) return;
            var saveDirectory = this.Scratch.OpenDirectory("save");
            foreach (var file in this.InitialSave.SaveContents.EnumerateFilesRecursive())
            {
                await saveDirectory.CopyFromAsync(file);
            }
        }

        public override CancellationToken StartEmulation()
        {
            if (this.ProcessCancellationSource != null) return this.ProcessCancellationSource.Token;
            var processBuilder = this.Executable.GetProcessBuilder();
            // todo: setup options here

            var psi = processBuilder.ToProcessStartInfo();
            this.RunningProcess = Process.Start(psi);
            
            this.ProcessCancellationSource = new CancellationTokenSource();
            this.RunningProcess.EnableRaisingEvents = true;
            
            this.RunningProcess.Exited += (s, e) => this.ProcessCancellationSource.Cancel();
            return this.ProcessCancellationSource.Token;
        }

        public override async Task SetupEnvironment()
        {
            // todo: copy BIOS files to env
        }

        public async override Task CompileConfiguration()
        {
            var serializer = new SimpleCfgConfigurationSerializer();
            var tokenizer = new ConfigurationTraversalContext();
            var configProfile = this.Game.WithConfigurations()
                .GetProfile<HiganRetroArchConfiguration>(nameof(RetroArchBsnesGameEmulation),
                this.ConfigurationProfile);

            if (configProfile == null)
            {
                configProfile = this.Game.WithConfigurations()
                    .CreateNewProfile<HiganRetroArchConfiguration>
                    (nameof(RetroArchBsnesGameEmulation), this.ConfigurationProfile);
            }
                
            var config = configProfile.Configuration;

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

        protected override async Task TeardownGame()
        {
            this.Scratch.Delete();
        } 

        public override async Task StopEmulation()
        {
            if (this.RunningProcess == null) return;
            using var controller = new RetroArchNetworkController();
            await controller.Quit();
            await this.RunningProcess.WaitForExitAsync(new CancellationTokenSource(500).Token);
            this.RunningProcess.Kill(true);
        }
    }
}
