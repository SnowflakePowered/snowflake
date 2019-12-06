using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnumsNET;

namespace Snowflake.Input.Controller.Extensions
{
    /// <summary>
    /// Controller element extensions.
    /// </summary>
    public static class ControllerElementExtensions
    {
        /// <summary>
        /// Checks if the element is a keyboard key element.
        /// </summary>
        /// <param name="element">The controller element to check.</param>
        /// <returns>Whether the element is a keyboard key element</returns>
        public static bool IsKeyboardKey(this ControllerElement element)
        {
            return element >= ControllerElement.KeyNone;
        }

        /// <summary>
        /// Checks if the element is an axis element
        /// </summary>
        /// <param name="element">The controller element to check.</param>
        /// <returns>Whether the element is a keyboard key element</returns>
        public static bool IsAxis(this ControllerElement element)
        {
            return element.GetMember()!.Name.Contains("Axis");
        }
    }
}
