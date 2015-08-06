namespace Snowflake.Emulator.Input.InputManager
{
    /// <summary>
    /// A platform agnostic representation of the properties of an input device.
    /// Implementation is handled by a OS-dependent InputManager library. 
    /// Currently this interface supports only the DirectInput/XInput APIs on Windows and udev on Linux. OSX support is not planned.
    /// Fields that are not used on an OS should be nulled and made unsettable. 
    /// </summary>
    public interface IInputDevice
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
        string DI_InstanceGUID { get; set; }
        /// <summary>
        /// Win32 - The ProductGUID of a DirectInput device
        /// </summary>
        string DI_ProductGUID { get; set; }
        /// <summary>
        /// Win32 - The ProductID of a DirectInput device
        /// </summary>
        int DI_ProductID { get; set; }
        /// <summary>
        /// Win32 - The InterfacePath of a DirectInput device
        /// </summary>
        string DI_InterfacePath { get; set; }
        /// <summary>
        /// Win32 - The JoystickID of a DirectInput device
        /// </summary>
        int DI_JoystickID { get; set; }
        /// <summary>
        /// Win32 - Whether the device is an XInput device
        /// </summary>
        bool XI_IsXInput { get; set; }
        /// <summary>
        /// Win32 - An assumed XInput gamepad index for the device. May not be entirely accurate.
        /// </summary>
        int XI_GamepadIndex { get; set; }
        /// <summary>
        /// xplat - The order in which this device was enumerated. 
        /// </summary>
        int DeviceIndex { get; set; }
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
    }
}
