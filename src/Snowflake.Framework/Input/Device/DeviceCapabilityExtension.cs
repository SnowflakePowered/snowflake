using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Extensions to <see cref="DeviceCapability"/>
    /// </summary>
    public static class DeviceCapabilityExtensions
    {
        /// <summary>
        /// Gets the capability class of this <see cref="DeviceCapability"/>
        /// </summary>
        /// <param name="this">The <see cref="DeviceCapability"/></param>
        /// <returns>The capability class of the capability.</returns>
        public static DeviceCapabilityClass GetClass(this DeviceCapability @this)
            => DeviceCapabilityClasses.GetClass(@this);
    }
}
