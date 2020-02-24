using EnumsNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Extensions to <see cref="ControllerElementType"/>
    /// </summary>
    public static class ControllerElementTypeExtensions
    {
        /// <summary>
        /// Parses a <see cref="ControllerElementType"/> from a string.
        /// </summary>
        /// <param name="controllerElement">The string representation of the controller element.</param>
        /// <returns>A <see cref="ControllerElementType"/> parsed from the string.</returns>
        /// <exception cref="ArgumentException">If the string is unable to be parsed.</exception>
        public static ControllerElementType Parse(string controllerElement)
        {
            return Enums.Parse<ControllerElementType>(controllerElement, true);
        }

        /// <summary>
        /// Gets the string representation of the <see cref="ControllerElementType"/>.
        /// </summary>
        /// <param name="this">The <see cref="ControllerElementType"/> to parse.</param>
        /// <returns>The string representation of the <see cref="ControllerElementType"/>.</returns>
        public static string ToString(this ControllerElementType @this)
        {
            return Enums.AsString(@this);
        }
    }
}
