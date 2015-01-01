using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Snowflake.Game;
using System.Data;
using Snowflake.Utility;
using Snowflake.Emulator.Configuration;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationFlagDatabase : BaseDatabase, IConfigurationFlagDatabase
    {
        public ConfigurationFlagDatabase(string fileName)
            : base(fileName)
        {

        }
        public void CreateFlagsTable(string emulatorId, IList<IConfigurationFlag> configFlags)
        {
            var queryString = new StringBuilder();
            queryString.AppendFormat("CREATE TABLE IF NOT EXISTS {0}(", emulatorId);

            foreach (IConfigurationFlag configFlag in configFlags)
            {
                queryString.AppendFormat("{0} TEXT,", configFlag.Key);
            }
            queryString.Append("game_id TEXT PRIMARY KEY)");
            this.DBConnection.Open();
            var sqlCommand = new SQLiteCommand(queryString.ToString(), this.DBConnection);
            sqlCommand.ExecuteNonQuery();
            this.DBConnection.Close();
        }

        public void AddGame(IGameInfo gameInfo, string emulatorId, IList<IConfigurationFlag> configFlags, IDictionary<string, string> flagValues)
        {
            
            var queryString = new StringBuilder();
            queryString.AppendFormat("INSERT OR REPLACE INTO {0} VALUES(", emulatorId);
            foreach (ConfigurationFlag configFlag in configFlags)
            {
                queryString.AppendFormat("@{0},", configFlag.Key);
            }
            queryString.Append("@game_id)");
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(queryString.ToString(), this.DBConnection))
            {
                sqlCommand.Parameters.AddWithValue("@game_id", gameInfo.UUID);
                foreach (ConfigurationFlag configFlag in configFlags)
                {
                    try
                    {
                        sqlCommand.Parameters.AddWithValue(String.Format("@{0}", configFlag.Key), flagValues[configFlag.Key]);
                    }
                    catch (KeyNotFoundException)
                    {
                        sqlCommand.Parameters.AddWithValue(String.Format("@{0}", configFlag.Key), configFlag.DefaultValue);
                    }
                }
                sqlCommand.ExecuteNonQuery();
                this.DBConnection.Close();
            }
        }

        public object GetValue(IGameInfo gameInfo, string emulatorId, string key, ConfigurationFlagTypes type) 
        {
            this.DBConnection.Open();
            //tableName and key are presumed to be safe since they are defined by plugin. Plugins can have malicious behaviour even if this input is sanitized so it's not worth sanitizing. 
            //Parameters do not work for the table name
            using (var sqlCommand = new SQLiteCommand("SELECT `%key` FROM `%emulatorId` WHERE `game_id` == @gameId", this.DBConnection))
            {
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%key", key);
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%emulatorId", emulatorId);

                sqlCommand.Parameters.AddWithValue("@gameId", gameInfo.UUID);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    result.Load(reader);
                    var row = result.Rows[0];
                    this.DBConnection.Close();
                    
                    var value = row.Field<string>(key);
                    switch (type)
                    {
                        case ConfigurationFlagTypes.BOOLEAN_FLAG:
                            return Boolean.Parse(value);
                        case ConfigurationFlagTypes.INTEGER_FLAG:
                            return Int32.Parse(value);
                        case ConfigurationFlagTypes.SELECT_FLAG:
                            return value;
                        default:
                            return value;
                    }
                }
            }
        }
    }
}
