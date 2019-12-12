using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Snowflake.Input.Device
{
    public static class DeviceCapabilityExtensions
    {
        private static ImmutableHashSet<DeviceCapability> _keyboardKeys
            = ImmutableHashSet.CreateRange(DeviceCapabilityClasses.Keyboard);

        private static ImmutableHashSet<DeviceCapability> _mouse
           = ImmutableHashSet.CreateRange(DeviceCapabilityClasses.Mouse);

        private static ImmutableHashSet<DeviceCapability> _axes
          = ImmutableHashSet.CreateRange(DeviceCapabilityClasses.Axes);

        /// <summary>
        /// Checks if the element is a keyboard key element.
        /// </summary>
        /// <param name="element">The controller element to check.</param>
        /// <returns>Whether the element is a keyboard key element</returns>
        public static bool IsKeyboardKey(this DeviceCapability element)
        {
            return _keyboardKeys.Contains(element) || _mouse.Contains(element);
        }

        /// <summary>
        /// Checks if the element is an axis element
        /// </summary>
        /// <param name="element">The controller element to check.</param>
        /// <returns>Whether the element is a keyboard key element</returns>
        public static bool IsAxis(this DeviceCapability element)
        {
            return _axes.Contains(element);
        }
    }
}
