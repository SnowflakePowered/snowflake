using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Emulator;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Platform;
using Snowflake.Plugin.Emulators.RetroArch;
using Snowflake.Plugin.Emulators.RetroArch.Adapters;
using Snowflake.Plugin.Emulators.RetroArch.Configuration;
using Snowflake.Plugin.Emulators.RetroArch.Executable;
using Snowflake.Plugin.Emulators.RetroArch.Input;
using Snowflake.Plugin.Emulators.RetroArch.Selections.VideoConfiguration;
using Snowflake.Plugin.Emulators.RetroArch.Shaders;
using Snowflake.Records.File;
using Snowflake.Records.Game;

namespace Snowflake.Plugin.Emulators.RetroArch
{
    internal class RetroArchInstance : EmulatorInstance
    {
        private readonly RetroArchProcessHandler processHandler;
        private Process runningProcess;
        public ShaderManager ShaderManager { get; set; }
        protected string CorePath { get; set; }
        internal RetroArchInstance(IGameRecord game, IFileRecord file,
            RetroArchCommonAdapter adapter,
            string corePath,
            RetroArchProcessHandler processHandler, int saveSlot,
            IPlatformInfo platform,
            IList<IEmulatedPort> controllerPorts)
            : base(adapter, game, file, saveSlot, platform, controllerPorts)
        {
            this.processHandler = processHandler;
            this.CorePath = corePath;
        }

        protected IDictionary<string, string> BuildConfiguration(IConfigurationCollection<RetroArchConfiguration> retroArchConfiguration)
        {
            // build the configuration
            IDictionary<string, string> configurations = new Dictionary<string, string>();
            foreach (var output in retroArchConfiguration.Descriptor.Outputs.Values)
            {
                var sectionBuilder = new StringBuilder();
                var serializer = new KeyValuePairConfigurationSerializer(output.BooleanMapping, "nul", "=");
                foreach (var section in retroArchConfiguration.Where(c => retroArchConfiguration.Descriptor.GetDestination(c.Key) == output.Key))
                {
                    sectionBuilder.Append(serializer.Serialize(section.Value));
                }

                configurations[output.Key] = sectionBuilder.ToString();
            }

            var retroarchSerializer = new KeyValuePairConfigurationSerializer(retroArchConfiguration.Descriptor.Outputs["#retroarch"].BooleanMapping, "nul", "=");
            IInputSerializer inputSerializer = new InputSerializer(retroarchSerializer);

            // handle input config
            foreach (var port in this.ControllerPorts)
            {
                var inputTemplate = new InputTemplate<RetroPadTemplate>(port.MappedElementCollection, port.EmulatedPortNumber);
                var mappings = (from inputMappings in this.EmulatorAdapter.InputMappings
                                where inputMappings.InputApi == InputApi.DirectInput
                                where inputMappings.DeviceLayouts.Contains(port.PluggedDevice.DeviceLayout.LayoutID)
                                select inputMappings).FirstOrDefault();
                if (mappings == null)
                {
                    throw new InvalidOperationException("Adapter does not support device layout");
                }

                // serialize the input template
                var inputConfig = inputSerializer.Serialize(inputTemplate, mappings);
                configurations["#retroarch"] += inputConfig;
            }

            return configurations;
        }

        /// <inheritdoc/>
        public override void Create()
        {
            // do configuration here
            IConfigurationCollection<RetroArchConfiguration> retroArchConfiguration = this.Configuration as IConfigurationCollection<RetroArchConfiguration>;
            retroArchConfiguration.Configuration.DirectoryConfiguration.SavefileDirectory =
                this.EmulatorAdapter.SaveManager.GetSaveDirectory(this.EmulatorAdapter.SaveType, this.Game.Guid, this.SaveSlot);
            retroArchConfiguration.Configuration.DirectoryConfiguration.SystemDirectory =
                this.EmulatorAdapter.BiosManager.GetBiosDirectory(this.Platform);
            retroArchConfiguration.Configuration.DirectoryConfiguration.CoreOptionsPath = Path.Combine(this.InstancePath,
                "retroarch-core-options.cfg");
            if (retroArchConfiguration.Configuration.VideoConfiguration.VideoShaderEnable)
            {
                var selectedShader = typeof(RetroArchShader)
                .GetField(Enum.GetName(typeof(RetroArchShader), retroArchConfiguration.Configuration.VideoConfiguration.VideoShader))
                .GetCustomAttribute<ShaderAttribute>();
                if (retroArchConfiguration.Configuration.VideoConfiguration.VideoDriver == VideoDriver.Vulkan && selectedShader.SlangSupport)
                {
                    retroArchConfiguration.Configuration.VideoConfiguration.VideoShaderPath =
                        this.ShaderManager?.GetShaderPath(selectedShader.ShaderName, ShaderType.Slang);
                }
                else
                {
                    retroArchConfiguration.Configuration.VideoConfiguration.VideoShaderPath =
                        this.ShaderManager?.GetShaderPath(selectedShader.ShaderName, selectedShader.CgSupport ? ShaderType.Cg :
                        selectedShader.GlslSupport ? ShaderType.Glsl : ShaderType.Unknown);
                }
            }

            foreach (var cfg in this.BuildConfiguration(retroArchConfiguration))
            {
                File.WriteAllText(Path.Combine(this.InstancePath, this.Configuration.Descriptor.Outputs[cfg.Key].Destination), cfg.Value);
            }

            // string retroarchCfg = this.BuildRCon(retroArchConfiguration);

            // output to the filename
            /*  File.WriteAllText(Path.Combine(this.InstancePath, retroArchConfiguration.FileName), retroarchCfg);

              if (this.ConfigurationCollections.ContainsKey("retroarch-core-options.cfg"))
              {
                  var coreConfig = this.ConfigurationCollections["retroarch-core-options.cfg"];
              }

              //debug*/
            Console.WriteLine(Path.Combine(this.InstancePath));

             // complete.
            this.CreateTime = DateTimeOffset.UtcNow;
            this.IsCreated = true;
        }

        /// <inheritdoc/>
        public override void Start()
        {
            var info = this.processHandler.GetProcessInfo(this.RomFile.FilePath);
            info.Debug = true;
            info.CorePath = this.CorePath;
            info.ConfigPath = Path.Combine(this.InstancePath, "retroarch.cfg");
            Console.WriteLine(info.GetStartInfo().FileName + " " + info.GetStartInfo().Arguments);
            this.runningProcess = Process.Start(info.GetStartInfo());
            Task.Run(() => this.runningProcess.WaitForExit()).ContinueWith(x => this.Destroy());

            // setup the instance hereee
        }

        /// <inheritdoc/>
        public override void Pause()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Resume()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Destroy()
        {
            this.IsActive = false;
            this.IsRunning = false;
            this.IsCreated = false;
            if (!this.runningProcess.HasExited)
            {
                this.runningProcess.Dispose();
            }

            Directory.Delete(this.InstancePath, true);
            this.DestroyTime = DateTimeOffset.UtcNow;
            this.IsDestroyed = true;
        }

        /// <inheritdoc/>
        public override DateTimeOffset CreateTime { get; protected set; }

        /// <inheritdoc/>
        public override DateTimeOffset StartTime { get; protected set; }

        /// <inheritdoc/>
        public override DateTimeOffset DestroyTime { get; protected set; }
    }
}
