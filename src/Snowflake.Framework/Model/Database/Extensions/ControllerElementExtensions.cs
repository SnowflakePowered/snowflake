using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Input.Controller.Mapped;
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
                ControllerID = @this.ControllerId,
                DeviceID = @this.DeviceId,
                MappedElements = @this.Select(e => e.AsModel(@this.ControllerId, @this.DeviceId, profileName)).ToList()
            };
        }

        private static MappedControllerElementModel AsModel(this IMappedControllerElement @this, 
            string controllerId,
            string deviceId, 
            string profileName)
        {
            return new MappedControllerElementModel
            {
                LayoutElement = @this.LayoutElement,
                DeviceElement = @this.DeviceElement,
                ProfileName = profileName,
                ControllerID = controllerId,
                DeviceID = deviceId
            };
        }

        public static IControllerElementMappings AsControllerElementMappings(this ControllerElementMappingsModel @this)
        {
            var mappings = new ControllerElementMappings(@this.DeviceID, @this.ControllerID);
            foreach (var mapping in @this.MappedElements)
            {
                mappings.Add(new MappedControllerElement(mapping.LayoutElement, mapping.DeviceElement));
            }
            return mappings;
        }
    }
}
