using System;
namespace Snowflake.Emulator.Input
{
    /// <summary>
    /// Represents the buttons on a keyboard.
    /// Numpad keys are prefixed with NUMPAD
    /// </summary>
    public interface IKeyboardMapping
    {
        string KEY_0 { get; }
        string KEY_1 { get; }
        string KEY_2 { get; }
        string KEY_3 { get; }
        string KEY_4 { get; }
        string KEY_5 { get; }
        string KEY_6 { get; }
        string KEY_7 { get; }
        string KEY_8 { get; }
        string KEY_9 { get; }
        string KEY_A { get; }
        string KEY_ALT { get; }
        string KEY_B { get; }
        string KEY_BACKSLASH { get; }
        string KEY_BACKSPACE { get; }
        string KEY_BRACKET_LEFT { get; }
        string KEY_BRACKET_RIGHT { get; }
        string KEY_C { get; }
        string KEY_COMMA { get; }
        string KEY_CTRL { get; }
        string KEY_D { get; }
        string KEY_DELETE { get; }
        string KEY_DOWN { get; }
        string KEY_E { get; }
        string KEY_END { get; }
        string KEY_ENTER { get; }
        string KEY_EQUALS { get; }
        string KEY_ESCAPE { get; }
        string KEY_F { get; }
        string KEY_F_1 { get; }
        string KEY_F_10 { get; }
        string KEY_F_11 { get; }
        string KEY_F_12 { get; }
        string KEY_F_2 { get; }
        string KEY_F_3 { get; }
        string KEY_F_4 { get; }
        string KEY_F_5 { get; }
        string KEY_F_6 { get; }
        string KEY_F_7 { get; }
        string KEY_F_8 { get; }
        string KEY_F_9 { get; }
        string KEY_G { get; }
        string KEY_H { get; }
        string KEY_HOME { get; }
        string KEY_I { get; }
        string KEY_INSERT { get; }
        string KEY_J { get; }
        string KEY_K { get; }
        string KEY_L { get; }
        string KEY_LEFT { get; }
        string KEY_M { get; }
        string KEY_MINUS { get; }
        string KEY_N { get; }
        string KEY_NUMPAD_0 { get; }
        string KEY_NUMPAD_1 { get; }
        string KEY_NUMPAD_2 { get; }
        string KEY_NUMPAD_3 { get; }
        string KEY_NUMPAD_4 { get; }
        string KEY_NUMPAD_5 { get; }
        string KEY_NUMPAD_6 { get; }
        string KEY_NUMPAD_7 { get; }
        string KEY_NUMPAD_8 { get; }
        string KEY_NUMPAD_9 { get; }
        string KEY_NUMPAD_ENTER { get; }
        string KEY_NUMPAD_MINUS { get; }
        string KEY_NUMPAD_PERIOD { get; }
        string KEY_NUMPAD_PLUS { get; }
        string KEY_O { get; }
        string KEY_P { get; }
        string KEY_PAGE_DOWN { get; }
        string KEY_PAGE_UP { get; }
        string KEY_PERIOD { get; }
        string KEY_Q { get; }
        string KEY_QUOTE { get; }
        string KEY_R { get; }
        string KEY_RIGHT { get; }
        string KEY_RIGHT_ALT { get; }
        string KEY_RIGHT_CTRL { get; }
        string KEY_RIGHT_SHIFT { get; }
        string KEY_S { get; }
        string KEY_SEMICOLON { get; }
        string KEY_SHIFT { get; }
        string KEY_SLASH { get; }
        string KEY_SPACEBAR { get; }
        string KEY_T { get; }
        string KEY_TAB { get; }
        string KEY_TILDE { get; }
        string KEY_U { get; }
        string KEY_UP { get; }
        string KEY_V { get; }
        string KEY_W { get; }
        string KEY_X { get; }
        string KEY_Y { get; }
        string KEY_Z { get; }
        string MOUSE_Y_DOWN { get; }
        string MOUSE_Y_UP { get; }
        string MOUSE_X_LEFT { get; }
        string MOUSE_X_RIGHT { get; }
        string this[string key] { get; }
    }
}
