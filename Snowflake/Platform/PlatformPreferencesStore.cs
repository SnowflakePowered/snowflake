using System.Collections.Generic;
using System.Linq;
using Snowflake.Emulator;
using Snowflake.Scraper;
using Snowflake.Service.Manager;
using Snowflake.Utility;

namespace Snowflake.Platform
{
    public class PlatformPreferencesStore : IPlatformPreferenceStore
    {
        private readonly IPluginManager pluginManager;
        private readonly ISimpleKeyValueStore backingValueStore;
        public PlatformPreferencesStore(string fileName, IPluginManager pluginManager)
        {
            this.pluginManager = pluginManager;
            this.backingValueStore = new SqliteKeyValueStore(fileName);
        }
      
        public void AddPlatform(IPlatformInfo platformInfo)
        {
            KeyValuePair<string, IEmulatorBridge> emulator = this.pluginManager.Get<IEmulatorBridge>().FirstOrDefault(x => x.Value.SupportedPlatforms.Contains(platformInfo.PlatformID));
            KeyValuePair<string, IScraper> scraper = this.pluginManager.Get<IScraper>().FirstOrDefault(x => x.Value.SupportedPlatforms.Contains(platformInfo.PlatformID));
            string emulatorId = emulator.Equals(default(KeyValuePair<string, IEmulatorBridge>)) ? "null" : emulator.Key;
            string scraperId = scraper.Equals(default(KeyValuePair<string, IScraper>)) ? "null" : scraper.Key;
            this.backingValueStore.InsertObjects( new Dictionary<string, string>() {
                { platformInfo.PlatformID + "_scraper", scraperId},
                { platformInfo.PlatformID + "_emulator", emulatorId}
            });
        }
        public IPlatformDefaults GetPreferences(IPlatformInfo platformInfo)
        {
            IDictionary<string, string> preferences = this.backingValueStore.GetObjects<string>(
            new List<string>()
            {
                platformInfo.PlatformID + "_scraper",
                platformInfo.PlatformID + "_emulator"
            });

            return new PlatformDefaults(preferences[platformInfo.PlatformID + "_scraper"],
                preferences[platformInfo.PlatformID + "_emulator"]);
        }
        public void SetEmulator(IPlatformInfo platformInfo, string value)
        {
            this.backingValueStore.InsertObject(platformInfo.PlatformID + "_emulator", value);
        }
        public void SetScraper(IPlatformInfo platformInfo, string value)
        {
            this.backingValueStore.InsertObject(platformInfo.PlatformID + "_scraper", value);
        }
    }
}
