using System;
using Snowflake.Platform;
namespace Snowflake.Controller
{
    /// <summary>
    /// This database stores which type of controller is to be used on which port.
    /// Snowflake supports up to 8 ports. It is recommended that when compiling controller profiles, 
    /// the port number should indicate the slot of the controller profile to be used.
    /// Each port should correspond to a physical controller port on the console. 
    /// Thus, player 1 should use the profile in slot 1 of the controller in port 1
    /// <see cref="Snowflake.Controller.IControllerDatabase"/>
    /// </summary>
    public interface IControllerPortsDatabase
    {
        /// <summary>
        /// Adds a platform (console) to the database
        /// </summary>
        /// <param name="platformInfo">The platform to be added</param>
        void AddPlatform(IPlatformInfo platformInfo);
        /// <summary>
        /// Gets the device in a certain port of a platform (console)
        /// </summary>
        /// <param name="platformInfo">The platform in which the controller is 'plugged in'</param>
        /// <param name="portNumber">The port number where the controller is 'plugged in'</param>
        /// <returns></returns>
        string GetDeviceInPort(IPlatformInfo platformInfo, int portNumber);
        /// <summary>
        /// Sets the device to be used in a certain port of the platform (console)
        /// </summary>
        /// <param name="platformInfo">The platform in which the controller is to be 'plugged in'</param>
        /// <param name="portNumber">The port number where the controller is to be 'plugged in'</param>
        /// <param name="controllerId">The name of the device'</param>
        void SetDeviceInPort(IPlatformInfo platformInfo, int portNumber, string deviceName);
    }
}
