using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Snowflake.Emulator;
using Snowflake.Scraper;
using Snowflake.Service.Manager;
using Snowflake.Utility;

namespace Snowflake.Platform
{
    public class PlatformPreferencesDatabase : BaseDatabase, IPlatformPreferenceDatabase
    {
        private readonly IPluginManager pluginManager;
        public PlatformPreferencesDatabase(string fileName, IPluginManager pluginManager)
            : base(fileName)
        {
            this.CreateDatabase();
            this.pluginManager = pluginManager;
        }
        private void CreateDatabase()
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS platformprefs(
                                                                platform_id TEXT PRIMARY KEY,
                                                                emulator TEXT,
                                                                scraper TEXT)", dbConnection);
            sqlCommand.ExecuteNonQuery();
            dbConnection.Close();
        }
        public void AddPlatform(IPlatformInfo platformInfo)
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"INSERT OR IGNORE INTO platformprefs VALUES(
                                          @platform_id,
                                          @emulator,
                                          @scraper)", dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platform_id", platformInfo.PlatformID);
                KeyValuePair<string, IEmulatorBridge> emulator = this.pluginManager.Plugins<IEmulatorBridge>().FirstOrDefault(x => x.Value.SupportedPlatforms.Contains(platformInfo.PlatformID));
                KeyValuePair<string, IScraper> scraper = this.pluginManager.Plugins<IScraper>().FirstOrDefault(x => x.Value.SupportedPlatforms.Contains(platformInfo.PlatformID));
                string emulatorId = emulator.Equals(default(KeyValuePair<string, IEmulatorBridge>)) ?  "null" : emulator.Key;
                string scraperId = scraper.Equals(default(KeyValuePair<string, IScraper>)) ? "null" : scraper.Key;
                sqlCommand.Parameters.AddWithValue("@emulator", emulatorId);
                sqlCommand.Parameters.AddWithValue("@scraper", scraperId);
                sqlCommand.ExecuteNonQuery();
            }
            dbConnection.Close();

        }
        public IPlatformDefaults GetPreferences(IPlatformInfo platformInfo)
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"SELECT * FROM `platformprefs` WHERE `platform_id` == @platform_id"
                , dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platform_id", platformInfo.PlatformID);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    result.Load(reader);
                    var row = result.Rows[0];
                    string scraper = result.Rows[0].Field<string>("scraper");
                    string emulator = result.Rows[0].Field<string>("emulator");
                    IPlatformDefaults platformDefaults =
                        new PlatformDefaults(scraper,  emulator);
                    dbConnection.Close();
                    return platformDefaults;
                }
            }
        }
        public void SetEmulator(IPlatformInfo platformInfo, string value)
        {
            this.SetColumn(platformInfo, "emulator", value);
        }
        public void SetScraper(IPlatformInfo platformInfo, string value)
        {
            this.SetColumn(platformInfo, "scraper", value);
        }
        private void SetColumn(IPlatformInfo platformInfo, string column, string value)
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand("UPDATE `platformprefs` SET `%colName` = @value WHERE `platform_id` == @platform_id", dbConnection))
            {
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%colName", column);

                sqlCommand.Parameters.AddWithValue("@value", value);
                sqlCommand.Parameters.AddWithValue("@platform_id", platformInfo.PlatformID);
                sqlCommand.ExecuteNonQuery();
            }
            dbConnection.Close();
        }

    }
}
