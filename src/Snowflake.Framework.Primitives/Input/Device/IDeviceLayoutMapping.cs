using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Represents a map from a <see cref="Controller.ControllerElement"/>
    /// to a <see cref="DeviceCapability"/>
    /// </summary>
    public interface IDeviceLayoutMapping : IEnumerable<ControllerElementMapping>
    {
        /// <summary>
        /// Gets the capability that maps on to the given element.
        /// 
        /// If none exists, then <see cref="DeviceCapability.None"/> is returned.
        /// </summary>
        /// <param name="e">The controller element.</param>
        /// <returns>The capability that maps on to the given element.</returns>
        DeviceCapability this[ControllerElement e] { get; }
    }
}
