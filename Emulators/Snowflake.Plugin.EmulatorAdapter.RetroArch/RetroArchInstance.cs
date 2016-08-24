using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Input.Device;
using Snowflake.Plugin.EmulatorAdapter.RetroArch;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Input;
using Snowflake.Records.Game;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Executable;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch
{
    internal class RetroArchInstance : EmulatorInstance
    {
        private readonly RetroArchCommonAdapter adapter;
        private readonly RetroArchProcessHandler processHandler;
        internal RetroArchInstance(IGameRecord game, IFileRecord file, 
            RetroArchCommonAdapter adapter, 
            RetroArchProcessHandler processHandler, int saveSlot,
            IPlatformInfo platform,
            IList<IEmulatedPort> controllerPorts,
            IDictionary<string, IConfigurationCollection> configurationCollections)
            : base(null, game, file, saveSlot, platform, controllerPorts, configurationCollections)
        {
            this.adapter = adapter;
            this.processHandler = processHandler;
        }

        private string BuildRetroarchCfg(RetroArchConfiguration retroArchConfiguration)
        {
            //build the configuration
            var sectionBuilder = new StringBuilder(retroArchConfiguration.ToString());

            //handle input config
            foreach (var port in this.ControllerPorts)
            {
                var inputTemplate = new RetroPadTemplate();
                inputTemplate.SetInputValues(port.MappedElementCollection, port.PluggedDevice, port.EmulatedPortNumber);
                var mappings = (from inputMappings in this.adapter.InputMappings
                                where inputMappings.InputApi == InputApi.DirectInput
                                where inputMappings.DeviceLayouts.Contains(port.PluggedDevice.DeviceLayout.LayoutID)
                                select inputMappings).FirstOrDefault();
                if (mappings == null) throw new InvalidOperationException("Adapter does not support device layout");
                //serialize the input template
                var inputConfig = retroArchConfiguration.Serializer.Serialize(inputTemplate, mappings);
                sectionBuilder.Append(inputConfig);
            }
            return sectionBuilder.ToString();
        }

        public override void Create()
        {
            //do configuration here

            var retroArchConfiguration = (RetroArchConfiguration)this.ConfigurationCollections["retroarch.cfg"];
            retroArchConfiguration.DirectoryConfiguration.SavefileDirectory =
                this.adapter.SaveManager.GetSaveDirectory(this.adapter.SaveType, this.Game.Guid, this.SaveSlot);
            retroArchConfiguration.DirectoryConfiguration.SystemDirectory =
                this.adapter.BiosManager.GetBiosDirectory(this.Platform);
            retroArchConfiguration.DirectoryConfiguration.CoreOptionsPath = Path.Combine(this.InstancePath,
                "retroarch-core-options.cfg");

            string retroarchCfg = this.BuildRetroarchCfg(retroArchConfiguration);
            
            //output to the filename
            File.WriteAllText(Path.Combine(this.InstancePath, retroArchConfiguration.FileName), retroarchCfg);

            if (this.ConfigurationCollections.ContainsKey("retroarch-core-options.cfg"))
            {
                var coreConfig = this.ConfigurationCollections["retroarch-core-options.cfg"];
                File.WriteAllText(Path.Combine(this.InstancePath, "retroarch-core-options.cfg"), coreConfig.ToString());
            }

            //debug
            Console.WriteLine(Path.Combine(this.InstancePath, retroArchConfiguration.FileName));
            
            //complete.
            this.CreateTime = DateTimeOffset.UtcNow;
            this.IsCreated = true;
        }

        public override void Start()
        {
            var info = this.processHandler.GetProcessInfo(this.RomFile.FilePath);
            info.CorePath = this.adapter.CorePath;
            info.ConfigPath = Path.Combine(this.InstancePath, "retroarch.cfg");
            Process.Start(info.GetStartInfo());
            //setup the instance hereee
        }

        public override void Pause()
        {
            throw new NotImplementedException(); 
        }

        public override void Resume()
        {
            throw new NotImplementedException();
        }

        public override void Destroy()
        {
            this.IsActive = false;
            this.IsRunning = false;
            this.IsCreated = false;
            Directory.Delete(this.InstancePath, true);
            this.DestroyTime = DateTimeOffset.UtcNow;
            this.IsDestroyed = true;
           
        }

        public override DateTimeOffset CreateTime { get; protected set; }
        public override DateTimeOffset StartTime { get; protected set; }
        public override DateTimeOffset DestroyTime { get; protected set; }
    }
}
