using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Snowflake.Controller;

namespace Snowflake.Information.Database
{
    internal class SqliteGamepadAbstractionDatabase : DapperDatabase, IGamepadAbstractionDatabase
    {
        public SqliteGamepadAbstractionDatabase(string fileName)
            : base(fileName, typeof (SqliteGamepadAbstractionDatabaseMapper))
        {
            this.CreateDatabase();
        }

        IGamepadAbstraction IGamepadAbstractionDatabase.this[string deviceName]
        {
            get { return (this as IGamepadAbstractionDatabase).GetGamepadAbstraction(deviceName); }

            set { (this as IGamepadAbstractionDatabase).SetGamepadAbstraction(value.DeviceName, value); }
        }

        private void CreateDatabase()
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS gamepadabstraction(
                                                                DeviceName TEXT PRIMARY KEY,
                                                                ProfileType INTEGER,
                                                                L1 TEXT,
                                                                L2 TEXT,
                                                                L3 TEXT,
                                                                R1 TEXT,
                                                                R2 TEXT,
                                                                R3 TEXT,
                                                                DpadUp TEXT,
                                                                DpadDown TEXT,
                                                                DpadLeft TEXT,
                                                                DpadRight TEXT,
                                                                RightAnalogXLeft TEXT,
                                                                RightAnalogXRight TEXT,
                                                                RightAnalogYUp TEXT,
                                                                RightAnalogYDown TEXT,
                                                                LeftAnalogXLeft TEXT,
                                                                LeftAnalogXRight TEXT,
                                                                LeftAnalogYUp TEXT,
                                                                LeftAnalogYDown TEXT,
                                                                btnStart TEXT, 
                                                                btnSelect TEXT,
                                                                btnA TEXT,
                                                                btnB TEXT,
                                                                btnX TEXT,
                                                                btnY TEXT
                                                                )", dbConnection);
                //serialize btnSelect due to reserved sql keyword. other btn for consistency
            sqlCommand.ExecuteNonQuery();
            dbConnection.Close();
        }

        IEnumerable<IGamepadAbstraction> IGamepadAbstractionDatabase.GetAllGamepadAbstractions()
        {
            IEnumerable<IGamepadAbstraction> gamepadAbstractions;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                gamepadAbstractions = dbConnection.GetList<GamepadAbstraction>();
                dbConnection.Close();
            }
            return gamepadAbstractions;
        }

        IGamepadAbstraction IGamepadAbstractionDatabase.GetGamepadAbstraction(string deviceName)
        {
            IGamepadAbstraction gamepadAbstraction;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                gamepadAbstraction = dbConnection.Get<GamepadAbstraction>(deviceName);
                dbConnection.Close();
            }
            return gamepadAbstraction;
        }

        void IGamepadAbstractionDatabase.RemoveGamepadAbstraction(string deviceName)
        {
            IGamepadAbstraction gamepadAbstraction =
                (this as IGamepadAbstractionDatabase)?.GetGamepadAbstraction(deviceName);
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                dbConnection.Delete(gamepadAbstraction);
                dbConnection.Close();
            }
        }

        void IGamepadAbstractionDatabase.SetGamepadAbstraction(string deviceName, IGamepadAbstraction gamepadAbstraction)
        {

            var _gamepadAbstraction = (gamepadAbstraction as GamepadAbstraction);
            _gamepadAbstraction.DeviceName = deviceName;
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                if ((this as IGamepadAbstractionDatabase).GetGamepadAbstraction(deviceName) != null)
                {
                    dbConnection.Delete(_gamepadAbstraction);
                }
                dbConnection.Insert(_gamepadAbstraction);
                dbConnection.Close();
            }
        }
    }
}
