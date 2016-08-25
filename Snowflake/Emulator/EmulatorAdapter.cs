using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Input.Hotkey;
using Snowflake.Extensibility;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Service;

namespace Snowflake.Emulator
{
    public abstract class EmulatorAdapter : Plugin
    {
        public IEnumerable<IInputMapping> InputMappings { get; }
        public abstract IEnumerable<IConfigurationCollection> DefaultConfigurations { get; }
        public abstract IHotkeyTemplateCollection DefaultHotkeys { get; }
        public IEnumerable<string> Capabilities { get; }
        public IEnumerable<string> Mimetypes { get; }
        protected IConfigurationCollectionStore CollectionStore { get; }
        public ISaveManager SaveManager { get; }
        public IBiosManager BiosManager { get; }
        protected IStoneProvider StoneProvider { get; }
        public string SaveType { get; }
        protected EmulatorAdapter(string appDataDirectory,
            IStoneProvider stoneProvider, 
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager,
            ISaveManager saveManager) : base(appDataDirectory)
        {
            this.StoneProvider = stoneProvider;
            this.InputMappings = 
                this.GetAllSiblingResourceNames("InputMappings")
                .Select(mappings => JsonConvert.DeserializeObject<InputMapping>
                (this.GetSiblingStringResource("InputMappings", mappings))).Cast<IInputMapping>().ToList();
            this.Capabilities = this.PluginProperties.GetEnumerable("capabilities")?.ToList();
            this.Mimetypes = this.PluginProperties.GetEnumerable("mimetypes")?.ToList();
            this.SaveType = this.PluginProperties.Get("savetype");
            this.CollectionStore = collectionStore;
            this.BiosManager = biosManager;
            this.SaveManager = saveManager;

        }

        public abstract IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord romFile, int saveSlot, IList<IEmulatedPort> ports);

        [Obsolete("Debug Purposes Only(!!)")]
        public abstract IEmulatorInstance Instantiate(IGameRecord gameRecord, ICoreService coreService);

    }
}
