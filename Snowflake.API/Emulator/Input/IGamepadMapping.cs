namespace Snowflake.Emulator.Input
{
    /// <summary>
    /// An IGamepadMapping maps a standard XInput-style gamepad to the values that represent each button in an emulator.
    /// IGamepadMapping supports the 4 lettered face buttons (ABXY), X and Y +/- values for 2 analog sticks and stick depression, and 4 shoulder buttons
    /// </summary>
    public interface IGamepadMapping
    {
        string GAMEPAD_A { get; }
        string GAMEPAD_B { get; }
        string GAMEPAD_DPAD_DOWN { get; }
        string GAMEPAD_DPAD_LEFT { get; }
        string GAMEPAD_DPAD_RIGHT { get; }
        string GAMEPAD_DPAD_UP { get; }
        string GAMEPAD_GUIDE { get; }
        string GAMEPAD_L_X_RIGHT { get; }
        string GAMEPAD_L_X_LEFT { get; }
        string GAMEPAD_L_Y_DOWN { get; }
        string GAMEPAD_L_Y_UP { get; }
        string GAMEPAD_L1 { get; }
        string GAMEPAD_L2 { get; }
        string GAMEPAD_L3 { get; }
        string GAMEPAD_R_X_RIGHT { get; }
        string GAMEPAD_R_X_LEFT { get; }
        string GAMEPAD_R_Y_DOWN { get; }
        string GAMEPAD_R_Y_UP { get; }
        string GAMEPAD_R1 { get; }
        string GAMEPAD_R2 { get; }
        string GAMEPAD_R3 { get; }
        string GAMEPAD_SELECT { get; }
        string GAMEPAD_START { get; }
        string GAMEPAD_X { get; }
        string GAMEPAD_Y { get; }
        string this[string key] { get; }
    }
}
