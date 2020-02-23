using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Snowflake.Input.Tests
{
    public class LabelsTests
    {
        [Fact]
        public void DefaultLabels_Test()
        {
            Assert.Equal("Button0", DefaultDeviceCapabilityLabels.DefaultLabels[DeviceCapability.Button0]);
            foreach(var (c, l) in DefaultDeviceCapabilityLabels.DefaultLabels)
            {
                Assert.Equal(l, c.ToString());
            }
        }

        [Fact]
        public void DictLabels_Test()
        {
            var dictLabels = new Dictionary<DeviceCapability, string>()
            {
                { DeviceCapability.Button0, "Button A" }
            };

            IDeviceCapabilityLabels labels = new DictionaryDeviceCapabilityLabels(dictLabels);
            Assert.Equal("Button A", labels[DeviceCapability.Button0]);
            Assert.Single(labels);
            Assert.Equal(string.Empty, labels[DeviceCapability.Axis0Negative]);
        }
    }
}
