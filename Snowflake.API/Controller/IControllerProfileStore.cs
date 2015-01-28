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
        /// Gets the list of available platforms in a store
        /// </summary>
        IList<string> AvailableProfiles { get; }
        /// <summary>
        /// Gets the ControllerID associated with this store
        /// </summary>
        string ControllerID { get; }
        /// <summary>
        /// Gets a controller profile for a certain device name and controller ID
        /// </summary>
        /// <param name="deviceName">The device name to get profile for</param>
        /// <param name="controllerId">The controller ID for the ControllerDefinition that the profile corresponds to</param>
        /// <returns>The controller profile for the specified device and controller</returns>
        IControllerProfile GetControllerProfile(string deviceName);
        /// <summary>
        /// Sets a controller profile for a certain device name and controller ID
        /// </summary>
        /// <param name="deviceName">The device name to get profile for</param>
        /// <param name="controllerId">The controller ID for the ControllerDefinition that the profile corresponds to</param>
        /// <param name="controllerProfile">The new controller profile for the device</param>
        void SetControllerProfile(string deviceName, IControllerProfile controllerProfile);
        /// <summary>
        /// Indexer shim for GetControllerProfile and SetControllerProfile
        /// </summary>
        /// <param name="deviceName">The device name to get or set profile for</param>
        /// <returns>The controller profile for the specified device and controller</returns>
        IControllerProfile this[string deviceName] { get; set; }
    }
}
