using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;
using Snowflake.Platform;
using Snowflake.Utility;
namespace Snowflake.Controller
{
    public class ControllerProfileDatabase : BaseDatabase, IControllerProfileDatabase
    {
        public ControllerProfileDatabase(string fileName)
            : base(fileName)
        {

        }

        public void AddPlatform(IPlatformInfo platform)
        {
         /*   foreach (IControllerDefinition controller in platform.Controllers.Values)
            {
                this.CreateControllerTable(controller);
            }*/
        }
        private void CreateControllerTable(IControllerDefinition controllerDefinition)
        {
            var queryString = new StringBuilder();
            queryString.AppendFormat("CREATE TABLE IF NOT EXISTS {0}(", controllerDefinition.ControllerID);
            foreach (IControllerInput input in controllerDefinition.ControllerInputs.Values)
            {
                queryString.AppendFormat("INPUT_{0} TEXT,", input.InputName);
            }
            queryString.Append("ControllerID TEXT,");
            queryString.Append("ProfileType TEXT,");
            queryString.Append("DeviceName TEXT,");
            queryString.Append("ControllerIndex INTEGER PRIMARY KEY)");

            this.DBConnection.Open();
            var sqlCommand = new SQLiteCommand(queryString.ToString(), this.DBConnection);
            sqlCommand.ExecuteNonQuery();
            this.DBConnection.Close();

            try 
            {
                //If there are no valid controller configurations put the defaults in there
                this.GetControllerProfile(controllerDefinition.ControllerID, 1); 
                this.GetControllerProfile(controllerDefinition.ControllerID, 2);
            }
            catch
            {
                this.AddDefaults(controllerDefinition);
            }
            
        }

        private void AddDefaults(IControllerDefinition controllerDefinition)
        {
            IControllerProfile keyboard1 = new ControllerProfile(controllerDefinition.ControllerID, ControllerProfileType.KEYBOARD_PROFILE, controllerDefinition.ControllerInputs.ToDictionary(input => input.Value.InputName, input => input.Value.KeyboardDefault));
            IControllerProfile gamepad2 = new ControllerProfile(controllerDefinition.ControllerID, ControllerProfileType.GAMEPAD_PROFILE, controllerDefinition.ControllerInputs.ToDictionary(input => input.Value.InputName, input => input.Value.GamepadDefault));
            this.AddControllerProfile(keyboard1, 1);
            this.AddControllerProfile(gamepad2, 2);
        }

        public void AddControllerProfile(IControllerProfile controllerProfile, int controllerIndex)
        {
            var queryString = new StringBuilder();
            queryString.AppendFormat("INSERT OR REPLACE INTO {0} (", controllerProfile.ControllerID);
            foreach (KeyValuePair<string, string> input in controllerProfile.InputConfiguration)
            {
                queryString.AppendFormat("INPUT_{0},", input.Key);
            }
            queryString.Append("ControllerID,");
            queryString.Append("ProfileType,");
            queryString.Append("DeviceName,");
            queryString.Append("ControllerIndex) VALUES (");
            foreach (KeyValuePair<string, string> input in controllerProfile.InputConfiguration)
            {
                queryString.AppendFormat("@INPUT_{0},", input.Key);
            }
            queryString.Append("@ControllerID,");
            queryString.Append("@ProfileType,");
            queryString.Append("@DeviceName,");
            queryString.Append("@ControllerIndex)");
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(queryString.ToString(), this.DBConnection))
            {
                foreach (KeyValuePair<string, string> input in controllerProfile.InputConfiguration)
                {
                    sqlCommand.Parameters.AddWithValue(String.Format("@INPUT_{0}", input.Key), input.Value);
                }
                sqlCommand.Parameters.AddWithValue("@ControllerID", controllerProfile.ControllerID);
                sqlCommand.Parameters.AddWithValue("@ProfileType", controllerProfile.ProfileType.ToString());
                sqlCommand.Parameters.AddWithValue("@DeviceName", null);
                sqlCommand.Parameters.AddWithValue("@ControllerIndex", controllerIndex);

                sqlCommand.ExecuteNonQuery();
            }
            this.DBConnection.Close();
        }
        public void SetDeviceName(string controllerId, int controllerIndex, string deviceName)
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand("UPDATE `%tableName` SET `DeviceName` = @deviceName WHERE `ControllerIndex` == @controllerIndex", this.DBConnection))
            {
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%tableName", controllerId);

                sqlCommand.Parameters.AddWithValue("@deviceName", deviceName);
                sqlCommand.Parameters.AddWithValue("@controllerIndex", controllerIndex);
                sqlCommand.ExecuteNonQuery();
            }
            this.DBConnection.Close();
        }
        public string GetDeviceName(string controllerId, int controllerIndex)
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand("SELECT `DeviceName` FROM `%tableName` WHERE `ControllerIndex` == @controllerIndex", this.DBConnection))
            {
                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%tableName", controllerId);
                sqlCommand.Parameters.AddWithValue("@controllerIndex", controllerIndex);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    result.Load(reader);
                    var row = result.Rows[0];
                    this.DBConnection.Close();
                    return row.Field<string>("DeviceName");
                }
            }
        }
        public IControllerProfile GetControllerProfile(string controllerId, int controllerIndex)
        {
            this.DBConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"SELECT * FROM `%tableName` WHERE `ControllerIndex` == @controllerIndex"
                , this.DBConnection))
            {

                sqlCommand.CommandText = sqlCommand.CommandText.Replace("%tableName", controllerId);
                sqlCommand.Parameters.AddWithValue("@controllerIndex", controllerIndex);
                try
                {
                    using (var reader = sqlCommand.ExecuteReader())
                    {
                        var result = new DataTable();
                        result.Load(reader);
                        var row = result.Rows[0];
                        var resultProfileType = (ControllerProfileType)Enum.Parse(typeof(ControllerProfileType), row.Field<string>("ProfileType"), true);
                        var resultControllerId = row.Field<string>("ControllerID");
                        IDictionary<string, string> resultInputConfiguration =
                            (from DataColumn item in result.Columns
                             where item.ColumnName.StartsWith("INPUT_")
                             select new KeyValuePair<string, string>
                                 (item.ColumnName.Remove(0, "INPUT_".Length), result.Rows[0].Field<string>(item)))
                             .ToDictionary(config => config.Key, config => config.Value);
                        this.DBConnection.Close();
                        return new ControllerProfile(resultControllerId, resultProfileType, resultInputConfiguration);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    this.DBConnection.Close();
                }
            }
        }
    }
}
