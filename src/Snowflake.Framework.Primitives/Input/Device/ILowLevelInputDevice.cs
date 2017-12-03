using System;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// A platform agnostic representation of the low-level properties of an input device.
    /// Implementation is handled by a OS-dependent InputManager implementation.
    /// Currently this interface supports only the DirectInput/XInput APIs on Windows and udev on Linux. OSX support is not planned.
    /// Fields that are not used on an OS should be nulled and made unsettable.
    /// </summary>
    public interface ILowLevelInputDevice
    {
        /// <summary>
        /// Win32 - The ProductName of a DirectInput device
        /// </summary>
        string DI_ProductName { get; set; }

        /// <summary>
        /// Win32 - The InstanceName of a DirectInput device
        /// </summary>
        string DI_InstanceName { get; set; }

        /// <summary>
        /// Win32 - The InstanceGUID of a DirectInput device
        /// </summary>
        Guid DI_InstanceGUID { get; set; }

        /// <summary>
        /// Win32 - The ProductGUID of a DirectInput device
        /// </summary>
        Guid DI_ProductGUID { get; set; }

        /// <summary>
        /// Win32 - The InterfacePath of a DirectInput device
        /// </summary>
        string DI_InterfacePath { get; set; }

        /// <summary>
        /// Win32 - The JoystickID of a DirectInput device
        /// </summary>
        int? DI_JoystickID { get; set; }

        /// <summary>
        /// Win32 - The ProductID of a DirectInput device
        /// </summary>
        int? DI_ProductID { get; set; }

        /// <summary>
        /// Win32 - The VendorID of a DirectInput device
        /// </summary>
        int? DI_VendorID { get; set; }

        /// <summary>
        /// Win32 - Whether the device is an XInput device
        /// </summary>
        bool XI_IsXInput { get; set; }

        /// <summary>
        /// Win32 - An assumed XInput gamepad index for the device. May not be entirely accurate.
        /// </summary>
        int? XI_GamepadIndex { get; set; }

        /// <summary>
        /// udev - The vendor of the device
        /// </summary>
        string UDEV_Vendor { get; set; }

        /// <summary>
        /// udev - The device model
        /// </summary>
        string UDEV_Model { get; set; }

        /// <summary>
        /// udev - The mount point of the device
        /// </summary>
        string UDEV_MountPath { get; set; }

        /// <summary>
        /// The type of device
        /// </summary>
        DeviceType DI_DeviceType { get; set; }

        /// <summary>
        /// The order in which this DirectInput gamepad was enumerated.
        /// This is given in the order that the inputmanager provides and may not be reliable.
        /// </summary>
        int? DI_EnumerationNumber { get; set; }

        /// <summary>
        /// Whether or not the XInput Device is connected
        /// </summary>
        bool? XI_IsConnected { get; set; }

        /// <summary>
        /// The API used to discover this device
        /// </summary>
        InputApi DiscoveryApi { get; set; }
    }
}
