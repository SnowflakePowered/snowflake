using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Controller
{
    /// <summary>
    /// JSON backed store of IControllerProfile
    /// </summary>
    public interface IControllerProfileStore
    {
        /// <summary>
        /// Gets a controller profile for a certain device name and controller ID
        /// </summary>
        /// <param name="deviceName">The device name to get profile for</param>
        /// <param name="controllerId">The controller ID for the ControllerDefinition that the profile corresponds to</param>
        /// <returns>The controller profile for the specified device and controller</returns>
        IControllerProfile GetControllerProfile(string deviceName, string controllerId);
        /// <summary>
        /// Sets a controller profile for a certain device name and controller ID
        /// </summary>
        /// <param name="deviceName">The device name to get profile for</param>
        /// <param name="controllerId">The controller ID for the ControllerDefinition that the profile corresponds to</param>
        /// <param name="controllerProfile">The new controller profile for the device</param>
        void SetControllerProfile(string deviceName, string controllerId, IControllerProfile controllerProfile);
    }
}
