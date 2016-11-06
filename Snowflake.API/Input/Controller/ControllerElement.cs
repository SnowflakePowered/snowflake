using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// An enumeration of Stone controller elements
    /// See https://github.com/SnowflakePowered/stone/blob/master/spec/Controllers.md
    /// for the specification.
    /// </summary>
    public enum ControllerElement
    {
        /// <summary>
        /// No element
        /// </summary>
        NoElement,

        /// <summary>
        /// The conventional 'A' or confirm face button in a controller
        /// </summary>
        ButtonA,

        /// <summary>
        /// The conventional 'B' or back face button in a controller
        /// </summary>
        ButtonB,

        /// <summary>
        /// The 'C'-labeled, or 3rd button in a 6-face button layout or similar
        /// </summary>
        ButtonC,

        /// <summary>
        /// The conventional 'X' button in a controller
        /// </summary>
        ButtonX,

        /// <summary>
        /// The conventional 'Y' button in a controller
        /// </summary>
        ButtonY,

        /// <summary>
        /// The 'Z'-labeled or 6th button in a 6-face button layout or similar
        /// </summary>
        ButtonZ,

        /// <summary>
        /// The shoulder button registering a digital signal on the left side of the controller
        /// </summary>
        ButtonL,

        /// <summary>
        /// The shoulder button registering a digital signal on the right side of the controller
        /// </summary>
        ButtonR,

        /// <summary>
        /// The traditional 'Start' button on a conventional controller that usually pauses or starts the game
        /// </summary>
        ButtonStart,

        /// <summary>
        /// The traditional 'Select' button on a conventional controller that provides auxillary functions
        /// </summary>
        ButtonSelect,

        /// <summary>
        /// A guide button featured on modern controllers that bring up a pause or guide overlay outside of the game itself
        /// </summary>
        ButtonGuide,

        /// <summary>
        /// A depression on the left analog stick that registers a digital signal. Usually labeled as L3
        /// </summary>
        ButtonClickL,

        /// <summary>
        /// A depression on the Right analog stick that registers a digital signal. Usually labeled as R3
        /// </summary>
        ButtonClickR,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button0,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button1,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button2,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button3,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button4,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button5,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button6,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button7,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button8,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons,
        /// or as a numeric pad key on certain controllers
        /// </summary>
        Button9,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button10,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button11,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button12,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button13,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button14,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button15,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button16,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button17,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button18,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button19,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button20,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button21,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button22,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button23,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button24,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button25,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button26,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button27,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button28,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button29,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button30,

        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        Button31,

        /// <summary>
        /// Directional button indicating the north or upwards direction
        /// </summary>
        DirectionalN,

        /// <summary>
        /// Directional button indicating the east or rightwards direction
        /// </summary>
        DirectionalE,

        /// <summary>
        /// Direction button indicating the south or downwards direction
        /// </summary>
        DirectionalS,

        /// <summary>
        /// Direction button indicating the west or leftwards direction
        /// </summary>
        DirectionalW,

        /// <summary>
        /// Auxillary directional button indicating the northeast, or upwards and right direction
        /// </summary>
        DirectionalNE,

        /// <summary>
        /// Auxillary directional button indicating the northwest, or upwards and left direction
        /// </summary>
        DirectionalNW,

        /// <summary>
        /// Auxillary directional button indicating the southeast, or downwards and right direction
        /// </summary>
        DirectionalSE,

        /// <summary>
        /// Auxillary directional button indicating the southwest, or downwards and left direction
        /// </summary>
        DirectionalSW,

        /// <summary>
        /// An analog shoulder trigger, able to be depressed smoothly with varying degrees of pressure
        /// from undepressed (0%) to fully depressed (100%), on the left side of the controller, usually marked L2
        /// </summary>
        TriggerLeft,

        /// <summary>
        /// An analog shoulder trigger, able to be depressed smoothly with varying degrees of pressure
        /// from undepressed (0%) to fully depressed (100%), on the right side of the controller, usually marked R2
        /// </summary>
        TriggerRight,

        /// <summary>
        /// Rightwards movement of the left analog stick along the X axis (horizontally)
        /// </summary>
        AxisLeftAnalogPositiveX,

        /// <summary>
        /// Leftwards movement of the left analog stick along the X axis (horizontally)
        /// </summary>
        AxisLeftAnalogNegativeX,

        /// <summary>
        /// Upwards movement of the left analog stick along the Y axis (vertically)
        /// </summary>
        AxisLeftAnalogPositiveY,

        /// <summary>
        /// Downwards movement of the left analog stick along the Y axis (vertically)
        /// </summary>
        AxisLeftAnalogNegativeY,

        /// <summary>
        /// Rightwards movement of the right analog stick along the X axis (horizontally)
        /// </summary>
        AxisRightAnalogPositiveX,

        /// <summary>
        /// Leftwards movement of the right analog stick along the X axis (horizontally)
        /// </summary>
        AxisRightAnalogNegativeX,

        /// <summary>
        /// Upwards movement of the right analog stick along the Y axis (vertically)	
        /// </summary>
        AxisRightAnalogPositiveY,

        /// <summary>
        /// Downwards movement of the right analog stick along the Y axis (vertically)
        /// </summary>
        AxisRightAnalogNegativeY,

        /// <summary>
        /// A large rumble action (usually through the larger of two rumble motors in a controller
        /// </summary>
        RumbleBig,

        /// <summary>
        /// A smaller rumble action (usually through the smaller of two rumble motors in a controller
        /// </summary>
        RumbleSmall,

        /// <summary>
        /// A pointing device that can express position in the form of a contiguous set of coordinates on a
        /// 2 dimensional cartesian plane. Examples include a mouse, or the Wii Remote IR
        /// </summary>
        Pointer2D,

        /// <summary>
        /// A pointing device that can express position in the form of a contiguous set of coordinates in 3 dimensional space.
        /// Examples include the Oculus Touch device, or the Playstation Move
        /// </summary>
        Pointer3D,

        /// <summary>
        /// Continous rightwards movement of a pointer device on the X axis (horizontal)
        /// </summary>
        PointerAxisPositiveX,

        /// <summary>
        /// Continous leftwards movement of a pointer device on the X axis (horizontal)
        /// </summary>
        PointerAxisNegativeX,

        /// <summary>
        /// Continous upwards movement of a pointer device on the Y axis (vertical)
        /// </summary>
        PointerAxisPositiveY,

        /// <summary>
        /// Continous downwards movement of a pointer device on the Y axis (vertical)
        /// </summary>
        PointerAxisNegativeY,

        /// <summary>
        /// Continous forwards movement of a pointer device on the Z axis
        /// </summary>
        PointerAxisPositiveZ,

        /// <summary>
        /// Continous backwards movement of a pointer device on the Z axis
        /// </summary>
        PointerAxisNegativeZ,

        /// <summary>
        /// A keyboad with an unspecified amount of keys. Intended for emulated computers such as the Commodore 64
        /// </summary>
        Keyboard,

        /// <summary>
        /// A touch sensitive surface of unspecified size and precision,
        /// where input can be expressed as a non-contiguous matrix of coordinates on a 2
        /// dimentional cartesian plane. However, most touchscreens in video game controllers
        /// are only concerned with a single matrix due to the lack of multi-touch
        /// </summary>
        Touchscreen,
        // The following elements are keyboard keys.
        // By convention, these keys mappable from K -> K.
        // Instead, they can only be mapped from K -> C where
        // C is a controller element. Doing otherwise is undefined behavior.
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

    /// <summary>
    /// Controller element extensions.
    /// </summary>
    public static class ControllerElementExtensions
    {
        /// <summary>
        /// Checks if the element is a keyboard key element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>Whether the element is a keyboard key element</returns>
        public static bool IsKeyboardKey(this ControllerElement element)
        {
            return element >= ControllerElement.KeyNone;
        }

        public static bool IsAxis(this ControllerElement element)
        {
            return Enum.GetName(typeof(ControllerElement), element).Contains("Axis");
        }
    }
}
