using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Hotkey;
using Snowflake.Configuration.Input;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Service;

namespace Snowflake.Emulator
{
   
    public abstract class EmulatorAdapter : Plugin, IEmulatorAdapter
    {
        public IEnumerable<IInputMapping> InputMappings { get; }
        public IEnumerable<string> Capabilities { get; }
        public IEnumerable<string> Mimetypes { get; }
        protected IConfigurationCollectionStore CollectionStore { get; }
        protected IHotkeyTemplateStore HotkeyTemplateStore { get; }
        public ISaveManager SaveManager { get; }
        public IBiosManager BiosManager { get; }
        protected IStoneProvider StoneProvider { get; }
        public string SaveType { get; }
        public IEnumerable<string> RequiredBios { get; }
        public IEnumerable<string> OptionalBios { get; }

        protected EmulatorAdapter(string appDataDirectory,
            IStoneProvider stoneProvider, 
            IConfigurationCollectionStore collectionStore,
            IHotkeyTemplateStore hotkeyTemplateStore,
            IBiosManager biosManager,
            ISaveManager saveManager) : base(appDataDirectory)
        {
            this.StoneProvider = stoneProvider;
            this.InputMappings = 
                this.GetAllSiblingResourceNames("InputMappings")
                .Select(mappings => JsonConvert.DeserializeObject<InputMapping>
                (this.GetSiblingStringResource("InputMappings", mappings))).Cast<IInputMapping>().ToList();
            this.Capabilities = this.PluginProperties.GetEnumerable("capabilities")?.ToList() ?? Enumerable.Empty<string>();
            this.Mimetypes = this.PluginProperties.GetEnumerable("mimetypes")?.ToList() ?? Enumerable.Empty<string>();
            this.SaveType = this.PluginProperties.Get("savetype");
            this.CollectionStore = collectionStore;
            this.BiosManager = biosManager;
            this.SaveManager = saveManager;
            this.OptionalBios = this.PluginProperties.GetEnumerable("optionalbios")?.ToList() ?? Enumerable.Empty<string>();
            this.RequiredBios = this.PluginProperties.GetEnumerable("requiredbios")?.ToList() ?? Enumerable.Empty<string>();
            this.HotkeyTemplateStore = hotkeyTemplateStore;
        }

        public abstract IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord romFile, int saveSlot, IList<IEmulatedPort> ports);

        public abstract IDictionary<string, IConfigurationCollection> GetConfigurations(IGameRecord gameRecord);

        public abstract IDictionary<string, IConfigurationCollection> GetDefaultConfigurations();

        public abstract IHotkeyTemplate GetHotkeyTemplate();

        public abstract IHotkeyTemplate GetDefaultHotkeyTemplate();

    }
}
