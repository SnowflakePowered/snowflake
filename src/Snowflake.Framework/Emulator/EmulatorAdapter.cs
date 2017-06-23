using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Services;
using System.IO;

namespace Snowflake.Emulator
{
   
    public abstract class EmulatorAdapter : Plugin, IEmulatorAdapter
    {
        public IEnumerable<IInputMapping> InputMappings { get; }
        public IEnumerable<string> Capabilities { get; }
        public IEnumerable<string> Mimetypes { get; }
        protected IConfigurationCollectionStore CollectionStore { get; }
        public ISaveManager SaveManager { get; }
        public IBiosManager BiosManager { get; }
        protected IStoneProvider StoneProvider { get; }
        public string SaveType { get; }
        public IEnumerable<string> RequiredBios { get; }
        public IEnumerable<string> OptionalBios { get; }

        protected EmulatorAdapter(IPluginProvision provision,
            IStoneProvider stoneProvider, 
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager,
            ISaveManager saveManager) : base(provision)
        {
            this.StoneProvider = stoneProvider;
            this.InputMappings = 
                this.Provision.CommonResourceDirectory.CreateSubdirectory("InputMappings").EnumerateFiles()
                .Select(mapping => JsonConvert.DeserializeObject<InputMapping>(File.ReadAllText(mapping.FullName)))
                .Cast<IInputMapping>().ToList();
            this.Capabilities = this.Provision.Properties.GetEnumerable("capabilities").ToList();
            this.Mimetypes = this.Provision.Properties.GetEnumerable("mimetypes").ToList();
            this.SaveType = this.Provision.Properties.Get("savetype");
            this.CollectionStore = collectionStore;
            this.BiosManager = biosManager;
            this.SaveManager = saveManager;
            this.OptionalBios = this.Provision.Properties.GetEnumerable("optionalbios").ToList();
            this.RequiredBios = this.Provision.Properties.GetEnumerable("requiredbios").ToList();
        }

        public abstract IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord romFile, int saveSlot, IList<IEmulatedPort> ports);

        public abstract IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default");

        public abstract IConfigurationCollection GetConfiguration();

    }
}
