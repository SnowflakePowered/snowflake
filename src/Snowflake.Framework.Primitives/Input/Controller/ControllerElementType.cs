using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Every Stone controller element can have one of any of the following types.
    /// This means a face button can be treated as a directional button, etc,
    /// and serves as hinting outside of the semantic meaning of the element keys themselves to the consumer.
    /// </summary>
    public enum ControllerElementType
    {
        /// <summary>
        /// This element does not exist. Intended for internal use for consumers
        /// </summary>
        Null,

        /// <summary>
        /// A button, usually with a switch or dome that can be depressed on the controller, exclusing directional buttons
        /// </summary>
        Button,

        /// <summary>
        /// A directional button or D-pad button representing one of 8 cardinal directions
        /// </summary>
        Directional,

        /// <summary>
        /// An axis that increases in value on the number line.
        /// For example, analog stick movement towards the right, or towards the forward direction.
        /// </summary>
        AxisPositive,

        /// <summary>
        /// An axis that decreases in value on the number line.
        /// For example, analog stick movement towards the left, or towards the backwards direction.
        /// </summary>
        AxisNegative,

        /// <summary>
        /// An analog shoulder trigger, able to be depressed smoothly with varying degrees of pressure
        /// from undepressed (0%) to fully depressed (100%), usually marked L2 or R2
        /// </summary>
        Trigger,

        /// <summary>
        /// A rumble motor that shakes the controller
        /// </summary>
        Rumble,

        /// <summary>
        /// A keyboard of an unspecified amount of keys. Intended for special-case keyboard handling for consumers
        /// </summary>
        Keyboard,

        /// <summary>
        /// A pointing device that can express position in the form of a contiguous set of coordinates on a multi-dimensional cartesian plane.
        /// Examples include a mouse, or the Wii Remote IR for 2D.
        /// </summary>
        Pointer,

        /// <summary>
        /// A pointer axis that increases in value on the number line, where the pointer device itself is
        /// not limited to axis-based representation. For example, continous right mouse movement.
        /// </summary>
        PointerAxisPositive,

        /// <summary>
        /// A pointer axis that increases in value on the number line, where the pointer device itself
        /// is not limited to axis-based representation. For example, continous left mouse movement.
        /// </summary>
        PointerAxisNegative,

        /// <summary>
        /// A touch sensitive surface of unspecified size and precision,
        /// where input can be expressed as a non-contiguous matrix of coordinates on a
        /// 2 dimentional cartesian plane. However, most touchscreens in video game controllers
        /// are only concerned with a single matrix due to the lack of multi-touch support.
        /// </summary>
        Touchscreen,

        /// <summary>
        /// A 3 axis rotation gyroscope of unspecified precision, 
        /// where input can be expressed as a vector of 3 coordinates X, Y, and Z
        /// </summary>
        Gyroscope,
    }
}
