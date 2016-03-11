using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Snowflake.Game;
using Snowflake.Utility;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationFlagStore : IConfigurationFlagStore
    {
        public string EmulatorBridgeID { get; }
        readonly string configurationFlagLocation;
        private readonly ISimpleKeyValueStore backingStore;
        readonly IEmulatorBridge emulatorBridge;
        public ConfigurationFlagStore(IEmulatorBridge emulatorBridge)
        {
            this.backingStore = new SqliteKeyValueStore(Path.Combine(emulatorBridge.PluginDataPath, "flagscache.db"));
            this.EmulatorBridgeID = emulatorBridge.PluginName;
            this.configurationFlagLocation = Path.Combine(emulatorBridge.PluginDataPath, "flagscache.db");
            this.AddDefaults(emulatorBridge.ConfigurationFlags);
            this.emulatorBridge = emulatorBridge;
        }

        public void AddGame(IGameInfo gameInfo, IDictionary<string, string> flagValues)
        {
            this.backingStore.InsertObjects(flagValues
                .ToDictionary(flagPair => $"{gameInfo.UUID}-{flagPair.Key}",
                flagPair => flagPair.Value));
        }
        public void AddGame(IGameInfo gameInfo)
        {
            IDictionary<string, string> flagValues = new Dictionary<string, string>();
            this.AddGame(gameInfo, flagValues);
        }
        public dynamic GetValue(IGameInfo gameInfo, string key, ConfigurationFlagTypes type)
        {
            return this.GetValue(key, type, gameInfo.UUID, this.GetDefaultValue(key, type));
        }
        public void SetValue(IGameInfo gameInfo, string key, object value, ConfigurationFlagTypes type)
        {
            this.SetValue(key, value, type, gameInfo.UUID);
        }
        public dynamic GetDefaultValue(string key, ConfigurationFlagTypes type)
        {
            return this.GetValue(key, type, "default", this.emulatorBridge.ConfigurationFlags[key].DefaultValue);
        }
        public void SetDefaultValue(string key, object value, ConfigurationFlagTypes type)
        {
            this.SetValue(key, value, type, "default");
        }
        private void AddDefaults(IDictionary<string, IConfigurationFlag> configurationFlags)
        {
            IDictionary<string, string> flagValues = configurationFlags
                .ToDictionary(flag => flag.Key, flag => flag.Value.DefaultValue);

            this.backingStore.InsertObjects(flagValues.ToDictionary(flagPair => $"default-{flagPair.Key}", 
                flagPair => flagPair.Value));
        }
        private void SetValue(string key, object value, ConfigurationFlagTypes type, string prefix)
        {
            switch (type)
            {
                case ConfigurationFlagTypes.BOOLEAN_FLAG:
                    this.backingStore.InsertObject($"{prefix}-{key}", (bool)value);
                    break;
                case ConfigurationFlagTypes.INTEGER_FLAG:
                    this.backingStore.InsertObject($"{prefix}-{key}", (int)value);
                    break;
                case ConfigurationFlagTypes.SELECT_FLAG:
                    this.backingStore.InsertObject($"{prefix}-{key}", (int)value);
                    break;
            }
        }

        private dynamic GetValue(string key, ConfigurationFlagTypes type, string prefix, object fallback)
        {
            switch (type)
            {
                case ConfigurationFlagTypes.BOOLEAN_FLAG:
                    return this.backingStore.GetObject<bool>($"{prefix}-{key}");
                case ConfigurationFlagTypes.INTEGER_FLAG:
                    return this.backingStore.GetObject<int>($"{prefix}-{key}");
                case ConfigurationFlagTypes.SELECT_FLAG:
                    return this.backingStore.GetObject<int>($"{prefix}-{key}");
                default:
                    return fallback;
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
    }
}
