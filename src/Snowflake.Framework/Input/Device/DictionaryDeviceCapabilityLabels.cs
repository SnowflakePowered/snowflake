using EnumsNET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Implements a label mapping using a backing dictionary. Missing labels are replaced with
    /// the empty string.
    /// </summary>
    public sealed class DictionaryDeviceCapabilityLabels : IDeviceCapabilityLabels
    {
        /// <summary>
        /// Create a new <see cref="IDeviceCapabilityLabels"/> using a backing dictionary.
        /// </summary>
        /// <param name="labels">The backing dictionary to use.</param>
        public DictionaryDeviceCapabilityLabels(IDictionary<DeviceCapability, string> labels)
        {
            this.Labels = labels;
        }

        private IDictionary<DeviceCapability, string> Labels { get; }
       
        public string this[DeviceCapability capability] => 
            this.Labels.TryGetValue(capability, out string? value) ? value : string.Empty;

        public IEnumerator<KeyValuePair<DeviceCapability, string>> GetEnumerator()
        {
            return this.Labels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
