using System;
using Snowflake.Input;

namespace Snowflake.InputManager
{
    internal class InputDevice : IInputDevice
    {
        public string DI_ProductName { get; set; }
        public string DI_InstanceName { get; set; }
        public Guid DI_InstanceGUID { get; set; }
        public Guid DI_ProductGUID { get; set; }
        public string DI_InterfacePath { get; set; }
        public int? DI_ProductID { get; set; }
        public int? DI_VendorID { get; set; }
        public int? DI_JoystickID { get; set; }
        public bool XI_IsXInput { get; set; }
        public int? XI_GamepadIndex { get; set; }
        public int? DI_EnumerationNumber { get; set; }
        public string UDEV_Vendor { get { return null; } set { } }
        public string UDEV_Model { get { return null; } set { } }
        public string UDEV_MountPath { get { return null; } set { } }
        public DeviceType DI_DeviceType { get; set;}
    }
}
