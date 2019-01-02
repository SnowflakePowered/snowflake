using System;
using Snowflake.Input.Device;

namespace Snowflake.Plugin.InputManager.Win32
{
    internal class LowLevelInputDevice : ILowLevelInputDevice
    {
        /// <inheritdoc/>
        public string DI_ProductName { get; set; }

        /// <inheritdoc/>
        public string DI_InstanceName { get; set; }

        /// <inheritdoc/>
        public Guid DI_InstanceGUID { get; set; }

        /// <inheritdoc/>
        public Guid DI_ProductGUID { get; set; }

        /// <inheritdoc/>
        public string DI_InterfacePath { get; set; }

        /// <inheritdoc/>
        public int? DI_ProductID { get; set; }

        /// <inheritdoc/>
        public int? DI_VendorID { get; set; }

        /// <inheritdoc/>
        public int? DI_JoystickID { get; set; }

        /// <inheritdoc/>
        public bool XI_IsXInput { get; set; }

        /// <inheritdoc/>
        public int? XI_GamepadIndex { get; set; }

        /// <inheritdoc/>
        public int? DI_EnumerationNumber { get; set; }

        /// <inheritdoc/>
        public string UDEV_Vendor
        {
            get { return null; }
            set { }
        }

        /// <inheritdoc/>
        public string UDEV_Model
        {
            get { return null; }
            set { }
        }

        /// <inheritdoc/>
        public string UDEV_MountPath
        {
            get { return null; }
            set { }
        }

        /// <inheritdoc/>
        public bool? XI_IsConnected { get; set; }

        /// <inheritdoc/>
        public DeviceType DI_DeviceType { get; set; }

        /// <inheritdoc/>
        public InputApi DiscoveryApi { get; set; }
    }
}
