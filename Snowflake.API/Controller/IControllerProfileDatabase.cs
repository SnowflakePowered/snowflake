using System;
using Snowflake.Platform;
using System.Collections.Generic;
namespace Snowflake.Controller
{
    /// <summary>
    /// A database that holds the controller profile data for a certain slot.
    /// The database supports 1 to 8 slots or 'controllerIndex', Snowflake can only store up to 8 profiles for a controller.
    /// Every controller profile ID has their own table with all the inputs as a column prefixes with 'INPUT_'.
    /// Profiles can be accessed once stored by the ControllerID of the controller definition the profile has mapped to, and the slot number.
    /// <see cref="Snowflake.Controller.IControllerProfile"/>
    /// <seealso cref="Snowflake.Controller.IControllerDefinition"/>
    /// </summary>
    public interface IControllerProfileDatabase
    {
        /// <summary>
        /// Add or update controller profile to the database. 
        /// </summary>
        /// <param name="controllerProfile">The controller profile</param>
        /// <param name="controllerIndex">The slot in which to store the profile</param>
        void AddControllerProfile(IControllerProfile controllerProfile, int controllerIndex);
        /// <summary>
        /// Gets the profile for a certain controller definition in a certain slot.
        /// </summary>
        /// <param name="controllerId">The controller ID for the ControllerDefinition that the profile corresponds to</param>
        /// <param name="controllerIndex">The slot in which the profile is stored</param>
        /// <returns></returns>
        IControllerProfile GetControllerProfile(string controllerId, int controllerIndex);
        /// <summary>
        /// Set the device name for a certain controller profile.
        /// Used for custom profiles to map to non XInput gamepads.
        /// todo: An alternate solution should be used on non-Windows environments.
        /// <see cref="Snowflake.Controller.ControllerProfileType.CUSTOM_PROFILE"/>
        /// </summary>
        /// <param name="controllerId">The controller ID of the controller profile</param>
        /// <param name="controllerIndex">The slot where the profile is stored</param>
        /// <param name="deviceName">The device name that the profile corresponds to</param>
        void SetDeviceName(string controllerId, int controllerIndex, string deviceName);
        /// <summary>
        /// Get the device name that corresponds to a profile.
        /// Used for custom profiles to map to non XInput gamepads.
        /// todo: An alternate solution should be used on non-Windows environments.
        /// <see cref="Snowflake.Controller.ControllerProfileType.CUSTOM_PROFILE"/>
        /// </summary>
        /// <param name="controllerId">The controller ID of the controller profile</param>
        /// <param name="controllerIndex">The slot where the profile is stored</param>
        /// <returns>Device name if one is set, null if controller is not type of CUSTOM_PROFILE</returns>
        string GetDeviceName(string controllerId, int controllerIndex);
        /// <summary>
        /// Add a platform's controllers to the database
        /// </summary>
        /// <param name="platform">The platform to add</param>
        void AddPlatform(IPlatformInfo platform);

    }
}
