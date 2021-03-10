using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Input.Controller;
using Snowflake.Model.Database.Models;

namespace Snowflake.Model.Database.Extensions
{
    internal static class ControllerElementExtensions
    {
        public static ControllerElementMappingCollectionModel AsModel(this IControllerElementMappingProfile @this, string profileName)
        {
            return new ControllerElementMappingCollectionModel
            {
                ProfileID = @this.ProfileGuid,
                ProfileName = profileName,
                DriverType = @this.DriverType,
                ControllerID = @this.ControllerID,
                DeviceName = @this.DeviceName,
                VendorID = @this.VendorID,
                MappedElements = @this.Select(e => e.AsModel(@this.ProfileGuid)).ToList()
            };
        }

        private static ControllerElementMappingModel AsModel(this ControllerElementMapping @this, Guid profileGuid)
        {
            return new ControllerElementMappingModel
            {
                LayoutElement = @this.LayoutElement,
                DeviceCapability = @this.DeviceCapability,
                ProfileID = profileGuid,
            };
        }

        public static IControllerElementMappingProfile AsControllerElementMappings(this ControllerElementMappingCollectionModel @this)
        {
            var mappings = new ControllerElementMappingProfile(@this.DeviceName,
                @this.ControllerID,
                @this.DriverType,
                @this.VendorID,
                @this.ProfileID);
            foreach (var mapping in @this.MappedElements)
            {
                mappings.Add(new ControllerElementMapping(mapping.LayoutElement, mapping.DeviceCapability));
            }

            return mappings;
        }
    }
}
