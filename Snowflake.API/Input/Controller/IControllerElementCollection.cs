using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Represents a collection of controller elements in a layout
    /// </summary>
    public interface IControllerElementCollection : IEnumerable<KeyValuePair<ControllerElement, IControllerElementInfo>>
    {
        /// <summary>
        /// The conventional 'A' or confirm face button in a controller
        /// </summary>
        IControllerElementInfo ButtonA { get; }
        /// <summary>
        /// The conventional 'B' or back face button in a controller
        /// </summary>
        IControllerElementInfo ButtonB { get; }
        /// <summary>
        /// The 'C'-labeled, or 3rd button in a 6-face button layout or similar
        /// </summary>
        IControllerElementInfo ButtonC { get; }
        /// <summary>
        /// The conventional 'X' button in a controller
        /// </summary>
        IControllerElementInfo ButtonX { get; }
        /// <summary>
        /// The conventional 'Y' button in a controller
        /// </summary>
        IControllerElementInfo ButtonY { get; }
        /// <summary>
        /// The 'Z'-labeled or 6th button in a 6-face button layout or similar
        /// </summary>
        IControllerElementInfo ButtonZ { get; }
        /// <summary>
        /// The shoulder button registering a digital signal on the left side of the controller
        /// </summary>
        IControllerElementInfo ButtonL { get; }
        /// <summary>
        /// The shoulder button registering a digital signal on the right side of the controller
        /// </summary>
        IControllerElementInfo ButtonR { get; }
        /// <summary>
        /// The traditional 'Start' button on a conventional controller that usually pauses or starts the game
        /// </summary>
        IControllerElementInfo ButtonStart { get; }
        /// <summary>
        /// The traditional 'Select' button on a conventional controller that provides auxillary functions
        /// </summary>
        IControllerElementInfo ButtonSelect { get; }
        /// <summary>
        /// A guide button featured on modern controllers that bring up a pause or guide overlay outside of the game itself
        /// </summary>
        IControllerElementInfo ButtonGuide { get; }
        /// <summary>
        /// A depression on the left analog stick that registers a digital signal. Usually labeled as L3
        /// </summary>
        IControllerElementInfo ButtonClickL { get; }
        /// <summary>
        /// A depression on the Right analog stick that registers a digital signal. Usually labeled as R3
        /// </summary>
        IControllerElementInfo ButtonClickR { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button0 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button1 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button2 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button3 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button4 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button5 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button6 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button7 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button8 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit
        /// within the other semantic buttons { get; }
        /// or as a numeric pad key on certain controllers
        /// </summary>
        IControllerElementInfo Button9 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button10 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button11 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button12 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button13 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button14 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button15 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button16 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button17 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button18 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button19 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button20 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button21 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button22 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button23 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button24 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button25 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button26 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button27 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button28 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button29 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button30 { get; }
        /// <summary>
        /// Numbered button without semantic value.
        /// Used as a placeholder for buttons that do not fit within the other semantic buttons.
        /// </summary>
        IControllerElementInfo Button31 { get; }
        /// <summary>
        /// Directional button indicating the north or upwards direction
        /// </summary>
        IControllerElementInfo DirectionalN { get; }
        /// <summary>
        /// Directional button indicating the east or rightwards direction
        /// </summary>
        IControllerElementInfo DirectionalE { get; }
        /// <summary>
        /// Direction button indicating the south or downwards direction
        /// </summary>
        IControllerElementInfo DirectionalS { get; }
        /// <summary>
        /// Direction button indicating the west or leftwards direction
        /// </summary>
        IControllerElementInfo DirectionalW { get; }
        /// <summary>
        /// Auxillary directional button indicating the northeast, or upwards and right direction
        /// </summary>
        IControllerElementInfo DirectionalNE { get; }
        /// <summary>
        /// Auxillary directional button indicating the northwest, or upwards and left direction
        /// </summary>
        IControllerElementInfo DirectionalNW { get; }
        /// <summary>
        /// Auxillary directional button indicating the southeast, or downwards and right direction
        /// </summary>
        IControllerElementInfo DirectionalSE { get; }
        /// <summary>
        /// Auxillary directional button indicating the southwest, or downwards and left direction
        /// </summary>
        IControllerElementInfo DirectionalSW { get; }
        /// <summary>
        /// An analog shoulder trigger, able to be depressed smoothly with varying degrees of pressure
        /// from undepressed (0%) to fully depressed (100%), on the left side of the controller, usually marked L2
        /// </summary>
        IControllerElementInfo TriggerLeft { get; }
        /// <summary>
        /// An analog shoulder trigger, able to be depressed smoothly with varying degrees of pressure
        /// from undepressed (0%) to fully depressed (100%), on the right side of the controller, usually marked R2
        /// </summary>
        IControllerElementInfo TriggerRight { get; }
        /// <summary>
        /// Rightwards movement of the left analog stick along the X axis (horizontally)
        /// </summary>
        IControllerElementInfo AxisLeftAnalogPositiveX { get; }
        /// <summary>
        /// Leftwards movement of the left analog stick along the X axis (horizontally)
        /// </summary>
        IControllerElementInfo AxisLeftAnalogNegativeX { get; }
        /// <summary>
        /// Upwards movement of the left analog stick along the Y axis (vertically)
        /// </summary>
        IControllerElementInfo AxisLeftAnalogPositiveY { get; }
        /// <summary>
        /// Downwards movement of the left analog stick along the Y axis (vertically)
        /// </summary>
        IControllerElementInfo AxisLeftAnalogNegativeY { get; }
        /// <summary>
        /// Rightwards movement of the right analog stick along the X axis (horizontally)
        /// </summary>
        IControllerElementInfo AxisRightAnalogPositiveX { get; }
        /// <summary>
        /// Leftwards movement of the right analog stick along the X axis (horizontally)
        /// </summary>
        IControllerElementInfo AxisRightAnalogNegativeX { get; }
        /// <summary>
        /// Upwards movement of the right analog stick along the Y axis (vertically)	
        /// </summary>
        IControllerElementInfo AxisRightAnalogPositiveY { get; }
        /// <summary>
        /// Downwards movement of the right analog stick along the Y axis (vertically)
        /// </summary>
        IControllerElementInfo AxisRightAnalogNegativeY { get; }
        /// <summary>
        /// A large rumble action (usually through the larger of two rumble motors in a controller
        /// </summary>
        IControllerElementInfo RumbleBig { get; }
        /// <summary>
        /// A smaller rumble action (usually through the smaller of two rumble motors in a controller
        /// </summary>
        IControllerElementInfo RumbleSmall { get; }
        /// <summary>
        /// A pointing device that can express position in the form of a contiguous set of coordinates on a
        /// 2 dimensional cartesian plane. Examples include a mouse, or the Wii Remote IR
        /// </summary>
        IControllerElementInfo Pointer2D { get; }
        /// <summary>
        /// A pointing device that can express position in the form of a contiguous set of coordinates in 3 dimensional space.
        /// Examples include the Oculus Touch device, or the Playstation Move
        /// </summary>
        IControllerElementInfo Pointer3D { get; }
        /// <summary>
        /// Continous rightwards movement of a pointer device on the X axis (horizontal)
        /// </summary>
        IControllerElementInfo PointerAxisPositiveX { get; }
        /// <summary>
        /// Continous leftwards movement of a pointer device on the X axis (horizontal)
        /// </summary>
        IControllerElementInfo PointerAxisNegativeX { get; }
        /// <summary>
        /// Continous upwards movement of a pointer device on the Y axis (vertical)
        /// </summary>
        IControllerElementInfo PointerAxisPositiveY { get; }
        /// <summary>
        /// Continous downwards movement of a pointer device on the Y axis (vertical)
        /// </summary>
        IControllerElementInfo PointerAxisNegativeY { get; }
        /// <summary>
        /// Continous forwards movement of a pointer device on the Z axis
        /// </summary>
        IControllerElementInfo PointerAxisPositiveZ { get; }
        /// <summary>
        /// Continous backwards movement of a pointer device on the Z axis
        /// </summary>
        IControllerElementInfo PointerAxisNegativeZ { get; }
        /// <summary>
        /// A keyboad with an unspecified amount of keys. Intended for emulated computers such as the Commodore 64
        /// </summary>
        IControllerElementInfo Keyboard { get; }
        /// <summary>
        /// A touch sensitive surface of unspecified size and precision { get; }
        /// where input can be expressed as a non-contiguous matrix of coordinates on a 2
        /// dimentional cartesian plane. However, most touchscreens in video game controllers
        /// are only concerned with a single matrix due to the lack of multi-touch
        /// </summary>
        IControllerElementInfo Touchscreen { get; }
        /// <summary>
        /// Indexer accessor for the elements in this collection.
        /// If not present, should return null.
        /// </summary>
        /// <param name="controllerElement">The controller element to lookup</param>
        /// <returns>The element info for this layout</returns>
        IControllerElementInfo this[ControllerElement controllerElement] { get; }
    }
}
