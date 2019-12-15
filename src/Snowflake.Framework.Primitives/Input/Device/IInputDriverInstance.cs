using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Represents an instance of a device with regards to a specific input driver.
    /// </summary>
    public interface IInputDriverInstance
    {
        /// <summary>
        /// The input driver this instance implements
        /// </summary>
        InputDriverType Driver { get; }

        /// <summary>
        /// When enumerating devices with a given driver, the index of enumeration for this driver.
        /// 
        /// If <see cref="Driver"/> is <see cref="InputDriverType.Passthrough"/>, or
        /// <see cref="InputDriverType.Keyboard"/>, the index is always 0.
        /// </summary>
        int EnumerationIndex { get; }

        /// <summary>
        /// When enumerating devices with a given driver, the index of enumeration for this driver,
        /// with regards to the specific type of device, as determined by unique PID/VID combination,
        /// if and only if the driver disambiguates between different devices.
        /// 
        /// For example, if 2 DualShock 4 (054c:09cc) controllers were installed, with enumeration index of 
        /// 2 and 4 respectively, they would have a class enumeration index of 0 and 1.
        /// 
        /// However, if this was an XInput device, then this is the same as <see cref="EnumerationIndex"/>
        /// since XInput devices do not disambiguate between devices..
        /// </summary>
        int ClassEnumerationIndex { get; }

        /// <summary>
        /// When enumerating devices with a given driver, the index of enumeration for this driver,
        /// with regards to the specific type of device, as determined by unique ProductName combination,
        /// if and only if the driver disambiguates between different devices.
        /// 
        /// For example, if 2 DualShock 4 ("Wireless Controller") controllers were installed, 
        /// with enumeration index of 2 and 4 respectively, they would have a class enumeration
        /// index of 0 and 1.
        /// 
        /// However, if this was an XInput device, then this is the same as <see cref="EnumerationIndex"/>,
        /// since XInput devices do not disambiguate between devices.
        /// </summary>
        int UniqueNameEnumerationIndex { get; }

        /// <summary>
        /// The capabilities this device instance supports. 
        /// </summary>
        IEnumerable<DeviceCapability> Capabilities { get; }

        /// <summary>
        /// Without regards to a Stone controller layout, provides
        /// the natural mapping from controller element to 
        /// </summary>
        IDeviceLayoutMapping DefaultLayout { get; }
    }
}
