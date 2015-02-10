using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Snowflake.Game;
using Newtonsoft.Json;
using Snowflake.Service;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationFlagStore : IConfigurationFlagStore
    {
        public string EmulatorBridgeID { get; private set; }
        readonly string configurationFlagLocation;
        readonly IEmulatorBridge emulatorBridge;
        public ConfigurationFlagStore(IEmulatorBridge emulatorBridge)
        {
            this.EmulatorBridgeID = emulatorBridge.PluginName;
            this.emulatorBridge = emulatorBridge;
            this.configurationFlagLocation = Path.Combine(emulatorBridge.PluginDataPath, "flagscache");
            if (!Directory.Exists(configurationFlagLocation)) Directory.CreateDirectory(configurationFlagLocation);
            this.AddDefaults(emulatorBridge.ConfigurationFlags);
        }
        private void AddDefaults(IDictionary<string, IConfigurationFlag> configurationFlags)
        {
            IDictionary<string, string> flagValues = configurationFlags.ToDictionary(flag => flag.Key, flag => flag.Value.DefaultValue);
            if (!File.Exists(this.GetDefaultFileName()))
            {
                File.WriteAllText(this.GetDefaultFileName(), JsonConvert.SerializeObject(flagValues));
            }
        }
        public void AddGame(IGameInfo gameInfo, IDictionary<string, string> flagValues)
        {
            if (!File.Exists(this.GetCacheFileName(gameInfo)))
            {
                File.WriteAllText(this.GetCacheFileName(gameInfo), JsonConvert.SerializeObject(flagValues));
            }
        }
        public void AddGame(IGameInfo gameInfo)
        {
            IDictionary<string, string> flagValues = new Dictionary<string, string>();
            this.AddGame(gameInfo, flagValues);
        }
        public dynamic GetValue(IGameInfo gameInfo, string key, ConfigurationFlagTypes type)
        {
            return this.GetValue(key, type, this.GetCacheFileName(gameInfo), this.GetDefaultValue(key, type));
        }
        public void SetValue(IGameInfo gameInfo, string key, object value, ConfigurationFlagTypes type)
        {
            this.SetValue(key, value, type, this.GetCacheFileName(gameInfo));
        }
        public dynamic GetDefaultValue(string key, ConfigurationFlagTypes type)
        {
            return this.GetValue(key, type, this.GetDefaultFileName(), this.emulatorBridge.ConfigurationFlags[key].DefaultValue);
        }
        public void SetDefaultValue(string key, object value, ConfigurationFlagTypes type)
        {
            this.SetValue(key, value, type, this.GetDefaultFileName());
        }
        private void SetValue(string key, object value, ConfigurationFlagTypes type, string filename)
        {
            IDictionary<string, string> keys = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(filename));
            keys[key] = value.ToString();
            File.WriteAllText(filename, JsonConvert.SerializeObject(keys));
        }
        private dynamic GetValue(string key, ConfigurationFlagTypes type, string filename, object fallback)
        {
            string value = String.Empty;
            try
            {
                value = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(filename))[key];
            }
            catch
            {
                value = fallback.ToString();
            }
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
        private string GetDefaultFileName()
        {
            return Path.Combine(this.configurationFlagLocation, String.Format("{0}.{1}.cfg", "default", this.EmulatorBridgeID));
        }
    }
}
