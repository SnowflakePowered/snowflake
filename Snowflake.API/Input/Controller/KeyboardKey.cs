using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Represents a standard keyboard
    /// </summary>
    public enum KeyboardKey
    {
        ///<summary>
        /// The None Key
        ///</summary>
        KeyNone,
        ///<summary>
        /// The A Key
        ///</summary>
        KeyA,
        ///<summary>
        /// The B Key
        ///</summary>
        KeyB,
        ///<summary>
        /// The C Key
        ///</summary>
        KeyC,
        ///<summary>
        /// The D Key
        ///</summary>
        KeyD,
        ///<summary>
        /// The E Key
        ///</summary>
        KeyE,
        ///<summary>
        /// The F Key
        ///</summary>
        KeyF,
        ///<summary>
        /// The G Key
        ///</summary>
        KeyG,
        ///<summary>
        /// The H Key
        ///</summary>
        KeyH,
        ///<summary>
        /// The I Key
        ///</summary>
        KeyI,
        ///<summary>
        /// The J Key
        ///</summary>
        KeyJ,
        ///<summary>
        /// The K Key
        ///</summary>
        KeyK,
        ///<summary>
        /// The L Key
        ///</summary>
        KeyL,
        ///<summary>
        /// The M Key
        ///</summary>
        KeyM,
        ///<summary>
        /// The N Key
        ///</summary>
        KeyN,
        ///<summary>
        /// The O Key
        ///</summary>
        KeyO,
        ///<summary>
        /// The P Key
        ///</summary>
        KeyP,
        ///<summary>
        /// The Q Key
        ///</summary>
        KeyQ,
        ///<summary>
        /// The R Key
        ///</summary>
        KeyR,
        ///<summary>
        /// The S Key
        ///</summary>
        KeyS,
        ///<summary>
        /// The T Key
        ///</summary>
        KeyT,
        ///<summary>
        /// The U Key
        ///</summary>
        KeyU,
        ///<summary>
        /// The V Key
        ///</summary>
        KeyV,
        ///<summary>
        /// The W Key
        ///</summary>
        KeyW,
        ///<summary>
        /// The X Key
        ///</summary>
        KeyX,
        ///<summary>
        /// The Y Key
        ///</summary>
        KeyY,
        ///<summary>
        /// The Z Key
        ///</summary>
        KeyZ,
        ///<summary>
        /// The 0 Key
        ///</summary>
        Key0,
        ///<summary>
        /// The 1 Key
        ///</summary>
        Key1,
        ///<summary>
        /// The 2 Key
        ///</summary>
        Key2,
        ///<summary>
        /// The 3 Key
        ///</summary>
        Key3,
        ///<summary>
        /// The 4 Key
        ///</summary>
        Key4,
        ///<summary>
        /// The 5 Key
        ///</summary>
        Key5,
        ///<summary>
        /// The 6 Key
        ///</summary>
        Key6,
        ///<summary>
        /// The 7 Key
        ///</summary>
        Key7,
        ///<summary>
        /// The 8 Key
        ///</summary>
        Key8,
        ///<summary>
        /// The 9 Key
        ///</summary>
        Key9,
        ///<summary>
        /// The Equals (=) Key
        ///</summary>
        KeyEquals,
        ///<summary>
        /// The Minus (-) Key
        ///</summary>
        KeyMinus,
        ///<summary>
        /// The Backspace Key
        ///</summary>
        KeyBackspace,
        ///<summary>
        /// The Spacebar Key
        ///</summary>
        KeySpacebar,
        ///<summary>
        /// The Enter Key
        ///</summary>
        KeyEnter,
        ///<summary>
        /// The Up Key
        ///</summary>
        KeyUp,
        ///<summary>
        /// The Down Key
        ///</summary>
        KeyDown,
        ///<summary>
        /// The Left Key
        ///</summary>
        KeyLeft,
        ///<summary>
        /// The Right Key
        ///</summary>
        KeyRight,
        ///<summary>
        /// The Tab Key
        ///</summary>
        KeyTab,
        ///<summary>
        /// The Insert Key
        ///</summary>
        KeyInsert,
        ///<summary>
        /// The Delete Key
        ///</summary>
        KeyDelete,
        ///<summary>
        /// The Home Key
        ///</summary>
        KeyHome,
        ///<summary>
        /// The End Key
        ///</summary>
        KeyEnd,
        ///<summary>
        /// The PageUp Key
        ///</summary>
        KeyPageUp,
        ///<summary>
        /// The PageDown Key
        ///</summary>
        KeyPageDown,
        ///<summary>
        /// The Shift Key
        ///</summary>
        KeyShift,
        ///<summary>
        /// The Ctrl Key
        ///</summary>
        KeyCtrl,
        ///<summary>
        /// The Alt Key
        ///</summary>
        KeyAlt,
        ///<summary>
        /// The Escape Key
        ///</summary>
        KeyEscape,
        ///<summary>
        /// The Tilde (~) Key
        ///</summary>
        KeyTilde,
        ///<summary>
        /// The Quote (') Key
        ///</summary>
        KeyQuote,
        ///<summary>
        /// The Semicolon (;) Key
        ///</summary>
        KeySemicolon,
        ///<summary>
        /// The Comma (:) Key
        ///</summary>
        KeyComma,
        ///<summary>
        /// The Period (.) Key
        ///</summary>
        KeyPeriod,
        ///<summary>
        /// The Slash (/) Key
        ///</summary>
        KeySlash,
        ///<summary>
        /// The Left Bracket ([)Key
        ///</summary>
        KeyBracketLeft,
        ///<summary>
        /// The Right Brackey (]) Key
        ///</summary>
        KeyBracketRight,
        ///<summary>
        /// The Backslash (\) Key
        ///</summary>
        KeyBackslash,
        ///<summary>
        /// The Right Alt Key
        ///</summary>
        KeyRightAlt,
        ///<summary>
        /// The Right Ctrl Key
        ///</summary>
        KeyRightCtrl,
        ///<summary>
        /// The Right Shift Key
        ///</summary>
        KeyRightShift,
        ///<summary>
        /// The Num0 Key
        ///</summary>
        KeyNum0,
        ///<summary>
        /// The Num1 Key
        ///</summary>
        KeyNum1,
        ///<summary>
        /// The Num2 Key
        ///</summary>
        KeyNum2,
        ///<summary>
        /// The Num3 Key
        ///</summary>
        KeyNum3,
        ///<summary>
        /// The Num4 Key
        ///</summary>
        KeyNum4,
        ///<summary>
        /// The Num5 Key
        ///</summary>
        KeyNum5,
        ///<summary>
        /// The Num6 Key
        ///</summary>
        KeyNum6,
        ///<summary>
        /// The Num7 Key
        ///</summary>
        KeyNum7,
        ///<summary>
        /// The Num8 Key
        ///</summary>
        KeyNum8,
        ///<summary>
        /// The Num9 Key
        ///</summary>
        KeyNum9,
        ///<summary>
        /// The NumPeriod Key
        ///</summary>
        KeyNumPeriod,
        ///<summary>
        /// The NumPlus Key
        ///</summary>
        KeyNumPlus,
        ///<summary>
        /// The NumMinus Key
        ///</summary>
        KeyNumMinus,
        ///<summary>
        /// The NumEnter Key
        ///</summary>
        KeyNumEnter,
        ///<summary>
        /// The F1 Key
        ///</summary>
        KeyF1,
        ///<summary>
        /// The F2 Key
        ///</summary>
        KeyF2,
        ///<summary>
        /// The F3 Key
        ///</summary>
        KeyF3,
        ///<summary>
        /// The F4 Key
        ///</summary>
        KeyF4,
        ///<summary>
        /// The F5 Key
        ///</summary>
        KeyF5,
        ///<summary>
        /// The F6 Key
        ///</summary>
        KeyF6,
        ///<summary>
        /// The F7 Key
        ///</summary>
        KeyF7,
        ///<summary>
        /// The F8 Key
        ///</summary>
        KeyF8,
        ///<summary>
        /// The F9 Key
        ///</summary>
        KeyF9,
        ///<summary>
        /// The F10 Key
        ///</summary>
        KeyF10,
        ///<summary>
        /// The F11 Key
        ///</summary>
        KeyF11,
        ///<summary>
        /// The F12 Key
        ///</summary>
        KeyF12,
    }
}