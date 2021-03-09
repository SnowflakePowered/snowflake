using Snowflake.Configuration.Input;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Tests
{
    public class TestInputMapping : IDeviceInputMapping
    {
        public TestInputMapping()
        {
        }

        public string this[DeviceCapability element] => element.ToString();

        public InputDriver Driver => InputDriver.None;
    }
}
