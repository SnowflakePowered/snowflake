using System;
using System.Data;
using System.Data.SQLite;
using Snowflake.Platform;
using Snowflake.Utility;

namespace Snowflake.Controller
{
    public class ControllerPortsDatabase : IControllerPortsDatabase
    {
        private readonly ISimpleKeyValueStore backingValueStore;
        public ControllerPortsDatabase(string fileName)
        {
            this.backingValueStore = new SqliteKeyValueStore(fileName);
        }

        public void AddPlatform(IPlatformInfo platformInfo)
        {
            //todo do something here; currently not nescesary for value store implementation
          /*  SQLiteConnection dbConnection = this.GetConnection();
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
            }
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
                return null;
            }
            return this.backingValueStore.GetObject<string>($"{platformInfo.PlatformID}_port{portNumber}");
        }

        public void SetDeviceInPort(IPlatformInfo platformInfo, int portNumber, string deviceName)
        {
            this.backingValueStore.InsertObject<string>($"{platformInfo.PlatformID}_port{portNumber}", deviceName);
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
