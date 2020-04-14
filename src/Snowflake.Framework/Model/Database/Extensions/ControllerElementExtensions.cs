using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database.Extensions
{
    internal static class ControllerElementExtensions
    {
        public static ControllerElementMappingsModel AsModel(this IControllerElementMappings @this, string profileName)
        {
            return new ControllerElementMappingsModel
            {
                ProfileName = profileName,
                DriverType = @this.DriverType,
                ControllerID = @this.ControllerID,
                DeviceName = @this.DeviceName,
                VendorID = @this.VendorID,
                MappedElements = @this.Select(e => e.AsModel(@this.ControllerID, @this.DeviceName, 
                profileName, @this.VendorID, @this.DriverType)).ToList()
            };
        }

        private static MappedControllerElementModel AsModel(this MappedControllerElement @this,
            ControllerId controllerId,
            string deviceName,
            string profileName,
            int vendorId,
            InputDriver driverType)
        {
            return new MappedControllerElementModel
            {
                LayoutElement = @this.LayoutElement,
                DeviceCapability = @this.DeviceCapability,
                ProfileName = profileName,
                ControllerID = controllerId,
                DeviceName = deviceName,
                VendorID = vendorId,
                DriverType = driverType
            };
        }

        public static IControllerElementMappings AsControllerElementMappings(this ControllerElementMappingsModel @this)
        {
            var mappings = new ControllerElementMappings(@this.DeviceName,
                @this.ControllerID,
                @this.DriverType,
                @this.VendorID);
            foreach (var mapping in @this.MappedElements)
            {
                mappings.Add(new MappedControllerElement(mapping.LayoutElement, mapping.DeviceCapability));
            }

            return mappings;
        }
    }
}
