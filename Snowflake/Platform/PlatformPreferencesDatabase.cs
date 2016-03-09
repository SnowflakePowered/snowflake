using System.Collections.Generic;
using System.Linq;
using Snowflake.Emulator;
using Snowflake.Scraper;
using Snowflake.Service.Manager;
using Snowflake.Utility;

namespace Snowflake.Platform
{
    public class PlatformPreferencesDatabase : SynchronousAkavacheDatabase, IPlatformPreferenceDatabase
    {
        private readonly IPluginManager pluginManager;
        public PlatformPreferencesDatabase(string fileName, IPluginManager pluginManager)
            : base(fileName)
        {
            this.pluginManager = pluginManager;
        }
      
        public void AddPlatform(IPlatformInfo platformInfo)
        {
            KeyValuePair<string, IEmulatorBridge> emulator = this.pluginManager.Plugins<IEmulatorBridge>().FirstOrDefault(x => x.Value.SupportedPlatforms.Contains(platformInfo.PlatformID));
            KeyValuePair<string, IScraper> scraper = this.pluginManager.Plugins<IScraper>().FirstOrDefault(x => x.Value.SupportedPlatforms.Contains(platformInfo.PlatformID));
            string emulatorId = emulator.Equals(default(KeyValuePair<string, IEmulatorBridge>)) ? "null" : emulator.Key;
            string scraperId = scraper.Equals(default(KeyValuePair<string, IScraper>)) ? "null" : scraper.Key;
            this.InsertObjects( new Dictionary<string, string>() {
                { platformInfo.PlatformID + "_scraper", scraperId},
                { platformInfo.PlatformID + "_emulator", emulatorId}
            });
        }
        public IPlatformDefaults GetPreferences(IPlatformInfo platformInfo)
        {
            IDictionary<string, string> preferences = this.GetObjects<string>(new List<string>()
            {
                platformInfo.PlatformID + "_scraper",
                platformInfo.PlatformID + "_emulator"
            });

            return new PlatformDefaults(preferences[platformInfo.PlatformID + "_scraper"],
                preferences[platformInfo.PlatformID + "_emulator"]);
        }
        public void SetEmulator(IPlatformInfo platformInfo, string value)
        {
            this.InsertObject(platformInfo.PlatformID + "_emulator", value);
        }
        public void SetScraper(IPlatformInfo platformInfo, string value)
        {
            this.InsertObject(platformInfo.PlatformID + "_scraper", value);
        }
    }
}
