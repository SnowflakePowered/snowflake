using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Controller;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.ControllerLayout
{
    public class ControllerElementTypeEnum : EnumerationGraphType<ControllerElementType>
    {
        public ControllerElementTypeEnum()
        {
            Name = "ControllerElementType";
            Description = "A Stone Controller Element Type";
            //AddValue(nameof(ControllerElementType.Null),
            //    "This element does not exist. Intended for internal use for consumers.", ControllerElementType.Null);
            //AddValue(nameof(ControllerElementType.Button),
            //    "A button, usually with a switch or dome that can be depressed on the controller, exclusing directional buttons.",
            //    ControllerElementType.Button);
            //AddValue(nameof(ControllerElementType.Directional),
            //    "A directional button or D-pad button representing one of 8 cardinal directions.",
            //    ControllerElementType.Directional);
            //AddValue(nameof(ControllerElementType.AxisPositive),
            //    "An axis that increases in value on the number line. For example, analog stick movement towards the right, or towards the forward direction..",
            //    ControllerElementType.AxisPositive);
            //AddValue(nameof(ControllerElementType.AxisNegative),
            //    "An axis that decreases in value on the number line. For example, analog stick movement towards the left, or towards the backwards direction..",
            //    ControllerElementType.AxisNegative);
            //AddValue(nameof(ControllerElementType.Trigger),
            //    "An analog shoulder trigger, able to be depressed smoothly with varying degrees of pressure from undepressed (0%) to fully depressed (100%), usually marked L2 or R2.",
            //    ControllerElementType.Trigger);
            //AddValue(nameof(ControllerElementType.Rumble), "A rumble motor that shakes the controller.",
            //    ControllerElementType.Rumble);
            //AddValue(nameof(ControllerElementType.Keyboard),
            //    "A keyboard of an unspecified amount of keys. Intended for special-case keyboard handling for consumers.",
            //    ControllerElementType.Keyboard);
            //AddValue(nameof(ControllerElementType.Pointer),
            //    "A pointing device that can express position in the form of a contiguous set of coordinates on a multi-dimensional cartesian plane. Examples include a mouse, or the Wii Remote IR for 2D.",
            //    ControllerElementType.Pointer);
            //AddValue(nameof(ControllerElementType.PointerAxisPositive),
            //    "A pointer axis that increases in value on the number line, where the pointer device itself is not limited to axis-based representation. For example, continous right mouse movement.",
            //    ControllerElementType.PointerAxisPositive);
            //AddValue(nameof(ControllerElementType.PointerAxisNegative),
            //    "A pointer axis that increases in value on the number line, where the pointer device itself is not limited to axis-based representation. For example, continous left mouse movement.",
            //    ControllerElementType.PointerAxisNegative);
            //AddValue(nameof(ControllerElementType.Touchscreen),
            //    "A touch sensitive surface of unspecified size and precision, where input can be expressed as a non-contiguous matrix of coordinates on a 2 dimentional cartesian plane. However, most touchscreens in video game controllers are only concerned with a single matrix due to the lack of multi-touch support.",
            //    ControllerElementType.Touchscreen);
        }
    }
}
