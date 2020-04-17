using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Controller;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.ControllerLayout
{
    public class ControllerElementEnum : EnumerationGraphType<ControllerElement>
    {
        public ControllerElementEnum()
        {
            Name = "ControllerElement";
            Description = "A Stone Controller Element";
            //AddValue(nameof(ControllerElement.NoElement), "No element.", ControllerElement.NoElement);
            //AddValue(nameof(ControllerElement.ButtonA), "The conventional 'A' or confirm face button in a controller.",
            //    ControllerElement.ButtonA);
            //AddValue(nameof(ControllerElement.ButtonB), "The conventional 'B' or back face button in a controller.",
            //    ControllerElement.ButtonB);
            //AddValue(nameof(ControllerElement.ButtonC),
            //    "The 'C'-labeled, or 3rd button in a 6-face button layout or similar.", ControllerElement.ButtonC);
            //AddValue(nameof(ControllerElement.ButtonX), "The conventional 'X' button in a controller.",
            //    ControllerElement.ButtonX);
            //AddValue(nameof(ControllerElement.ButtonY), "The conventional 'Y' button in a controller.",
            //    ControllerElement.ButtonY);
            //AddValue(nameof(ControllerElement.ButtonZ),
            //    "The 'Z'-labeled or 6th button in a 6-face button layout or similar.", ControllerElement.ButtonZ);
            //AddValue(nameof(ControllerElement.ButtonL),
            //    "The shoulder button registering a digital signal on the left side of the controller.",
            //    ControllerElement.ButtonL);
            //AddValue(nameof(ControllerElement.ButtonR),
            //    "The shoulder button registering a digital signal on the right side of the controller.",
            //    ControllerElement.ButtonR);
            //AddValue(nameof(ControllerElement.ButtonStart),
            //    "The traditional 'Start' button on a conventional controller that usually pauses or starts the game.",
            //    ControllerElement.ButtonStart);
            //AddValue(nameof(ControllerElement.ButtonSelect),
            //    "The traditional 'Select' button on a conventional controller that provides auxillary functions.",
            //    ControllerElement.ButtonSelect);
            //AddValue(nameof(ControllerElement.ButtonGuide),
            //    "A guide button featured on modern controllers that bring up a pause or guide overlay outside of the game itself.",
            //    ControllerElement.ButtonGuide);
            //AddValue(nameof(ControllerElement.ButtonClickL),
            //    "A depression on the left analog stick that registers a digital signal. Usually labeled as L3.",
            //    ControllerElement.ButtonClickL);
            //AddValue(nameof(ControllerElement.ButtonClickR),
            //    "A depression on the Right analog stick that registers a digital signal. Usually labeled as R3.",
            //    ControllerElement.ButtonClickR);
            //AddValue(nameof(ControllerElement.Button0),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button0);
            //AddValue(nameof(ControllerElement.Button1),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button1);
            //AddValue(nameof(ControllerElement.Button2),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button2);
            //AddValue(nameof(ControllerElement.Button3),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button3);
            //AddValue(nameof(ControllerElement.Button4),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button4);
            //AddValue(nameof(ControllerElement.Button5),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button5);
            //AddValue(nameof(ControllerElement.Button6),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button6);
            //AddValue(nameof(ControllerElement.Button7),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button7);
            //AddValue(nameof(ControllerElement.Button8),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button8);
            //AddValue(nameof(ControllerElement.Button9),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons, or as a numeric pad key on certain controllers.",
            //    ControllerElement.Button9);
            //AddValue(nameof(ControllerElement.Button10),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button10);
            //AddValue(nameof(ControllerElement.Button11),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button11);
            //AddValue(nameof(ControllerElement.Button12),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button12);
            //AddValue(nameof(ControllerElement.Button13),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button13);
            //AddValue(nameof(ControllerElement.Button14),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button14);
            //AddValue(nameof(ControllerElement.Button15),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button15);
            //AddValue(nameof(ControllerElement.Button16),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button16);
            //AddValue(nameof(ControllerElement.Button17),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button17);
            //AddValue(nameof(ControllerElement.Button18),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button18);
            //AddValue(nameof(ControllerElement.Button19),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button19);
            //AddValue(nameof(ControllerElement.Button20),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button20);
            //AddValue(nameof(ControllerElement.Button21),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button21);
            //AddValue(nameof(ControllerElement.Button22),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button22);
            //AddValue(nameof(ControllerElement.Button23),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button23);
            //AddValue(nameof(ControllerElement.Button24),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button24);
            //AddValue(nameof(ControllerElement.Button25),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button25);
            //AddValue(nameof(ControllerElement.Button26),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button26);
            //AddValue(nameof(ControllerElement.Button27),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button27);
            //AddValue(nameof(ControllerElement.Button28),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button28);
            //AddValue(nameof(ControllerElement.Button29),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button29);
            //AddValue(nameof(ControllerElement.Button30),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button30);
            //AddValue(nameof(ControllerElement.Button31),
            //    "Numbered button without semantic value. Used as a placeholder for buttons that do not fit within the other semantic buttons..",
            //    ControllerElement.Button31);
            //AddValue(nameof(ControllerElement.DirectionalN),
            //    "Directional button indicating the north or upwards direction.", ControllerElement.DirectionalN);
            //AddValue(nameof(ControllerElement.DirectionalE),
            //    "Directional button indicating the east or rightwards direction.", ControllerElement.DirectionalE);
            //AddValue(nameof(ControllerElement.DirectionalS),
            //    "Direction button indicating the south or downwards direction.", ControllerElement.DirectionalS);
            //AddValue(nameof(ControllerElement.DirectionalW),
            //    "Direction button indicating the west or leftwards direction.", ControllerElement.DirectionalW);
            //AddValue(nameof(ControllerElement.DirectionalNE),
            //    "Auxillary directional button indicating the northeast, or upwards and right direction.",
            //    ControllerElement.DirectionalNE);
            //AddValue(nameof(ControllerElement.DirectionalNW),
            //    "Auxillary directional button indicating the northwest, or upwards and left direction.",
            //    ControllerElement.DirectionalNW);
            //AddValue(nameof(ControllerElement.DirectionalSE),
            //    "Auxillary directional button indicating the southeast, or downwards and right direction.",
            //    ControllerElement.DirectionalSE);
            //AddValue(nameof(ControllerElement.DirectionalSW),
            //    "Auxillary directional button indicating the southwest, or downwards and left direction.",
            //    ControllerElement.DirectionalSW);
            //AddValue(nameof(ControllerElement.TriggerLeft),
            //    "An analog shoulder trigger, able to be depressed smoothly with varying degrees of pressure from undepressed (0%) to fully depressed (100%), on the left side of the controller, usually marked L2.",
            //    ControllerElement.TriggerLeft);
            //AddValue(nameof(ControllerElement.TriggerRight),
            //    "An analog shoulder trigger, able to be depressed smoothly with varying degrees of pressure from undepressed (0%) to fully depressed (100%), on the right side of the controller, usually marked R2.",
            //    ControllerElement.TriggerRight);
            //AddValue(nameof(ControllerElement.AxisLeftAnalogPositiveX),
            //    "Rightwards movement of the left analog stick along the X axis (horizontally).",
            //    ControllerElement.AxisLeftAnalogPositiveX);
            //AddValue(nameof(ControllerElement.AxisLeftAnalogNegativeX),
            //    "Leftwards movement of the left analog stick along the X axis (horizontally).",
            //    ControllerElement.AxisLeftAnalogNegativeX);
            //AddValue(nameof(ControllerElement.AxisLeftAnalogPositiveY),
            //    "Upwards movement of the left analog stick along the Y axis (vertically).",
            //    ControllerElement.AxisLeftAnalogPositiveY);
            //AddValue(nameof(ControllerElement.AxisLeftAnalogNegativeY),
            //    "Downwards movement of the left analog stick along the Y axis (vertically).",
            //    ControllerElement.AxisLeftAnalogNegativeY);
            //AddValue(nameof(ControllerElement.AxisRightAnalogPositiveX),
            //    "Rightwards movement of the right analog stick along the X axis (horizontally).",
            //    ControllerElement.AxisRightAnalogPositiveX);
            //AddValue(nameof(ControllerElement.AxisRightAnalogNegativeX),
            //    "Leftwards movement of the right analog stick along the X axis (horizontally).",
            //    ControllerElement.AxisRightAnalogNegativeX);
            //AddValue(nameof(ControllerElement.AxisRightAnalogPositiveY),
            //    "Upwards movement of the right analog stick along the Y axis (vertically).",
            //    ControllerElement.AxisRightAnalogPositiveY);
            //AddValue(nameof(ControllerElement.AxisRightAnalogNegativeY),
            //    "Downwards movement of the right analog stick along the Y axis (vertically).",
            //    ControllerElement.AxisRightAnalogNegativeY);
            //AddValue(nameof(ControllerElement.RumbleBig),
            //    "A large rumble action (usually through the larger of two rumble motors in a controller.",
            //    ControllerElement.RumbleBig);
            //AddValue(nameof(ControllerElement.RumbleSmall),
            //    "A smaller rumble action (usually through the smaller of two rumble motors in a controller.",
            //    ControllerElement.RumbleSmall);
            //AddValue(nameof(ControllerElement.Pointer2D),
            //    "A pointing device that can express position in the form of a contiguous set of coordinates on a 2 dimensional cartesian plane. Examples include a mouse, or the Wii Remote IR.",
            //    ControllerElement.Pointer2D);
            //AddValue(nameof(ControllerElement.Pointer3D),
            //    "A pointing device that can express position in the form of a contiguous set of coordinates in 3 dimensional space. Examples include the Oculus Touch device, or the Playstation Move.",
            //    ControllerElement.Pointer3D);
            //AddValue(nameof(ControllerElement.PointerAxisPositiveX),
            //    "Continous rightwards movement of a pointer device on the X axis (horizontal).",
            //    ControllerElement.PointerAxisPositiveX);
            //AddValue(nameof(ControllerElement.PointerAxisNegativeX),
            //    "Continous leftwards movement of a pointer device on the X axis (horizontal).",
            //    ControllerElement.PointerAxisNegativeX);
            //AddValue(nameof(ControllerElement.PointerAxisPositiveY),
            //    "Continous upwards movement of a pointer device on the Y axis (vertical).",
            //    ControllerElement.PointerAxisPositiveY);
            //AddValue(nameof(ControllerElement.PointerAxisNegativeY),
            //    "Continous downwards movement of a pointer device on the Y axis (vertical).",
            //    ControllerElement.PointerAxisNegativeY);
            //AddValue(nameof(ControllerElement.PointerAxisPositiveZ),
            //    "Continous forwards movement of a pointer device on the Z axis.",
            //    ControllerElement.PointerAxisPositiveZ);
            //AddValue(nameof(ControllerElement.PointerAxisNegativeZ),
            //    "Continous backwards movement of a pointer device on the Z axis.",
            //    ControllerElement.PointerAxisNegativeZ);
            //AddValue(nameof(ControllerElement.Keyboard),
            //    "A keyboad with an unspecified amount of keys. Intended for emulated computers such as the Commodore 64.",
            //    ControllerElement.Keyboard);
            //AddValue(nameof(ControllerElement.Touchscreen),
            //    "A touch sensitive surface of unspecified size and precision, where input can be expressed as a non-contiguous matrix of coordinates on a 2 dimentional cartesian plane. However, most touchscreens in video game controllers are only concerned with a single matrix due to the lack of multi-touch.",
            //    ControllerElement.Touchscreen);
        }
    }
}
