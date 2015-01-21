using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Snowflake.Game;
using Newtonsoft.Json;
using Snowflake.Emulator.Configuration;

namespace Snowflake.Emulator.Configuration
{
    //new config store per config
    public class ConfigurationStore : IConfigurationStore
    {
        public string TemplateID { get; private set; }
        public IConfigurationProfile DefaultProfile { get; private set; }
        public string ConfigurationStorePath { get; private set; }
        private ConfigurationStore(string configurationstoreRoot, IConfigurationProfile defaultProfile)
        {
            if (!Directory.Exists(configurationstoreRoot)) Directory.CreateDirectory(configurationstoreRoot);
            if (!Directory.Exists(Path.Combine(configurationstoreRoot, defaultProfile.TemplateID))) Directory.CreateDirectory(Path.Combine(configurationstoreRoot, defaultProfile.TemplateID));
            this.ConfigurationStorePath = Path.Combine(configurationstoreRoot, defaultProfile.TemplateID);
            this.DefaultProfile = defaultProfile;
            this.TemplateID = defaultProfile.TemplateID;
        }

        public bool ContainsFilename(IGameInfo gameInfo)
        {
            return File.Exists(Path.Combine(this.ConfigurationStorePath, gameInfo.FileName + ".json"));
        }
        public bool ContainsCRC32(IGameInfo gameInfo)
        {
            return File.Exists(Path.Combine(this.ConfigurationStorePath, gameInfo.CRC32 +  ".json"));
        }

        public bool Contains(IGameInfo gameInfo)
        {
            return (ContainsFilename(gameInfo) || ContainsCRC32(gameInfo));
        }
        public IConfigurationProfile GetConfigurationProfile(IGameInfo gameInfo)
        {
            if (!this.Contains(gameInfo))
            {
                return this.DefaultProfile;
            }
            else
            {
                string fileName = ContainsFilename(gameInfo) ? Path.Combine(this.ConfigurationStorePath, gameInfo.FileName + ".json") : Path.Combine(this.ConfigurationStorePath, gameInfo.CRC32 + ".json");
                return ConfigurationProfile.FromJsonProtoTemplate(JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(fileName)));
            }
        }
        public IConfigurationProfile this[IGameInfo gameInfo]
        {
            get
            {
                return this.GetConfigurationProfile(gameInfo);
            }
        }

        public ConfigurationStore(ConfigurationProfile defaultProfile) : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "configurationstores"), defaultProfile) { }

    }
}
