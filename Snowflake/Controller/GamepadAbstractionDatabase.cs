using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;
using Snowflake.Emulator.Input.Constants;
using System.Data.SQLite;
using System.Data;
namespace Snowflake.Controller
{
    public class GamepadAbstractionDatabase : BaseDatabase, IGamepadAbstractionDatabase
    {
        public GamepadAbstractionDatabase(string fileName)
            : base(fileName)
        {
            this.CreateDatabase();
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
                                                                Start TEXT,
                                                                Select TEXT,
                                                                A TEXT,
                                                                B TEXT,
                                                                X TEXT,
                                                                Y TEXT
                                                                )", dbConnection);
            sqlCommand.ExecuteNonQuery();
            dbConnection.Close();
            this.SetGamepadAbstraction("~defaultGamepad", DefaultGamepad, true);
            this.SetGamepadAbstraction("~defaultKeyboard", DefaultKeyboard, true);
            this.SetGamepadAbstraction("KeyboardDevice", DefaultKeyboard, false);
            this.SetGamepadAbstraction("XInputDevice1", DefaultKeyboard, false);
            this.SetGamepadAbstraction("XInputDevice2", DefaultGamepad, false);
            this.SetGamepadAbstraction("XInputDevice3", DefaultGamepad, false);
            this.SetGamepadAbstraction("XInputDevice4", DefaultGamepad, false);


        }

        private const IGamepadAbstraction DefaultGamepad =
            new GamepadAbstraction("~defaultGamepad", ControllerProfileType.GAMEPAD_PROFILE)
            {
                L1 = GamepadConstants.GAMEPAD_L1,
                L2 = GamepadConstants.GAMEPAD_L2,
                L3 = GamepadConstants.GAMEPAD_L3,
                R1 = GamepadConstants.GAMEPAD_R1,
                R2 = GamepadConstants.GAMEPAD_R2,
                R3 = GamepadConstants.GAMEPAD_R3,
                DpadUp = GamepadConstants.GAMEPAD_DPAD_UP,
                DpadDown = GamepadConstants.GAMEPAD_DPAD_DOWN,
                DpadLeft = GamepadConstants.GAMEPAD_DPAD_LEFT,
                DpadRight = GamepadConstants.GAMEPAD_DPAD_RIGHT,
                LeftAnalogYUp = GamepadConstants.GAMEPAD_L_Y_UP,
                LeftAnalogYDown = GamepadConstants.GAMEPAD_L_Y_DOWN,
                LeftAnalogXLeft = GamepadConstants.GAMEPAD_L_X_LEFT,
                LeftAnalogXRight = GamepadConstants.GAMEPAD_L_X_RIGHT,
                RightAnalogYUp = GamepadConstants.GAMEPAD_R_Y_UP,
                RightAnalogYDown = GamepadConstants.GAMEPAD_R_Y_DOWN,
                RightAnalogXLeft = GamepadConstants.GAMEPAD_R_X_LEFT,
                RightAnalogXRight = GamepadConstants.GAMEPAD_R_X_RIGHT,
                Select = GamepadConstants.GAMEPAD_SELECT,
                Start = GamepadConstants.GAMEPAD_START,
                A = GamepadConstants.GAMEPAD_A,
                B = GamepadConstants.GAMEPAD_B,
                X = GamepadConstants.GAMEPAD_X,
                Y = GamepadConstants.GAMEPAD_Y
            };

        private const IGamepadAbstraction DefaultKeyboard =
            new GamepadAbstraction("~defaultKeyboard", ControllerProfileType.KEYBOARD_PROFILE)
            {
                L1 = GamepadConstants.GAMEPAD_L1,
                L2 = GamepadConstants.GAMEPAD_L2,
                L3 = GamepadConstants.GAMEPAD_L3,
                R1 = GamepadConstants.GAMEPAD_R1,
                R2 = GamepadConstants.GAMEPAD_R2,
                R3 = GamepadConstants.GAMEPAD_R3,
                DpadUp = KeyboardConstants.KEY_UP,
                DpadDown = KeyboardConstants.KEY_DOWN,
                DpadLeft = KeyboardConstants.KEY_LEFT,
                DpadRight = KeyboardConstants.KEY_RIGHT,
                LeftAnalogYUp = KeyboardConstants.KEY_W,
                LeftAnalogYDown = KeyboardConstants.KEY_S,
                LeftAnalogXLeft = KeyboardConstants.KEY_A,
                LeftAnalogXRight = KeyboardConstants.KEY_D,
                RightAnalogYUp = KeyboardConstants.KEY_I,
                RightAnalogYDown = KeyboardConstants.KEY_J,
                RightAnalogXLeft = KeyboardConstants.KEY_K,
                RightAnalogXRight = KeyboardConstants.KEY_L,
                Select = KeyboardConstants.KEY_SPACEBAR,
                Start = KeyboardConstants.KEY_ENTER,
                A = KeyboardConstants.KEY_Z,
                B = KeyboardConstants.KEY_X,
                X = KeyboardConstants.KEY_C,
                Y = KeyboardConstants.KEY_Y
            };

        public IGamepadAbstraction GetGamepadAbstraction(string deviceName)
        {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand("SELECT `*` FROM `gamepadabstraction` WHERE `DeviceName` == @DeviceName", dbConnection))
            {
                sqlCommand.Parameters.AddWithValue("@DeviceName", deviceName);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    //gonna have to replace datatable one day
                    var result = new DataTable();
                    result.Load(reader);

                    var row = result.Rows[0];
                    dbConnection.Close();
                    return new GamepadAbstraction(deviceName, (ControllerProfileType) row.Field<int>("ProfileType"))
                    {
                        L1 = row.Field<string>("L1"),
                        L2 = row.Field<string>("L2"),
                        L3 = row.Field<string>("L3"),
                        R1 = row.Field<string>("R1"),
                        R2 = row.Field<string>("R2"),
                        R3 = row.Field<string>("R2"),
                        DpadUp = row.Field<string>("DpadUp"),
                        DpadDown = row.Field<string>("DpadDown"),
                        DpadLeft = row.Field<string>("DpadLeft"),
                        DpadRight = row.Field<string>("DpadRight"),
                        LeftAnalogYUp = row.Field<string>("LeftAnalogYUp"),
                        LeftAnalogYDown = row.Field<string>("LeftAnalogYDown"),
                        LeftAnalogXLeft = row.Field<string>("LeftAnalogXLeft"),
                        LeftAnalogXRight = row.Field<string>("LeftAnalogXRight"),
                        RightAnalogYUp = row.Field<string>("RightAnalogYUp"),
                        RightAnalogYDown = row.Field<string>("RightAnalogYDown"),
                        RightAnalogXLeft = row.Field<string>("RightAnalogXLeft"),
                        RightAnalogXRight = row.Field<string>("RightAnalogXRight"),
                        Select = row.Field<string>("Select"),
                        Start = row.Field<string>("Start"),
                        A = row.Field<string>("A"),
                        B = row.Field<string>("B"),
                        X = row.Field<string>("X"),
                        Y = row.Field<string>("Y")
                    };
                }
            }
        }

        public IList<IGamepadAbstraction> GetAllGamepadAbstractions () {
            SQLiteConnection dbConnection = this.GetConnection();
            dbConnection.Open();
            using (var sqlCommand = new SQLiteCommand("SELECT `*` FROM `gamepadabstraction`", dbConnection))
            {
                IList<IGamepadAbstraction> gamepadAbstractions = new List<IGamepadAbstraction>();
                using (var reader = sqlCommand.ExecuteReader())
                {
                    //gonna have to replace datatable one day
                    var result = new DataTable();
                    result.Load(reader);
                    foreach(DataRow row in result.Rows){
                        gamepadAbstractions.Add(new GamepadAbstraction(row.Field<string>("DeviceName"), (ControllerProfileType) row.Field<int>("ProfileType"))
                        {
                            L1 = row.Field<string>("L1"),
                            L2 = row.Field<string>("L2"),
                            L3 = row.Field<string>("L3"),
                            R1 = row.Field<string>("R1"),
                            R2 = row.Field<string>("R2"),
                            R3 = row.Field<string>("R2"),
                            DpadUp = row.Field<string>("DpadUp"),
                            DpadDown = row.Field<string>("DpadDown"),
                            DpadLeft = row.Field<string>("DpadLeft"),
                            DpadRight = row.Field<string>("DpadRight"),
                            LeftAnalogYUp = row.Field<string>("LeftAnalogYUp"),
                            LeftAnalogYDown = row.Field<string>("LeftAnalogYDown"),
                            LeftAnalogXLeft = row.Field<string>("LeftAnalogXLeft"),
                            LeftAnalogXRight = row.Field<string>("LeftAnalogXRight"),
                            RightAnalogYUp = row.Field<string>("RightAnalogYUp"),
                            RightAnalogYDown = row.Field<string>("RightAnalogYDown"),
                            RightAnalogXLeft = row.Field<string>("RightAnalogXLeft"),
                            RightAnalogXRight = row.Field<string>("RightAnalogXRight"),
                            Select = row.Field<string>("Select"),
                            Start = row.Field<string>("Start"),
                            A = row.Field<string>("A"),
                            B = row.Field<string>("B"),
                            X = row.Field<string>("X"),
                            Y = row.Field<string>("Y")
                        });
                    }
                    dbConnection.Close();
                    return gamepadAbstractions;
                }
            }
        }
        
        public void SetGamepadAbstraction(string deviceName, IGamepadAbstraction gamepadAbstraction)
        {
            this.SetGamepadAbstraction(deviceName, gamepadAbstraction, true);
        }

        private void SetGamepadAbstraction(string deviceName, IGamepadAbstraction gamepadAbstraction, bool overwrite)
        {
            throw new NotImplementedException();
        }
        public void RemoveGamepadAbstraction(string deviceName)
        {
            throw new NotImplementedException();
        }
    }
}

