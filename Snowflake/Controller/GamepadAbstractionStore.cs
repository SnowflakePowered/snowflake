using System;
using System.Collections.Generic;
using System.Linq;
using Snowflake.Emulator.Input.Constants;
using Snowflake.Information.Database;
using Snowflake.Utility;

namespace Snowflake.Controller
{
    public class GamepadAbstractionStore : IGamepadAbstractionStore
    {
        private readonly ISimpleKeyValueStore backingDatabase;
        public GamepadAbstractionStore(string fileName)
        {
            this.backingDatabase = new SqliteKeyValueStore(fileName);
            this.SetGamepadAbstraction("~defaultKeyboard", this.DefaultKeyboard, true);
            this.SetGamepadAbstraction("KeyboardDevice", this.DefaultKeyboard, false);

            this.SetGamepadAbstraction("~defaultGamepad", this.DefaultGamepad, true);
            this.SetGamepadAbstraction("XInputDevice1", this.DefaultGamepad, false);
            this.SetGamepadAbstraction("XInputDevice2", this.DefaultGamepad, false);
            this.SetGamepadAbstraction("XInputDevice3", this.DefaultGamepad, false);
            this.SetGamepadAbstraction("XInputDevice4", this.DefaultGamepad, false);
        }

        private readonly IGamepadAbstraction DefaultGamepad =
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

        private readonly IGamepadAbstraction DefaultKeyboard =
            new GamepadAbstraction("~defaultKeyboard", ControllerProfileType.KEYBOARD_PROFILE)
            {
                L1 = KeyboardConstants.KEY_SHIFT,
                L2 = KeyboardConstants.KEY_CTRL,
                L3 = KeyboardConstants.KEY_ALT,
                R1 = KeyboardConstants.KEY_RIGHT_SHIFT,
                R2 = KeyboardConstants.KEY_RIGHT_CTRL,
                R3 = KeyboardConstants.KEY_RIGHT_ALT,
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
                Y = KeyboardConstants.KEY_V
            };

        public IGamepadAbstraction GetGamepadAbstraction(string deviceName)
        {
            return this.backingDatabase.GetObject<IGamepadAbstraction>(deviceName);
        }

        public IEnumerable<IGamepadAbstraction> GetAllGamepadAbstractions()
        {
            return this.backingDatabase.GetAllObjects<IGamepadAbstraction>().Select(kvp => kvp.Value);
        }
        
        public void SetGamepadAbstraction(string deviceName, IGamepadAbstraction gamepadAbstraction)
        {
            this.backingDatabase.InsertObject(deviceName, gamepadAbstraction);
        }

        private void SetGamepadAbstraction(string deviceName, IGamepadAbstraction gamepadAbstraction, bool overwrite)
        {
            this.SetGamepadAbstraction(deviceName, gamepadAbstraction); //todo implement overwrite
        }

        public void RemoveGamepadAbstraction(string deviceName)
        {
            this.backingDatabase.DeleteObject(deviceName);
        }

        public IGamepadAbstraction this[string deviceName]
        {
            get
            {
                return this.GetGamepadAbstraction(deviceName);
            }
            set
            {
                this.SetGamepadAbstraction(deviceName, value);
            }
        }
    }
}

