using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Helper methods for device capability classes
    /// </summary>
    public static class DeviceCapabilityClasses
    {
        private static IReadOnlyList<DeviceCapability> _buttons = new List<DeviceCapability> {
            DeviceCapability.Button0,
            DeviceCapability.Button1,
            DeviceCapability.Button2,
            DeviceCapability.Button3,
            DeviceCapability.Button4,
            DeviceCapability.Button5,
            DeviceCapability.Button6,
            DeviceCapability.Button7,
            DeviceCapability.Button8,
            DeviceCapability.Button9,
            DeviceCapability.Button10,
            DeviceCapability.Button11,
            DeviceCapability.Button12,
            DeviceCapability.Button13,
            DeviceCapability.Button14,
            DeviceCapability.Button15,
            DeviceCapability.Button16,
            DeviceCapability.Button17,
            DeviceCapability.Button18,
            DeviceCapability.Button19,
            DeviceCapability.Button20,
            DeviceCapability.Button21,
            DeviceCapability.Button22,
            DeviceCapability.Button23,
            DeviceCapability.Button24,
            DeviceCapability.Button25,
            DeviceCapability.Button26,
            DeviceCapability.Button27,
            DeviceCapability.Button28,
            DeviceCapability.Button29,
            DeviceCapability.Button30,
            DeviceCapability.Button31,
            DeviceCapability.Button32,
            DeviceCapability.Button33,
            DeviceCapability.Button34,
            DeviceCapability.Button35,
            DeviceCapability.Button36,
            DeviceCapability.Button37,
            DeviceCapability.Button38,
            DeviceCapability.Button39,
            DeviceCapability.Button40,
            DeviceCapability.Button41,
            DeviceCapability.Button42,
            DeviceCapability.Button43,
            DeviceCapability.Button44,
            DeviceCapability.Button45,
            DeviceCapability.Button46,
            DeviceCapability.Button47,
            DeviceCapability.Button48,
            DeviceCapability.Button49,
            DeviceCapability.Button50,
            DeviceCapability.Button51,
            DeviceCapability.Button52,
            DeviceCapability.Button53,
            DeviceCapability.Button54,
            DeviceCapability.Button55,
            DeviceCapability.Button56,
            DeviceCapability.Button57,
            DeviceCapability.Button58,
            DeviceCapability.Button59,
            DeviceCapability.Button60,
            DeviceCapability.Button61,
            DeviceCapability.Button62,
            DeviceCapability.Button63,
            DeviceCapability.Button64,
            DeviceCapability.Button65,
            DeviceCapability.Button66,
            DeviceCapability.Button67,
            DeviceCapability.Button68,
            DeviceCapability.Button69,
            DeviceCapability.Button70,
            DeviceCapability.Button71,
            DeviceCapability.Button72,
            DeviceCapability.Button73,
            DeviceCapability.Button74,
            DeviceCapability.Button75,
            DeviceCapability.Button76,
            DeviceCapability.Button77,
            DeviceCapability.Button78,
            DeviceCapability.Button79,
            DeviceCapability.Button80,
            DeviceCapability.Button81,
            DeviceCapability.Button82,
            DeviceCapability.Button83,
            DeviceCapability.Button84,
            DeviceCapability.Button85,
            DeviceCapability.Button86,
            DeviceCapability.Button87,
            DeviceCapability.Button88,
            DeviceCapability.Button89,
            DeviceCapability.Button90,
            DeviceCapability.Button91,
            DeviceCapability.Button92,
            DeviceCapability.Button93,
            DeviceCapability.Button94,
            DeviceCapability.Button95,
            DeviceCapability.Button96,
            DeviceCapability.Button97,
            DeviceCapability.Button98,
            DeviceCapability.Button99,
            DeviceCapability.Button100,
            DeviceCapability.Button101,
            DeviceCapability.Button102,
            DeviceCapability.Button103,
            DeviceCapability.Button104,
            DeviceCapability.Button105,
            DeviceCapability.Button106,
            DeviceCapability.Button107,
            DeviceCapability.Button108,
            DeviceCapability.Button109,
            DeviceCapability.Button110,
            DeviceCapability.Button111,
            DeviceCapability.Button112,
            DeviceCapability.Button113,
            DeviceCapability.Button114,
            DeviceCapability.Button115,
            DeviceCapability.Button116,
            DeviceCapability.Button117,
            DeviceCapability.Button118,
            DeviceCapability.Button119,
            DeviceCapability.Button120,
            DeviceCapability.Button121,
            DeviceCapability.Button122,
            DeviceCapability.Button123,
            DeviceCapability.Button124,
            DeviceCapability.Button125,
            DeviceCapability.Button126,
            DeviceCapability.Button127,
        };
        private static IReadOnlyList<DeviceCapability> _axes = new List<DeviceCapability> {
            DeviceCapability.Axis0Positive,
            DeviceCapability.Axis0Negative,
            DeviceCapability.Axis0,
            DeviceCapability.Axis1Positive,
            DeviceCapability.Axis1Negative,
            DeviceCapability.Axis1,
            DeviceCapability.Axis2Positive,
            DeviceCapability.Axis2Negative,
            DeviceCapability.Axis2,
            DeviceCapability.Axis3Positive,
            DeviceCapability.Axis3Negative,
            DeviceCapability.Axis3,
            DeviceCapability.Axis4Positive,
            DeviceCapability.Axis4Negative,
            DeviceCapability.Axis4,
            DeviceCapability.Axis5Positive,
            DeviceCapability.Axis5Negative,
            DeviceCapability.Axis5,
            DeviceCapability.Axis6Positive,
            DeviceCapability.Axis6Negative,
            DeviceCapability.Axis6,
            DeviceCapability.Axis7Positive,
            DeviceCapability.Axis7Negative,
            DeviceCapability.Axis7,
        };
        private static IReadOnlyList<DeviceCapability> _hats = new List<DeviceCapability> {
            DeviceCapability.Hat0N,
            DeviceCapability.Hat0S,
            DeviceCapability.Hat0E,
            DeviceCapability.Hat0W,
            DeviceCapability.Hat1N,
            DeviceCapability.Hat1S,
            DeviceCapability.Hat1E,
            DeviceCapability.Hat1W,
            DeviceCapability.Hat2N,
            DeviceCapability.Hat2S,
            DeviceCapability.Hat2E,
            DeviceCapability.Hat2W,
            DeviceCapability.Hat3N,
            DeviceCapability.Hat3S,
            DeviceCapability.Hat3E,
            DeviceCapability.Hat3W,
        };
        private static IReadOnlyList<DeviceCapability> _keyboard = new List<DeviceCapability> {
            DeviceCapability.KeyA,
            DeviceCapability.KeyB,
            DeviceCapability.KeyC,
            DeviceCapability.KeyD,
            DeviceCapability.KeyE,
            DeviceCapability.KeyF,
            DeviceCapability.KeyG,
            DeviceCapability.KeyH,
            DeviceCapability.KeyI,
            DeviceCapability.KeyJ,
            DeviceCapability.KeyK,
            DeviceCapability.KeyL,
            DeviceCapability.KeyM,
            DeviceCapability.KeyN,
            DeviceCapability.KeyO,
            DeviceCapability.KeyP,
            DeviceCapability.KeyQ,
            DeviceCapability.KeyR,
            DeviceCapability.KeyS,
            DeviceCapability.KeyT,
            DeviceCapability.KeyU,
            DeviceCapability.KeyV,
            DeviceCapability.KeyW,
            DeviceCapability.KeyX,
            DeviceCapability.KeyY,
            DeviceCapability.KeyZ,
            DeviceCapability.Key0,
            DeviceCapability.Key1,
            DeviceCapability.Key2,
            DeviceCapability.Key3,
            DeviceCapability.Key4,
            DeviceCapability.Key5,
            DeviceCapability.Key6,
            DeviceCapability.Key7,
            DeviceCapability.Key8,
            DeviceCapability.Key9,
            DeviceCapability.KeyEquals,
            DeviceCapability.KeyMinus,
            DeviceCapability.KeyBackspace,
            DeviceCapability.KeySpacebar,
            DeviceCapability.KeyEnter,
            DeviceCapability.KeyUp,
            DeviceCapability.KeyDown,
            DeviceCapability.KeyLeft,
            DeviceCapability.KeyRight,
            DeviceCapability.KeyTab,
            DeviceCapability.KeyInsert,
            DeviceCapability.KeyDelete,
            DeviceCapability.KeyHome,
            DeviceCapability.KeyEnd,
            DeviceCapability.KeyPageUp,
            DeviceCapability.KeyPageDown,
            DeviceCapability.KeyShift,
            DeviceCapability.KeyCtrl,
            DeviceCapability.KeyAlt,
            DeviceCapability.KeyEscape,
            DeviceCapability.KeyTilde,
            DeviceCapability.KeyQuote,
            DeviceCapability.KeySemicolon,
            DeviceCapability.KeyComma,
            DeviceCapability.KeyPeriod,
            DeviceCapability.KeySlash,
            DeviceCapability.KeyBracketLeft,
            DeviceCapability.KeyBracketRight,
            DeviceCapability.KeyBackslash,
            DeviceCapability.KeyRightAlt,
            DeviceCapability.KeyRightCtrl,
            DeviceCapability.KeyRightShift,
            DeviceCapability.KeyNum0,
            DeviceCapability.KeyNum1,
            DeviceCapability.KeyNum2,
            DeviceCapability.KeyNum3,
            DeviceCapability.KeyNum4,
            DeviceCapability.KeyNum5,
            DeviceCapability.KeyNum6,
            DeviceCapability.KeyNum7,
            DeviceCapability.KeyNum8,
            DeviceCapability.KeyNum9,
            DeviceCapability.KeyNumPeriod,
            DeviceCapability.KeyNumPlus,
            DeviceCapability.KeyNumMinus,
            DeviceCapability.KeyNumEnter,
            DeviceCapability.KeyF1,
            DeviceCapability.KeyF2,
            DeviceCapability.KeyF3,
            DeviceCapability.KeyF4,
            DeviceCapability.KeyF5,
            DeviceCapability.KeyF6,
            DeviceCapability.KeyF7,
            DeviceCapability.KeyF8,
            DeviceCapability.KeyF9,
            DeviceCapability.KeyF10,
            DeviceCapability.KeyF11,
            DeviceCapability.KeyF12,
        };

        private static IReadOnlyList<DeviceCapability> _mouseButton = new List<DeviceCapability>
        {
            DeviceCapability.Mouse0,
            DeviceCapability.Mouse1,
            DeviceCapability.Mouse2,
            DeviceCapability.Mouse3,
            DeviceCapability.Mouse4,
        };

        private static IReadOnlyList<DeviceCapability> _mouseCursor = new List<DeviceCapability> 
        {
            DeviceCapability.CursorXPositive,
            DeviceCapability.CursorXNegative,
            DeviceCapability.CursorX,
            DeviceCapability.CursorYPositive,
            DeviceCapability.CursorYNegative,
            DeviceCapability.CursorY,
        };
        private static IReadOnlyList<DeviceCapability> _rumble = new List<DeviceCapability> {
            DeviceCapability.Rumble0,
            DeviceCapability.Rumble1
        };

        private static ImmutableHashSet<DeviceCapability> _hashKeyboardKeys
            = ImmutableHashSet.CreateRange(DeviceCapabilityClasses.Keyboard);

        private static ImmutableHashSet<DeviceCapability> _hashMouseCursor
           = ImmutableHashSet.CreateRange(DeviceCapabilityClasses.MouseCursor);

        private static ImmutableHashSet<DeviceCapability> _hashMouseButton
         = ImmutableHashSet.CreateRange(DeviceCapabilityClasses.MouseButtons);

        private static ImmutableHashSet<DeviceCapability> _hashAxes
          = ImmutableHashSet.CreateRange(DeviceCapabilityClasses.Axes);

        private static ImmutableHashSet<DeviceCapability> _hashButtons
          = ImmutableHashSet.CreateRange(DeviceCapabilityClasses.Buttons);

        private static ImmutableHashSet<DeviceCapability> _hashHats
          = ImmutableHashSet.CreateRange(DeviceCapabilityClasses.Hats);


        /// <summary>
        /// All button <see cref="DeviceCapability"/>.
        /// </summary>
        public static IEnumerable<DeviceCapability> Buttons => _buttons;

        /// <summary>
        /// All axis <see cref="DeviceCapability"/>.
        /// </summary>
        public static IEnumerable<DeviceCapability> Axes => _axes;

        /// <summary>
        /// All directional <see cref="DeviceCapability"/>.
        /// </summary>
        public static IEnumerable<DeviceCapability> Hats => _hats;

        /// <summary>
        /// All keyboard <see cref="DeviceCapability"/>.
        /// </summary>
        public static IEnumerable<DeviceCapability> Keyboard => _keyboard;

        /// <summary>
        /// All mouse  button<see cref="DeviceCapability"/>.
        /// </summary>
        public static IEnumerable<DeviceCapability> MouseButtons => _mouseButton;

        /// <summary>
        /// All mouse cursor <see cref="DeviceCapability"/>.
        /// </summary>
        public static IEnumerable<DeviceCapability> MouseCursor => _mouseCursor;

        /// <summary>
        /// All rumble <see cref="DeviceCapability"/>.
        /// </summary>
        public static IEnumerable<DeviceCapability> Rumble => _rumble;

        /// <summary>
        /// Gets the <see cref="DeviceCapability"/> for the specified button index.
        /// </summary>
        /// <param name="i">The index of the button</param>
        /// <returns>The button <see cref="DeviceCapability"/></returns>
        public static DeviceCapability GetButton(int i)
        {
            if (i >= _buttons.Count) return DeviceCapability.None;
            return _buttons[i];
        }

        /// <summary>
        /// Gets the set of <see cref="DeviceCapability"/> for the specified axis index.
        /// </summary>
        /// <param name="i">The index of the axis</param>
        /// <returns>The set of <see cref="DeviceCapability"/> for the axis</returns>
        public static IEnumerable<DeviceCapability> GetAxis(int i)
        {
            if (i >= 8) yield break;
            for (int j = 0; j < 3; j++)
            {
                yield return _axes[(i * 3) + j];
            }
        }
        /// <summary>
        /// Gets the set of <see cref="DeviceCapability"/> for the specified directional hat index.
        /// </summary>
        /// <param name="i">The index of the directional hat</param>
        /// <returns>The set of <see cref="DeviceCapability"/> for the hat</returns>
        public static IEnumerable<DeviceCapability> GetHat(int i)
        {
            if (i >= 3) yield break;
            for (int j = 0; j < 4; j++)
            {
                yield return _hats[(i * 4) + j];
            }
        }

        /// <summary>
        /// Gets the capability class for a <see cref="DeviceCapability"/>
        /// </summary>
        /// <param name="capability">The <see cref="DeviceCapability"/></param>
        /// <returns>The class of the <see cref="DeviceCapability"/></returns>
        public static DeviceCapabilityClass GetClass(DeviceCapability capability)
        {
            if (_hashAxes.Contains(capability)) return DeviceCapabilityClass.ControllerAxis;
            if (_hashButtons.Contains(capability)) return DeviceCapabilityClass.ControllerFaceButton;
            if (_hashHats.Contains(capability)) return DeviceCapabilityClass.ControllerDirectional;
            if (_hashKeyboardKeys.Contains(capability)) return DeviceCapabilityClass.Keyboard;
            if (_hashMouseButton.Contains(capability)) return DeviceCapabilityClass.MouseButton;
            if (_hashMouseCursor.Contains(capability)) return DeviceCapabilityClass.MouseCursor;

            // only 2 rumble, probably don't need to hash this.
            if (DeviceCapabilityClasses._rumble.Contains(capability)) return DeviceCapabilityClass.Rumble;

            return DeviceCapabilityClass.None;
        }
    }
}