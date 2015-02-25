using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using Snowflake.Utility;
namespace Snowflake.Platform
{
    public class PlatformPreferencesDatabase : BaseDatabase, IPlatformPreferenceDatabase
    {
        public PlatformPreferencesDatabase(string fileName)
            : base(fileName)
        {
            this.CreateDatabase();
        }
        private void CreateDatabase()
        {
            this.DBConnection.Open();
            var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS platformprefs(
                                                                platform_id TEXT PRIMARY KEY,
                                                                emulator TEXT,
                                                                scraper TEXT,
                                                                identifier TEXT
                                                                )", this.DBConnection);
            sqlCommand.ExecuteNonQuery();
            this.DBConnection.Close();
        }
        public void AddPlatform(IPlatformInfo platformInfo)
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"INSERT OR IGNORE INTO platformprefs VALUES(
                                          @platform_id,
                                          @emulator,
                                          @scraper,
                                          @identifier)", this.DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platform_id", platformInfo.PlatformId);
                sqlCommand.Parameters.AddWithValue("@emulator", platformInfo.Defaults.Emulator);
                sqlCommand.Parameters.AddWithValue("@scraper", platformInfo.Defaults.Scraper);
                sqlCommand.Parameters.AddWithValue("@identifier", platformInfo.Defaults.Identifier);
                sqlCommand.ExecuteNonQuery();
            }
            this.DBConnection.Close();

        }
        public IPlatformDefaults GetPreferences(IPlatformInfo platformInfo)
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"SELECT * FROM `platformprefs` WHERE `platform_id` == @platform_id"
                , this.DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platform_id", platformInfo.PlatformId);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    result.Load(reader);
                    var row = result.Rows[0];
                    string scraper = result.Rows[0].Field<string>("scraper");
                    string identifier = result.Rows[0].Field<string>("identifier");
                    string emulator = result.Rows[0].Field<string>("emulator");
                    IPlatformDefaults platformDefaults =
                        new PlatformDefaults(scraper, identifier, emulator);
                    this.DBConnection.Close();
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
        public void SetIdentifier(IPlatformInfo platformInfo, string value)
        {
            this.SetColumn(platformInfo, "identifier", value);
        }

        private void SetColumn(IPlatformInfo platformInfo, string column, string value)
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand("UPDATE `platformprefs` SET `%colName` = @value WHERE `platform_id` == @platform_id", this.DBConnection))
            {
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%colName", column);

                sqlCommand.Parameters.AddWithValue("@value", value);
                sqlCommand.Parameters.AddWithValue("@platform_id", platformInfo.PlatformId);
                sqlCommand.ExecuteNonQuery();
            }
            this.DBConnection.Close();
        }

    }
}
