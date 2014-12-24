using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using Snowflake.Platform.Controller;
using Snowflake.Platform;
namespace Snowflake.Database
{
    public class ControllerDatabase : BaseDatabase
    {
        public ControllerDatabase(string fileName) : base(fileName){

        }

        public void LoadTables(IDictionary<string, PlatformInfo> platforms){
            foreach (PlatformInfo platform in platforms.Values)
            {
                foreach (ControllerDefinition controller in platform.Controllers.Values)
                {
                    this.CreateControllerTable(controller);
                }
            }
        }
        private void CreateControllerTable(ControllerDefinition controllerDefinition)
        {
            var queryString = new StringBuilder();
            queryString.AppendFormat("CREATE TABLE IF NOT EXISTS {0}(", controllerDefinition.ControllerID);
            foreach (ControllerInput input in controllerDefinition.ControllerInputs.Values)
            {
                queryString.AppendFormat("INPUT_{0} TEXT,", input.InputName);
            }
            queryString.Append("ControllerID TEXT,");
            queryString.Append("PlatformID TEXT,");
            queryString.Append("ProfileType TEXT,");
            queryString.Append("ControllerIndex INTEGER)");

            this.DBConnection.Open();
            var sqlCommand = new SQLiteCommand(queryString.ToString(), this.DBConnection);
            sqlCommand.ExecuteNonQuery();
            this.DBConnection.Close();

        }
        public void LoadFromProfile(ControllerProfile controllerProfile, int controllerIndex)
        {
            var queryString = new StringBuilder();
            queryString.AppendFormat("INSERT INTO {0} (", controllerProfile.ControllerID);
            foreach (KeyValuePair<string, string> input in controllerProfile.InputConfiguration)
            {
                queryString.AppendFormat("INPUT_{0},", input.Key);
            }
            queryString.Append("ControllerID,");
            queryString.Append("PlatformID,");
            queryString.Append("ProfileType,");
            queryString.Append("ControllerIndex) VALUES (");
            foreach (KeyValuePair<string, string> input in controllerProfile.InputConfiguration)
            {
                queryString.AppendFormat("@INPUT_{0},", input.Key);
            }
            queryString.Append("@ControllerID,");
            queryString.Append("@PlatformID,");
            queryString.Append("@ProfileType,");
            queryString.Append("@ControllerIndex)");
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(queryString.ToString(), this.DBConnection))
            {
                foreach (KeyValuePair<string, string> input in controllerProfile.InputConfiguration)
                {
                    sqlCommand.Parameters.AddWithValue(String.Format("@INPUT_{0}", input.Key), input.Value);
                }
                sqlCommand.Parameters.AddWithValue("@ControllerID", controllerProfile.ControllerID);
                sqlCommand.Parameters.AddWithValue("@PlatformID", controllerProfile.PlatformID);
                sqlCommand.Parameters.AddWithValue("@ProfileType", controllerProfile.ProfileType.ToString());
                sqlCommand.Parameters.AddWithValue("@ControllerIndex", controllerIndex);
                Console.WriteLine(sqlCommand.CommandText);

                sqlCommand.ExecuteNonQuery();
            }
            this.DBConnection.Close();
        }
        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(this.FileName);
            var dbConnection = new SQLiteConnection("Data Source=" + this.FileName + ";Version=3;");
            dbConnection.Open();
            var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS games(
                                                                platform_id TEXT,
                                                                uuid TEXT,
                                                                filename TEXT,
                                                                name TEXT,
                                                                mediastorekey TEXT,
                                                                metadata TEXT,
                                                                settings TEXT
                                                                )", dbConnection);
            sqlCommand.ExecuteNonQuery();
            dbConnection.Close();
        }


    }
}
