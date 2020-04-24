using HotChocolate;
using HotChocolate.Types;
using Snowflake.Input.Device;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Input
{
    internal sealed class DeviceIdentifierInput
    {
        public Optional<Guid> InstanceID { get; set; }
        public Optional<int> VendorID { get; set; }
        public Optional<string> DeviceName { get; set; }
        public Optional<int> ProductID { get; set; }
    }
    
    internal static class DeviceIdentifierInputExtensions
    {
        internal static (int vendorId, string deviceName) RetrieveDeviceName(this DeviceIdentifierInput identifier, IEnumerable<IInputDevice> devices)
        {
            if (identifier.InstanceID.HasValue)
            {
                var instanceIdDevice = devices.FirstOrDefault(d => d.InstanceGuid == identifier.InstanceID.Value);
                if (instanceIdDevice == null) throw new ArgumentException("No input device was found with the specified instance GUID");
                return (instanceIdDevice.VendorID, instanceIdDevice.DeviceName);
            }

            if (!identifier.VendorID.HasValue) throw new ArgumentException("Neither Instance GUID nor Vendor ID were specified.");
            if (identifier.ProductID.HasValue)
            {
                var productIdDevice = devices.FirstOrDefault(d => d.VendorID == identifier.VendorID.Value && d.ProductID == identifier.ProductID.Value);
                if (productIdDevice == null) throw new ArgumentException("No input device was found with the specified combination of Vendor ID and Product ID");
                return (productIdDevice.VendorID, productIdDevice.DeviceName);
            }

            if (!identifier.DeviceName.HasValue) throw new ArgumentException("No valid combination of Vendor ID, Product ID, and Device Name was provided.");
            return (identifier.VendorID, identifier.DeviceName);
        }
    }

    internal sealed class DeviceIdentifierInputType
        : InputObjectType<DeviceIdentifierInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<DeviceIdentifierInput> descriptor)
        {
            descriptor.Name("DeviceIdentifierInput")
                .Description(
                @"Identifies an input device or input device archetype. One of the following combination of values must be set, in order of priority: 
* `instanceId`
  * Identifies a connected device with the specified instance GUID.
* `vendorId` + `productId`
  * Identifies a connected device with the specified Vendor ID and Product ID combination.
* `vendorId` + `deviceName`
  * Identifies the device archetype with the specified Vendor ID and Device Name combination. This does not need to refer to a connected device.

Input profiles are always registered to the device archetype consisting of the Vendor ID and Device Name of the device. 
`instanceId` and `vendorId` + `productId` inputs are used only to find such an archetype from the list of connected devices. 
Combinations are strictly prioritized, for exampleif `instanceId` is set, this will override any identfier with `vendorId`. 
For combinations that require a connected device, if such a device is not found, there is no fallback to a lower priority combination, and an error will occur. 
For example, if `instanceId`, `vendorId`, and `deviceName` are all set, and a device with the provided `instanceId` could not be found, this will result in an error.");

            descriptor.Field(i => i.InstanceID)
                .Name("instanceId")
                .Description("The instance ID that identifies the input device")
                .Type<UuidType>();
            descriptor.Field(i => i.VendorID)
                .Name("vendorId")
                .Description("The vendor ID that identifies the vendor of the input device.")
                .Type<IntType>();
            descriptor.Field(i => i.ProductID)
                .Name("productId")
                .Description("The product ID that identifies the product type of the input device.")
                .Type<IntType>();
            descriptor.Field(i => i.DeviceName)
                .Name("deviceName")
                .Description("The device name that together with the Vendor ID identifies a specific device archetype.")
                .Type<StringType>();
        }
    }
}
