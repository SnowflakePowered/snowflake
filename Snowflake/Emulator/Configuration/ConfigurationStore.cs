using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Snowflake.Game;

namespace Snowflake.Emulator.Configuration
{
    //new config store per config
    public class ConfigurationStore : IConfigurationStore
    {
        public string TemplateID { get; }
        public IConfigurationProfile DefaultProfile
        {
            get
            {                
                return ConfigurationProfile.FromJsonProtoTemplate(JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(Path.Combine(this.ConfigurationStorePath, ".default"))));
            }
            private set
            {
                File.WriteAllText(Path.Combine(this.ConfigurationStorePath, ".default"), JsonConvert.SerializeObject(value));
            }
        }
        public string ConfigurationStorePath { get; }
        private ConfigurationStore(string configurationstoreRoot, IConfigurationProfile defaultProfile)
        {
            if (!Directory.Exists(configurationstoreRoot)) Directory.CreateDirectory(configurationstoreRoot);
            if (!Directory.Exists(Path.Combine(configurationstoreRoot, defaultProfile.TemplateID))) Directory.CreateDirectory(Path.Combine(configurationstoreRoot, defaultProfile.TemplateID));
            this.ConfigurationStorePath = Path.Combine(configurationstoreRoot, defaultProfile.TemplateID);
            if(!File.Exists(Path.Combine(this.ConfigurationStorePath, ".default"))) this.DefaultProfile = defaultProfile;
            this.TemplateID = defaultProfile.TemplateID;
        }

        public bool ContainsFilename(IGameInfo gameInfo)
        {
            return File.Exists(Path.Combine(this.ConfigurationStorePath, $"{gameInfo.FileName}.json"));
        }
        public bool ContainsCRC32(IGameInfo gameInfo)
        {
            return File.Exists(Path.Combine(this.ConfigurationStorePath, $"{gameInfo.CRC32}.json"));
        }

        public bool Contains(IGameInfo gameInfo)
        {
            return (this.ContainsFilename(gameInfo) || this.ContainsCRC32(gameInfo));
        }
        public IConfigurationProfile GetConfigurationProfile(IGameInfo gameInfo)
        {
            if (!this.Contains(gameInfo))
            {
                return this.DefaultProfile;
            }
            string fileName = this.ContainsFilename(gameInfo) ? Path.Combine(this.ConfigurationStorePath, $"{gameInfo.FileName}.json") : Path.Combine(this.ConfigurationStorePath, $"{gameInfo.CRC32}.json");
            return ConfigurationProfile.FromJsonProtoTemplate(JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(fileName)));
        }
        public IConfigurationProfile this[IGameInfo gameInfo] => this.GetConfigurationProfile(gameInfo);

        public ConfigurationStore(IConfigurationProfile defaultProfile) : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "configurationstores"), defaultProfile) { }

    }
}
