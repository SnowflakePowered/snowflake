using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Snowflake.Game;
using Newtonsoft.Json;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationFlagStore : IConfigurationFlagStore
    {
        public string EmulatorBridgeID { get; private set; }
        readonly string configurationFlagLocation;

        public ConfigurationFlagStore(IEmulatorBridge emulatorBridge)
        {
            this.EmulatorBridgeID = emulatorBridge.PluginName;
            this.configurationFlagLocation = Path.Combine(emulatorBridge.PluginDataPath, "flagscache");
            if (!Directory.Exists(configurationFlagLocation)) Directory.CreateDirectory(configurationFlagLocation);
        }
        public void AddGame(IGameInfo gameInfo, IDictionary<string, string> flagValues)
        {
            File.WriteAllText(this.GetCacheFileName(gameInfo), JsonConvert.SerializeObject(flagValues));
        }
        public dynamic GetValue(IGameInfo gameInfo, string key, ConfigurationFlagTypes type)
        {
            string value = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(this.GetCacheFileName(gameInfo)))[key];
            switch (type)
            {
                case ConfigurationFlagTypes.SELECT_FLAG:
                    return value;
                case ConfigurationFlagTypes.INTEGER_FLAG:
                    return Int32.Parse(value);
                case ConfigurationFlagTypes.BOOLEAN_FLAG:
                    return Boolean.Parse(value);
                default:
                    return value;
            }
        }
        public void SetValue(IGameInfo gameInfo, string key, object value, ConfigurationFlagTypes type)
        {
            IDictionary<string, string> keys = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(this.GetCacheFileName(gameInfo)));
            keys.Add(key, value.ToString());
        }
        public dynamic this[IGameInfo gameInfo, string key, ConfigurationFlagTypes type]
        {
            get
            {
                return this.GetValue(gameInfo, key, type);
            }
            set
            {
                this.SetValue(gameInfo, key, value, type);
            }
        }
        private string GetCacheFileName(IGameInfo gameInfo)
        {
            return Path.Combine(this.configurationFlagLocation, String.Format("{0}.{1}.cfg", gameInfo.UUID, this.EmulatorBridgeID));
        }
    }
}
