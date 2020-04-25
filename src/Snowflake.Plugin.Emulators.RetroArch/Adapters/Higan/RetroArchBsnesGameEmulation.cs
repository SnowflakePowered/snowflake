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
using Snowflake.Configuration;

#nullable enable

namespace Snowflake.Adapters.Higan
{
    public class RetroArchBsnesGameEmulation : GameEmulation<HiganRetroArchConfiguration>
    {
        public RetroArchBsnesGameEmulation(IGame game,
            HiganRetroArchConfiguration configurationProfile,
            IEnumerable<IEmulatedController> controllerPorts,
            ISaveProfile saveProfile,
            IDictionary<InputDriver, IDeviceInputMapping> inputMappings,
            IEmulatorExecutable retroarchExecutable) : base(game, configurationProfile, controllerPorts, saveProfile)
        {
            this.InputMappings = inputMappings;
            this.Executable = retroarchExecutable;
            this.Scratch = this.Game.WithFiles().GetRuntimeLocation();
        }

        public IDictionary<InputDriver, IDeviceInputMapping> InputMappings { get; }
        public IEmulatorExecutable Executable { get; }
        private IDirectory Scratch { get; }

        private CancellationTokenSource? ProcessCancellationSource { get; set; }
        private Process? RunningProcess { get; set; }
        public override Task<ISaveGame> PersistSaveGame()
        {
            return this.SaveProfile.CreateSave(this.Scratch.OpenDirectory("save"));
        }

        public override async Task RestoreSaveGame()
        {
            // todo: fix
            var saveDirectory = this.Scratch.OpenDirectory("save");
            var headSave = this.SaveProfile.GetHeadSave();
            if (headSave != null)
            {
                await headSave.ExtractSave(saveDirectory);
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

            this.RunningProcess.Exited += (s, e) =>
            {
                this.ProcessCancellationSource.Cancel();
            };

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
            
            var node = tokenizer.TraverseCollection(this.ConfigurationProfile);
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
            using var timeout = new CancellationTokenSource(500);
            await this.RunningProcess.WaitForExitAsync(timeout.Token);
            this.RunningProcess.Kill(true);
        }
    }
}
