using System;
using System.Data;
using System.Data.SQLite;
using Snowflake.Platform;
using Snowflake.Utility;

namespace Snowflake.Controller
{
    public class ControllerPortsDatabase : BaseDatabase, IControllerPortsDatabase
    {
        public ControllerPortsDatabase(string fileName)
            : base(fileName)
        {
            this.CreateDatabase();
        }

        private void CreateDatabase()
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS ports(
                                                                platform_id TEXT PRIMARY KEY,
                                                                port1 TEXT,
                                                                port2 TEXT,
                                                                port3 TEXT,
                                                                port4 TEXT,
                                                                port5 TEXT,
                                                                port6 TEXT,
                                                                port7 TEXT,
                                                                port8 TEXT
                                                                )", dbConnection);
            sqlCommand.ExecuteNonQuery();
            dbConnection.Close();
        }

        public void AddPlatform(IPlatformInfo platformInfo)
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand(@"INSERT OR IGNORE INTO ports VALUES(
                                                                @platform_id,
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                null,
                                                                null
                                                                )", dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platform_id", platformInfo.PlatformID);
                sqlCommand.ExecuteNonQuery();
                dbConnection.Close();
            }/*
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                this.SetDefaults_Win32(platformInfo); //Set windows defaults if runs on windows. 
            }
            else
            {
                this.SetDefaults_KeyboardOnly(platformInfo); //Only set keyboard defaults
            }*/
        }
        public string GetDeviceInPort(IPlatformInfo platformInfo, int portNumber)
        {
            if (portNumber > 8 || portNumber < 1){
                return string.Empty;
            }
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand($"SELECT `port{portNumber}` FROM `ports` WHERE `platform_id` == @platformId", dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@platformId", platformInfo.PlatformID);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var result = new DataTable();
                    result.Load(reader);
                    DataRow row = result.Rows[0];
                    dbConnection.Close();
                    return row.Field<string>($"port{portNumber}");
                }
            }
        }

        public void SetDeviceInPort(IPlatformInfo platformInfo, int portNumber, string deviceName)
        {
            SQLiteConnection dbConnection = this.GetConnection();
            if (portNumber > 8 || portNumber < 1)
            {
                throw new IndexOutOfRangeException("Snowflake only supports consoles up to 8 controller ports");
            }
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand($"UPDATE `ports` SET `port{portNumber}` = @controllerId WHERE `platform_id` == @platformId", dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@controllerId", deviceName);
                sqlCommand.Parameters.AddWithValue("@platformId", platformInfo.PlatformID);
                sqlCommand.ExecuteNonQuery();
            }
            dbConnection.Close();
        }

        private void SetDefaults_Win32(IPlatformInfo platformInfo)
        {
            this.SetDeviceInPort(platformInfo, 1, InputDeviceNames.KeyboardDevice);
            this.SetDeviceInPort(platformInfo, 2, InputDeviceNames.XInputDevice1);
            this.SetDeviceInPort(platformInfo, 3, InputDeviceNames.XInputDevice2);
            this.SetDeviceInPort(platformInfo, 4, InputDeviceNames.XInputDevice3);
            this.SetDeviceInPort(platformInfo, 5, InputDeviceNames.XInputDevice4);
        }
        private void SetDefaults_KeyboardOnly(IPlatformInfo platformInfo)
        {
            this.SetDeviceInPort(platformInfo, 1, InputDeviceNames.KeyboardDevice);
        }
    }
}
