using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Possible capabilities of a hardware device.
    /// 
    /// <para>
    /// This differs from <see cref="ControllerElement"/> in semantics;
    /// whereas <see cref="ControllerElement"/> semantics are derived from
    /// the Stone controller layout specification, <see cref="DeviceCapability"/>
    /// enumerations semantics come from the <see cref="IInputDriverInstance"/> for which
    /// these capabilities were instantiated.
    /// </para>
    /// 
    /// <para>
    /// These capabilities more or less mirror the union of possible capabilities provided
    /// by DirectInput, XInput, and evdev. Hence the semantics of each enum differ depending
    /// on the driver from which the instance was enumerated from.
    /// </para>
    /// 
    /// <para>
    /// However, Snowflake only supports unique mappings up to the set defined
    /// by Stone, so in practice, the full set of capabilities will not be able to be
    /// mapped to a controller.
    /// </para>
    /// 
    /// <para>
    /// <see cref="Button0"/> and such do not necessary map directly to <see cref="ControllerElement.Button0"/>.
    /// Commonly, <see cref="Button0"/> map instead map to <see cref="ControllerElement.ButtonA"/> and such,
    /// depending on the driver.
    /// </para>
    /// 
    /// <para>
    /// Axes have entries that are triplicated with the following semantics.
    /// <para>
    /// * Positive means the partition of the axis from 0.50 to 1.00, mapped to the interval [0, 1],
    /// that is 0 approaches 1 as 0.50 approaches 0.
    /// </para>
    /// <para>
    /// * Negative means the partition of the axis from 0.00 to 0.50, mapped to the interval [-1, 0],
    /// that is, 0 approaches -1 as 0.50 approaches 0.00
    /// </para>
    /// <para>
    /// * Without either qualifier means the entire axis from 0.00 to 1.00, mapped to the interval [-1, 1],
    /// that is -1 approaches 1 as 0.00 approaches 1.00.
    /// </para>
    /// 
    /// Every axis enumerated must have all three capabilities enumerated, but all three axis capabilities
    /// may not necessarily be mapped by an emulator.
    /// </para>
    /// </summary>
    public enum DeviceCapability
    {
        /// <summary>
        /// No capability.
        /// </summary>
        None,
        #region Buttons
        // -- Buttons
        /// <summary>
        /// Button 0
        /// </summary>
        Button0,

        /// <summary>
        /// Button 1
        /// </summary>
        Button1,

        /// <summary>
        /// Button 2
        /// </summary>
        Button2,

        /// <summary>
        /// Button 3
        /// </summary>
        Button3,

        /// <summary>
        /// Button 4
        /// </summary>
        Button4,

        /// <summary>
        /// Button 5
        /// </summary>
        Button5,

        /// <summary>
        /// Button 6
        /// </summary>
        Button6,

        /// <summary>
        /// Button 7
        /// </summary>
        Button7,

        /// <summary>
        /// Button 8
        /// </summary>
        Button8,

        /// <summary>
        /// Button 9
        /// </summary>
        Button9,

        /// <summary>
        /// Button 10
        /// </summary>
        Button10,

        /// <summary>
        /// Button 11
        /// </summary>
        Button11,

        /// <summary>
        /// Button 12
        /// </summary>
        Button12,

        /// <summary>
        /// Button 13
        /// </summary>
        Button13,

        /// <summary>
        /// Button 14
        /// </summary>
        Button14,

        /// <summary>
        /// Button 15
        /// </summary>
        Button15,

        /// <summary>
        /// Button 16
        /// </summary>
        Button16,

        /// <summary>
        /// Button 17
        /// </summary>
        Button17,

        /// <summary>
        /// Button 18
        /// </summary>
        Button18,

        /// <summary>
        /// Button 19
        /// </summary>
        Button19,

        /// <summary>
        /// Button 20
        /// </summary>
        Button20,

        /// <summary>
        /// Button 21
        /// </summary>
        Button21,

        /// <summary>
        /// Button 22
        /// </summary>
        Button22,

        /// <summary>
        /// Button 23
        /// </summary>
        Button23,

        /// <summary>
        /// Button 24
        /// </summary>
        Button24,

        /// <summary>
        /// Button 25
        /// </summary>
        Button25,

        /// <summary>
        /// Button 26
        /// </summary>
        Button26,

        /// <summary>
        /// Button 27
        /// </summary>
        Button27,

        /// <summary>
        /// Button 28
        /// </summary>
        Button28,

        /// <summary>
        /// Button 29
        /// </summary>
        Button29,

        /// <summary>
        /// Button 30
        /// </summary>
        Button30,

        /// <summary>
        /// Button 31
        /// </summary>
        Button31,

        /// <summary>
        /// Button 32
        /// </summary>
        Button32,

        /// <summary>
        /// Button 33
        /// </summary>
        Button33,

        /// <summary>
        /// Button 34
        /// </summary>
        Button34,

        /// <summary>
        /// Button 35
        /// </summary>
        Button35,

        /// <summary>
        /// Button 36
        /// </summary>
        Button36,

        /// <summary>
        /// Button 37
        /// </summary>
        Button37,

        /// <summary>
        /// Button 38
        /// </summary>
        Button38,

        /// <summary>
        /// Button 39
        /// </summary>
        Button39,

        /// <summary>
        /// Button 40
        /// </summary>
        Button40,

        /// <summary>
        /// Button 41
        /// </summary>
        Button41,

        /// <summary>
        /// Button 42
        /// </summary>
        Button42,

        /// <summary>
        /// Button 43
        /// </summary>
        Button43,

        /// <summary>
        /// Button 44
        /// </summary>
        Button44,

        /// <summary>
        /// Button 45
        /// </summary>
        Button45,

        /// <summary>
        /// Button 46
        /// </summary>
        Button46,

        /// <summary>
        /// Button 47
        /// </summary>
        Button47,

        /// <summary>
        /// Button 48
        /// </summary>
        Button48,

        /// <summary>
        /// Button 49
        /// </summary>
        Button49,

        /// <summary>
        /// Button 50
        /// </summary>
        Button50,

        /// <summary>
        /// Button 51
        /// </summary>
        Button51,

        /// <summary>
        /// Button 52
        /// </summary>
        Button52,

        /// <summary>
        /// Button 53
        /// </summary>
        Button53,

        /// <summary>
        /// Button 54
        /// </summary>
        Button54,

        /// <summary>
        /// Button 55
        /// </summary>
        Button55,

        /// <summary>
        /// Button 56
        /// </summary>
        Button56,

        /// <summary>
        /// Button 57
        /// </summary>
        Button57,

        /// <summary>
        /// Button 58
        /// </summary>
        Button58,

        /// <summary>
        /// Button 59
        /// </summary>
        Button59,

        /// <summary>
        /// Button 60
        /// </summary>
        Button60,

        /// <summary>
        /// Button 61
        /// </summary>
        Button61,

        /// <summary>
        /// Button 62
        /// </summary>
        Button62,

        /// <summary>
        /// Button 63
        /// </summary>
        Button63,

        /// <summary>
        /// Button 64
        /// </summary>
        Button64,

        /// <summary>
        /// Button 65
        /// </summary>
        Button65,

        /// <summary>
        /// Button 66
        /// </summary>
        Button66,

        /// <summary>
        /// Button 67
        /// </summary>
        Button67,

        /// <summary>
        /// Button 68
        /// </summary>
        Button68,

        /// <summary>
        /// Button 69
        /// </summary>
        Button69,

        /// <summary>
        /// Button 70
        /// </summary>
        Button70,

        /// <summary>
        /// Button 71
        /// </summary>
        Button71,

        /// <summary>
        /// Button 72
        /// </summary>
        Button72,

        /// <summary>
        /// Button 73
        /// </summary>
        Button73,

        /// <summary>
        /// Button 74
        /// </summary>
        Button74,

        /// <summary>
        /// Button 75
        /// </summary>
        Button75,

        /// <summary>
        /// Button 76
        /// </summary>
        Button76,

        /// <summary>
        /// Button 77
        /// </summary>
        Button77,

        /// <summary>
        /// Button 78
        /// </summary>
        Button78,

        /// <summary>
        /// Button 79
        /// </summary>
        Button79,

        /// <summary>
        /// Button 80
        /// </summary>
        Button80,

        /// <summary>
        /// Button 81
        /// </summary>
        Button81,

        /// <summary>
        /// Button 82
        /// </summary>
        Button82,

        /// <summary>
        /// Button 83
        /// </summary>
        Button83,

        /// <summary>
        /// Button 84
        /// </summary>
        Button84,

        /// <summary>
        /// Button 85
        /// </summary>
        Button85,

        /// <summary>
        /// Button 86
        /// </summary>
        Button86,

        /// <summary>
        /// Button 87
        /// </summary>
        Button87,

        /// <summary>
        /// Button 88
        /// </summary>
        Button88,

        /// <summary>
        /// Button 89
        /// </summary>
        Button89,

        /// <summary>
        /// Button 90
        /// </summary>
        Button90,

        /// <summary>
        /// Button 91
        /// </summary>
        Button91,

        /// <summary>
        /// Button 92
        /// </summary>
        Button92,

        /// <summary>
        /// Button 93
        /// </summary>
        Button93,

        /// <summary>
        /// Button 94
        /// </summary>
        Button94,

        /// <summary>
        /// Button 95
        /// </summary>
        Button95,

        /// <summary>
        /// Button 96
        /// </summary>
        Button96,

        /// <summary>
        /// Button 97
        /// </summary>
        Button97,

        /// <summary>
        /// Button 98
        /// </summary>
        Button98,

        /// <summary>
        /// Button 99
        /// </summary>
        Button99,

        /// <summary>
        /// Button 100
        /// </summary>
        Button100,

        /// <summary>
        /// Button 101
        /// </summary>
        Button101,

        /// <summary>
        /// Button 102
        /// </summary>
        Button102,

        /// <summary>
        /// Button 103
        /// </summary>
        Button103,

        /// <summary>
        /// Button 104
        /// </summary>
        Button104,

        /// <summary>
        /// Button 105
        /// </summary>
        Button105,

        /// <summary>
        /// Button 106
        /// </summary>
        Button106,

        /// <summary>
        /// Button 107
        /// </summary>
        Button107,

        /// <summary>
        /// Button 108
        /// </summary>
        Button108,

        /// <summary>
        /// Button 109
        /// </summary>
        Button109,

        /// <summary>
        /// Button 110
        /// </summary>
        Button110,

        /// <summary>
        /// Button 111
        /// </summary>
        Button111,

        /// <summary>
        /// Button 112
        /// </summary>
        Button112,

        /// <summary>
        /// Button 113
        /// </summary>
        Button113,

        /// <summary>
        /// Button 114
        /// </summary>
        Button114,

        /// <summary>
        /// Button 115
        /// </summary>
        Button115,

        /// <summary>
        /// Button 116
        /// </summary>
        Button116,

        /// <summary>
        /// Button 117
        /// </summary>
        Button117,

        /// <summary>
        /// Button 118
        /// </summary>
        Button118,

        /// <summary>
        /// Button 119
        /// </summary>
        Button119,

        /// <summary>
        /// Button 120
        /// </summary>
        Button120,

        /// <summary>
        /// Button 121
        /// </summary>
        Button121,

        /// <summary>
        /// Button 122
        /// </summary>
        Button122,

        /// <summary>
        /// Button 123
        /// </summary>
        Button123,

        /// <summary>
        /// Button 124
        /// </summary>
        Button124,

        /// <summary>
        /// Button 125
        /// </summary>
        Button125,

        /// <summary>
        /// Button 126
        /// </summary>
        Button126,

        /// <summary>
        /// Button 127
        /// </summary>
        Button127,
        #endregion
        #region Hats
        // -- POV Hats
        /// <summary>
        /// Hat 0 North
        /// </summary>
        Hat0N,
        /// <summary>
        /// Hat 0 South
        /// </summary>
        Hat0S,
        /// <summary>
        /// Hat 0 East
        /// </summary>
        Hat0E,
        /// <summary>
        /// Hat 0 West
        /// </summary>
        Hat0W,

        /// <summary>
        /// Hat 1 North
        /// </summary>
        Hat1N,
        /// <summary>
        /// Hat 1 South
        /// </summary>
        Hat1S,
        /// <summary>
        /// Hat 1 East
        /// </summary>
        Hat1E,
        /// <summary>
        /// Hat 1 West
        /// </summary>
        Hat1W,

        /// <summary>
        /// Hat 2 North
        /// </summary>
        Hat2N,
        /// <summary>
        /// Hat 2 South
        /// </summary>
        Hat2S,
        /// <summary>
        /// Hat 2 East
        /// </summary>
        Hat2E,
        /// <summary>
        /// Hat 2 West
        /// </summary>
        Hat2W,

        /// <summary>
        /// Hat 3 North
        /// </summary>
        Hat3N,
        /// <summary>
        /// Hat 3 South
        /// </summary>
        Hat3S,
        /// <summary>
        /// Hat 3 East
        /// </summary>
        Hat3E,
        /// <summary>
        /// Hat 3 West
        /// </summary>
        Hat3W,
        #endregion
        #region Axes
        /// <summary>
        /// Axis 0 Positive
        /// </summary>
        Axis0Positive,
        /// <summary>
        /// Axis 0 Negative
        /// </summary>
        Axis0Negative,
        /// <summary>
        /// Axis 0
        /// </summary>
        Axis0,

        /// <summary>
        /// Axis 1 Positive
        /// </summary>
        Axis1Positive,
        /// <summary>
        /// Axis 1 Negative
        /// </summary>
        Axis1Negative,
        /// <summary>
        /// Axis 1
        /// </summary>
        Axis1,

        /// <summary>
        /// Axis 2 Positive
        /// </summary>
        Axis2Positive,
        /// <summary>
        /// Axis 2 Negative
        /// </summary>
        Axis2Negative,
        /// <summary>
        /// Axis 2
        /// </summary>
        Axis2,

        /// <summary>
        /// Axis 3 Positive
        /// </summary>
        Axis3Positive,
        /// <summary>
        /// Axis 3 Negative
        /// </summary>
        Axis3Negative,
        /// <summary>
        /// Axis 3
        /// </summary>
        Axis3,

        /// <summary>
        /// Axis 4 Positive
        /// </summary>
        Axis4Positive,
        /// <summary>
        /// Axis 4 Negative
        /// </summary>
        Axis4Negative,
        /// <summary>
        /// Axis 4
        /// </summary>
        Axis4,

        /// <summary>
        /// Axis 5 Positive
        /// </summary>
        Axis5Positive,
        /// <summary>
        /// Axis 5 Negative
        /// </summary>
        Axis5Negative,
        /// <summary>
        /// Axis 5
        /// </summary>
        Axis5,

        /// <summary>
        /// Axis 6 Positive
        /// </summary>
        Axis6Positive,
        /// <summary>
        /// Axis 6 Negative
        /// </summary>
        Axis6Negative,
        /// <summary>
        /// Axis 6
        /// </summary>
        Axis6,

        /// <summary>
        /// Axis 7 Positive
        /// </summary>
        Axis7Positive,
        /// <summary>
        /// Axis 7 Negative
        /// </summary>
        Axis7Negative,
        /// <summary>
        /// Axis 7
        /// </summary>
        Axis7,
        #endregion
        #region Keyboard
        // -- Keyboard Keys

        /// <summary>
        /// The None Key
        /// </summary>
        KeyNone,

        /// <summary>
        /// The A Key
        /// </summary>
        KeyA,

        /// <summary>
        /// The B Key
        /// </summary>
        KeyB,

        /// <summary>
        /// The C Key
        /// </summary>
        KeyC,

        /// <summary>
        /// The D Key
        /// </summary>
        KeyD,

        /// <summary>
        /// The E Key
        /// </summary>
        KeyE,

        /// <summary>
        /// The F Key
        /// </summary>
        KeyF,

        /// <summary>
        /// The G Key
        /// </summary>
        KeyG,

        /// <summary>
        /// The H Key
        /// </summary>
        KeyH,

        /// <summary>
        /// The I Key
        /// </summary>
        KeyI,

        /// <summary>
        /// The J Key
        /// </summary>
        KeyJ,

        /// <summary>
        /// The K Key
        /// </summary>
        KeyK,

        /// <summary>
        /// The L Key
        /// </summary>
        KeyL,

        /// <summary>
        /// The M Key
        /// </summary>
        KeyM,

        /// <summary>
        /// The N Key
        /// </summary>
        KeyN,

        /// <summary>
        /// The O Key
        /// </summary>
        KeyO,

        /// <summary>
        /// The P Key
        /// </summary>
        KeyP,

        /// <summary>
        /// The Q Key
        /// </summary>
        KeyQ,

        /// <summary>
        /// The R Key
        /// </summary>
        KeyR,

        /// <summary>
        /// The S Key
        /// </summary>
        KeyS,

        /// <summary>
        /// The T Key
        /// </summary>
        KeyT,

        /// <summary>
        /// The U Key
        /// </summary>
        KeyU,

        /// <summary>
        /// The V Key
        /// </summary>
        KeyV,

        /// <summary>
        /// The W Key
        /// </summary>
        KeyW,

        /// <summary>
        /// The X Key
        /// </summary>
        KeyX,

        /// <summary>
        /// The Y Key
        /// </summary>
        KeyY,

        /// <summary>
        /// The Z Key
        /// </summary>
        KeyZ,

        /// <summary>
        /// The 0 Key
        /// </summary>
        Key0,

        /// <summary>
        /// The 1 Key
        /// </summary>
        Key1,

        /// <summary>
        /// The 2 Key
        /// </summary>
        Key2,

        /// <summary>
        /// The 3 Key
        /// </summary>
        Key3,

        /// <summary>
        /// The 4 Key
        /// </summary>
        Key4,

        /// <summary>
        /// The 5 Key
        /// </summary>
        Key5,

        /// <summary>
        /// The 6 Key
        /// </summary>
        Key6,

        /// <summary>
        /// The 7 Key
        /// </summary>
        Key7,

        /// <summary>
        /// The 8 Key
        /// </summary>
        Key8,

        /// <summary>
        /// The 9 Key
        /// </summary>
        Key9,

        /// <summary>
        /// The Equals (=) Key
        /// </summary>
        KeyEquals,

        /// <summary>
        /// The Minus (-) Key
        /// </summary>
        KeyMinus,

        /// <summary>
        /// The Backspace Key
        /// </summary>
        KeyBackspace,

        /// <summary>
        /// The Spacebar Key
        /// </summary>
        KeySpacebar,

        /// <summary>
        /// The Enter Key
        /// </summary>
        KeyEnter,

        /// <summary>
        /// The Up Key
        /// </summary>
        KeyUp,

        /// <summary>
        /// The Down Key
        /// </summary>
        KeyDown,

        /// <summary>
        /// The Left Key
        /// </summary>
        KeyLeft,

        /// <summary>
        /// The Right Key
        /// </summary>
        KeyRight,

        /// <summary>
        /// The Tab Key
        /// </summary>
        KeyTab,

        /// <summary>
        /// The Insert Key
        /// </summary>
        KeyInsert,

        /// <summary>
        /// The Delete Key
        /// </summary>
        KeyDelete,

        /// <summary>
        /// The Home Key
        /// </summary>
        KeyHome,

        /// <summary>
        /// The End Key
        /// </summary>
        KeyEnd,

        /// <summary>
        /// The PageUp Key
        /// </summary>
        KeyPageUp,

        /// <summary>
        /// The PageDown Key
        /// </summary>
        KeyPageDown,

        /// <summary>
        /// The Shift Key
        /// </summary>
        KeyShift,

        /// <summary>
        /// The Ctrl Key
        /// </summary>
        KeyCtrl,

        /// <summary>
        /// The Alt Key
        /// </summary>
        KeyAlt,

        /// <summary>
        /// The Escape Key
        /// </summary>
        KeyEscape,

        /// <summary>
        /// The Tilde (~) Key
        /// </summary>
        KeyTilde,

        /// <summary>
        /// The Quote (') Key
        /// </summary>
        KeyQuote,

        /// <summary>
        /// The Semicolon (;) Key
        /// </summary>
        KeySemicolon,

        /// <summary>
        /// The Comma (:) Key
        /// </summary>
        KeyComma,

        /// <summary>
        /// The Period (.) Key
        /// </summary>
        KeyPeriod,

        /// <summary>
        /// The Slash (/) Key
        /// </summary>
        KeySlash,

        /// <summary>
        /// The Left Bracket ([)Key
        /// </summary>
        KeyBracketLeft,

        /// <summary>
        /// The Right Brackey (]) Key
        /// </summary>
        KeyBracketRight,

        /// <summary>
        /// The Backslash (\) Key
        /// </summary>
        KeyBackslash,

        /// <summary>
        /// The Right Alt Key
        /// </summary>
        KeyRightAlt,

        /// <summary>
        /// The Right Ctrl Key
        /// </summary>
        KeyRightCtrl,

        /// <summary>
        /// The Right Shift Key
        /// </summary>
        KeyRightShift,

        /// <summary>
        /// The Num0 Key
        /// </summary>
        KeyNum0,

        /// <summary>
        /// The Num1 Key
        /// </summary>
        KeyNum1,

        /// <summary>
        /// The Num2 Key
        /// </summary>
        KeyNum2,

        /// <summary>
        /// The Num3 Key
        /// </summary>
        KeyNum3,

        /// <summary>
        /// The Num4 Key
        /// </summary>
        KeyNum4,

        /// <summary>
        /// The Num5 Key
        /// </summary>
        KeyNum5,

        /// <summary>
        /// The Num6 Key
        /// </summary>
        KeyNum6,

        /// <summary>
        /// The Num7 Key
        /// </summary>
        KeyNum7,

        /// <summary>
        /// The Num8 Key
        /// </summary>
        KeyNum8,

        /// <summary>
        /// The Num9 Key
        /// </summary>
        KeyNum9,

        /// <summary>
        /// The NumPeriod Key
        /// </summary>
        KeyNumPeriod,

        /// <summary>
        /// The NumPlus Key
        /// </summary>
        KeyNumPlus,

        /// <summary>
        /// The NumMinus Key
        /// </summary>
        KeyNumMinus,

        /// <summary>
        /// The NumEnter Key
        /// </summary>
        KeyNumEnter,

        /// <summary>
        /// The F1 Key
        /// </summary>
        KeyF1,

        /// <summary>
        /// The F2 Key
        /// </summary>
        KeyF2,

        /// <summary>
        /// The F3 Key
        /// </summary>
        KeyF3,

        /// <summary>
        /// The F4 Key
        /// </summary>
        KeyF4,

        /// <summary>
        /// The F5 Key
        /// </summary>
        KeyF5,

        /// <summary>
        /// The F6 Key
        /// </summary>
        KeyF6,

        /// <summary>
        /// The F7 Key
        /// </summary>
        KeyF7,

        /// <summary>
        /// The F8 Key
        /// </summary>
        KeyF8,

        /// <summary>
        /// The F9 Key
        /// </summary>
        KeyF9,

        /// <summary>
        /// The F10 Key
        /// </summary>
        KeyF10,

        /// <summary>
        /// The F11 Key
        /// </summary>
        KeyF11,

        /// <summary>
        /// The F12 Key
        /// </summary>
        KeyF12,
        #endregion
    }
}