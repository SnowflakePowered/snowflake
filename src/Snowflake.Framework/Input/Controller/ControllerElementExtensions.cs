using EnumsNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Extensions to <see cref="ControllerElement"/>
    /// </summary>
    public static class ControllerElementExtensions
    {
        /// <summary>
        /// Parses a <see cref="ControllerElement"/> from a string.
        /// </summary>
        /// <param name="controllerElement">The string representation of the controller element.</param>
        /// <returns>A <see cref="ControllerElement"/> parsed from the string.</returns>
        /// <exception cref="ArgumentException">If the string is unable to be parsed.</exception>
        public static ControllerElement Parse(string controllerElement)
        {
            return Enums.Parse<ControllerElement>(controllerElement, true);
        }

        /// <summary>
        /// Gets the string representation of the <see cref="ControllerElement"/>.
        /// </summary>
        /// <param name="this">The <see cref="ControllerElement"/> to parse.</param>
        /// <returns>The string representation of the <see cref="ControllerElement"/>.</returns>
        public static string ToString(this ControllerElement @this)
        {
            return Enums.AsString(@this);
        }
    }
}
