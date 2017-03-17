using EnumsNET;
using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Utility
{
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
            return element.GetMember().Name.Contains("Axis");
        }
    }
}
