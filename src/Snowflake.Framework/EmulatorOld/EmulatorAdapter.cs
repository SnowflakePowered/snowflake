using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Services;

namespace Snowflake.EmulatorOld
{
    public abstract class EmulatorAdapter : ProvisionedPlugin, IEmulatorAdapter
    {
        /// <inheritdoc/>
        public IEnumerable<IInputMapping> InputMappings { get; }

        /// <inheritdoc/>
        public IEnumerable<string> Capabilities { get; }

        /// <inheritdoc/>
        public IEnumerable<string> Mimetypes { get; }

        /// <inheritdoc/>
        public ISaveManager SaveManager { get; }

        /// <inheritdoc/>
        public IBiosManager BiosManager { get; }

        /// <inheritdoc/>
        public string SaveType { get; }

        /// <inheritdoc/>
        public IEnumerable<string> RequiredBios { get; }

        /// <inheritdoc/>
        public IEnumerable<string> OptionalBios { get; }

        protected IConfigurationCollectionStore CollectionStore { get; }

        protected IStoneProvider StoneProvider { get; }

        protected EmulatorAdapter(IPluginProvision provision,
            IStoneProvider stoneProvider,
            IConfigurationCollectionStore collectionStore,
            IBiosManager biosManager,
            ISaveManager saveManager)
            : base(provision)
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

        /// <inheritdoc/>
        public abstract IEmulatorInstance Instantiate(IGameRecord gameRecord, IFileRecord romFile, int saveSlot, IList<IEmulatedPort> ports);

        /// <inheritdoc/>
        public abstract IConfigurationCollection GetConfiguration(IGameRecord gameRecord, string profileName = "default");

        /// <inheritdoc/>
        public abstract IConfigurationCollection GetConfiguration();
    }
}
