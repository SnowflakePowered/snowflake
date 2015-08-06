using System.Collections.Generic;

namespace Snowflake.Emulator.Input
{
    public class GamepadMapping : IGamepadMapping
    {
        public string GAMEPAD_A { get; }
        public string GAMEPAD_B { get; }
        public string GAMEPAD_X { get; }
        public string GAMEPAD_Y { get; }
        public string GAMEPAD_START { get; }
        public string GAMEPAD_SELECT { get; }
        public string GAMEPAD_L1 { get; }
        public string GAMEPAD_L2 { get; }
        public string GAMEPAD_L3 { get; }
        public string GAMEPAD_R1 { get; }
        public string GAMEPAD_R2 { get; }
        public string GAMEPAD_R3 { get; }
        public string GAMEPAD_L_X_LEFT { get; }
        public string GAMEPAD_L_X_RIGHT { get; }
        public string GAMEPAD_L_Y_UP { get; }
        public string GAMEPAD_L_Y_DOWN { get; }
        public string GAMEPAD_R_X_LEFT { get; }
        public string GAMEPAD_R_X_RIGHT { get; }
        public string GAMEPAD_R_Y_UP { get; }
        public string GAMEPAD_R_Y_DOWN { get; }
        public string GAMEPAD_GUIDE { get; }
        public string GAMEPAD_DPAD_UP { get; }
        public string GAMEPAD_DPAD_DOWN { get; }
        public string GAMEPAD_DPAD_LEFT { get; }
        public string GAMEPAD_DPAD_RIGHT { get; }
        private readonly IDictionary<string, string> mappingData;
        public GamepadMapping(IDictionary<string, string> mappingData)
        {
            this.mappingData = mappingData;
            this.GAMEPAD_A = mappingData["GAMEPAD_A"];
            this.GAMEPAD_B = mappingData["GAMEPAD_B"];
            this.GAMEPAD_X = mappingData["GAMEPAD_X"];
            this.GAMEPAD_Y = mappingData["GAMEPAD_Y"];
            this.GAMEPAD_START = mappingData["GAMEPAD_START"];
            this.GAMEPAD_SELECT = mappingData["GAMEPAD_SELECT"];
            this.GAMEPAD_L1 = mappingData["GAMEPAD_L1"];
            this.GAMEPAD_L2 = mappingData["GAMEPAD_L2"];
            this.GAMEPAD_L3 = mappingData["GAMEPAD_L3"];
            this.GAMEPAD_R1 = mappingData["GAMEPAD_R1"];
            this.GAMEPAD_R2 = mappingData["GAMEPAD_R2"];
            this.GAMEPAD_R3 = mappingData["GAMEPAD_R3"];
            this.GAMEPAD_L_X_LEFT = mappingData["GAMEPAD_L_X_LEFT"];
            this.GAMEPAD_L_X_RIGHT = mappingData["GAMEPAD_L_X_RIGHT"];
            this.GAMEPAD_L_Y_UP = mappingData["GAMEPAD_L_Y_UP"];
            this.GAMEPAD_L_Y_DOWN = mappingData["GAMEPAD_L_Y_DOWN"];
            this.GAMEPAD_R_X_LEFT = mappingData["GAMEPAD_R_X_LEFT"];
            this.GAMEPAD_R_X_RIGHT = mappingData["GAMEPAD_R_X_RIGHT"];
            this.GAMEPAD_R_Y_UP = mappingData["GAMEPAD_R_Y_UP"];
            this.GAMEPAD_R_Y_DOWN = mappingData["GAMEPAD_R_Y_DOWN"];
            this.GAMEPAD_GUIDE = mappingData["GAMEPAD_GUIDE"];
            this.GAMEPAD_DPAD_UP = mappingData["GAMEPAD_DPAD_UP"];
            this.GAMEPAD_DPAD_DOWN = mappingData["GAMEPAD_DPAD_DOWN"];
            this.GAMEPAD_DPAD_LEFT = mappingData["GAMEPAD_DPAD_LEFT"];
            this.GAMEPAD_DPAD_RIGHT = mappingData["GAMEPAD_DPAD_RIGHT"];
        }
        public string this[string key] => this.mappingData[key];
    }
}
