using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Input.Device;
using Snowflake.Plugin.EmulatorAdapter.RetroArch;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Input;
using Snowflake.Records.Game;
using Snowflake.Emulator;
using Snowflake.Platform;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch
{
    internal class RetroArchInstance : EmulatorInstance
    {
        private RetroArchCommonAdapter adapter;
        internal RetroArchInstance(IGameRecord game, RetroArchCommonAdapter adapter, int saveSlot,
            IPlatformInfo platform,
            IList<IEmulatedPort> controllerPorts,
            IDictionary<string, IConfigurationCollection> configurationCollections)
            : base(null, game, saveSlot, platform, controllerPorts, configurationCollections)
        {
            this.adapter = adapter;
        }


        public override void Create()
        {
            //do configuration here

            var retroArchConfiguration = (RetroArchConfiguration)this.ConfigurationCollections["retroarch.cfg"];
            retroArchConfiguration.DirectoryConfiguration.SavefileDirectory =
                this.adapter.SaveManager.GetSaveDirectory(this.adapter.SaveType, this.Game.Guid, this.SaveSlot);
            retroArchConfiguration.DirectoryConfiguration.SystemDirectory =
                this.adapter.BiosManager.GetBiosDirectory(this.Platform);

            //build the configuration
            var sectionBuilder = new StringBuilder();

            foreach (var section in retroArchConfiguration)
            {
                sectionBuilder.Append(retroArchConfiguration.Serializer.Serialize(section));
            }

            //handle input config
            foreach (var port in this.ControllerPorts)
            {
                var inputTemplate = new RetroPadTemplate();
                inputTemplate.SetInputValues(port.MappedElementCollection, port.PluggedDevice, port.EmulatedPortNumber);
                var mappings = (from inputMappings in adapter.InputMappings
                    where inputMappings.InputApi == InputApi.DirectInput
                    where inputMappings.DeviceLayouts.Contains(port.PluggedDevice.DeviceLayout.LayoutID)
                    select inputMappings).FirstOrDefault();
                if (mappings == null) throw new InvalidOperationException("Adapter does not support device layout");
                var inputConfig = retroArchConfiguration.Serializer.Serialize(inputTemplate, mappings);
                sectionBuilder.Append(inputConfig);
            }
            File.WriteAllText(Path.Combine(this.InstancePath, retroArchConfiguration.FileName), sectionBuilder.ToString());

            //debug
            Console.WriteLine(Path.Combine(this.InstancePath, retroArchConfiguration.FileName));
            //start retroarchexe here with instacnepath as working directory
        }

        public override void Start()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override DateTime StartTime { get; }
        public override DateTime DestroyTime { get; }
    }
}
