using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    internal class InputDevice : IInputDevice
    {
        public int VendorID { get; }

        public int ProductID { get; }

        public string DeviceName { get; }

        public Guid InstanceGuid { get; }

        public string DevicePath { get; }

        public string FriendlyName { get; }
        public IEnumerable<IInputDriverInstance> Instances { get; }

        public InputDevice(int vendorId, int productId, 
            string productName, 
            string friendlyName,
            string devicePath,
            Guid instanceGuid, 
            IReadOnlyList<IInputDriverInstance> instances)
        {
            this.VendorID = vendorId;
            this.ProductID = productId;
            this.DeviceName = productName;
            this.DevicePath = devicePath;
            this.FriendlyName = friendlyName;
            this.InstanceGuid = instanceGuid;
            this.Instances = instances;
        }
    }
}
