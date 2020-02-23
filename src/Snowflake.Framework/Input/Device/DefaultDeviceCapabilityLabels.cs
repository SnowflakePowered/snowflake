using EnumsNET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Implements a default label mapping using the name of the capability enum element.
    /// </summary>
    public sealed class DefaultDeviceCapabilityLabels : IDeviceCapabilityLabels
    {
        /// <summary>
        /// Default labels using the name of the capability enum element.
        /// </summary>
        public static IDeviceCapabilityLabels DefaultLabels => _DefaultLabels;

        private static readonly DefaultDeviceCapabilityLabels _DefaultLabels = new DefaultDeviceCapabilityLabels();

        public string this[DeviceCapability capability] => Enums.AsString(capability);

        public IEnumerator<KeyValuePair<DeviceCapability, string>> GetEnumerator()
        {
            return Enums.GetMembers<DeviceCapability>()
                .Select(e => KeyValuePair.Create(e.Value, e.AsString())).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
