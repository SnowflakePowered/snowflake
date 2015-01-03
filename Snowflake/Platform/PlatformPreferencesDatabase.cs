using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
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
            using (var sqlCommand = new SQLiteCommand(@"INSERT OR REPLACE INTO platformprefs VALUES(
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
        public string GetEmulator(IPlatformInfo platformInfo) { }
        public string GetScraper(IPlatformInfo platformInfo) { }
        public string GetIdentifier(IPlatformInfo platformInfo) { }
        public void SetEmulator(IPlatformInfo platformInfo) { }
        public void SetScraper(IPlatformInfo platformInfo) { }
        public void SetIdentifier(IPlatformInfo platformInfo) { }
    }
}
