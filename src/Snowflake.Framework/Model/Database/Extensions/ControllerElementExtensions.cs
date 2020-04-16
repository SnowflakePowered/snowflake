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
        public static ControllerElementMappingCollectionModel AsModel(this IControllerElementMappingProfile @this, string profileName)
        {
            return new ControllerElementMappingCollectionModel
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

        private static ControllerElementMappingModel AsModel(this ControllerElementMapping @this,
            ControllerId controllerId,
            string deviceName,
            string profileName,
            int vendorId,
            InputDriver driverType)
        {
            return new ControllerElementMappingModel
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

        public static IControllerElementMappingProfile AsControllerElementMappings(this ControllerElementMappingCollectionModel @this)
        {
            var mappings = new ControllerElementMappingProfile(@this.DeviceName,
                @this.ControllerID,
                @this.DriverType,
                @this.VendorID);
            foreach (var mapping in @this.MappedElements)
            {
                mappings.Add(new ControllerElementMapping(mapping.LayoutElement, mapping.DeviceCapability));
            }

            return mappings;
        }
    }
}
