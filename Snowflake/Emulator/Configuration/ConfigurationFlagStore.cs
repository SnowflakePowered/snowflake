using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Snowflake.Game;
using Newtonsoft.Json;
using Snowflake.Service;
using Snowflake.Utility;
using System.Data.SQLite;
namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationFlagStore : BaseDatabase, IConfigurationFlagStore
    {
        public string EmulatorBridgeID { get; private set; }
        readonly string configurationFlagLocation;
        readonly IEmulatorBridge emulatorBridge;
        public ConfigurationFlagStore(IEmulatorBridge emulatorBridge)
            : base(Path.Combine(emulatorBridge.PluginDataPath, "flagscache.db"))
        {
            this.CreateDatabase();
            this.EmulatorBridgeID = emulatorBridge.PluginName;
            this.configurationFlagLocation = Path.Combine(emulatorBridge.PluginDataPath, "flagscache.db");
            this.AddDefaults(emulatorBridge.ConfigurationFlags);
            this.emulatorBridge = emulatorBridge;
        }
        private void CreateDatabase()
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS flags(
                                                                flagKey TEXT PRIMARY KEY,
                                                                flagValue TEXT)", dbConnection);
            sqlCommand.ExecuteNonQuery();
            dbConnection.Close();
        }
        public void AddGame(IGameInfo gameInfo, IDictionary<string, string> flagValues)
        {


            var flagDb = this.GetConnection();
            flagDb.Open();
            foreach (KeyValuePair<string, string> flagPair in flagValues)
            {
                using (var sqliteCommand = new SQLiteCommand(@"INSERT OR REPLACE INTO flags VALUES(
                                          @flagKey,
                                          @flagValue)", flagDb))
                {
                    sqliteCommand.Parameters.AddWithValue("@flagKey", gameInfo.UUID + "-" + flagPair.Key);
                    sqliteCommand.Parameters.AddWithValue("@flagValue", flagPair.Value.ToString());
                    sqliteCommand.ExecuteNonQuery();
                }
            }
            flagDb.Close();
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
            IDictionary<string, string> flagValues = configurationFlags.ToDictionary(flag => flag.Key, flag => flag.Value.DefaultValue);
            var flagDb = this.GetConnection();
            flagDb.Open();
            foreach (KeyValuePair<string, string> flagPair in flagValues)
            {
               using(var sqliteCommand = new SQLiteCommand(@"INSERT OR REPLACE INTO flags VALUES(
                                          @flagKey,
                                          @flagValue)", flagDb)){
                   sqliteCommand.Parameters.AddWithValue("@flagKey", "default-"+flagPair.Key);
                   sqliteCommand.Parameters.AddWithValue("@flagValue", flagPair.Value.ToString());
                   sqliteCommand.ExecuteNonQuery();
               }
            }
            flagDb.Close();
            

        }
        private void SetValue(string key, object value, ConfigurationFlagTypes type, string prefix)
        {
            var flagDb = this.GetConnection();
            flagDb.Open();
            using (var sqliteCommand = new SQLiteCommand(@"INSERT OR REPLACE INTO flags VALUES(
                                          @flagKey,
                                          @flagValue)", flagDb))
            {
                Console.WriteLine("Setting value " + value + " for key " + key + " for prefix " + prefix);
                sqliteCommand.Parameters.AddWithValue("@flagKey", prefix + "-" + key);
                sqliteCommand.Parameters.AddWithValue("@flagValue", value.ToString());
                sqliteCommand.ExecuteNonQuery();
            }
            Console.WriteLine("Commit value " + value + " for key " + key + " for prefix " + prefix);
            flagDb.Close();
            
        }
        private dynamic GetValue(string key, ConfigurationFlagTypes type, string prefix, object fallback)
        {
            var dbConnection = this.GetConnection();
            dbConnection.Open();
            string value = String.Empty;
            using (var sqlCommand = new SQLiteCommand(@"SELECT `flagValue` FROM `flags` WHERE `flagKey` == @searchQuery"
               , dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@searchQuery", prefix + "-" + key);
               
                    try
                    {
                        value = (string)sqlCommand.ExecuteScalar();
                    }
                    catch (AccessViolationException)
                    {
                        System.Threading.Thread.Sleep(500); //le concurrency hack :(
                        value = (string)sqlCommand.ExecuteScalar();
                    }
                    Console.WriteLine("Received value " + value + " for key " + key + " for prefix " + prefix);
            }
            dbConnection.Close();
            if (value == null)
            {
                value = fallback.ToString();
            }
            Console.WriteLine("Final value " + value + " for key " + key + " for prefix " + prefix);

            switch (type)
            {
                case ConfigurationFlagTypes.SELECT_FLAG:
                    return Int32.Parse(value);
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
    }
}
