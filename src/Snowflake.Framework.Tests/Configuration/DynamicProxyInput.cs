using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Services;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Configuration.Tests
{
    public class DynamicProxyInputTests
    {
        [Fact]
        public void Setter_Test()
        {
            var mapcol = new ControllerElementMappings("Keyboard",
                "TEST_CONTROLLER",
                InputDriver.Keyboard,
                IDeviceEnumerator.VirtualVendorID,
                new XInputDeviceInstance(0).DefaultLayout);
            var x = new InputTemplate<IRetroArchInput>(mapcol);
            x[ControllerElement.ButtonA] = DeviceCapability.Hat0S;
            Assert.Equal(DeviceCapability.Hat0S, x.Template.InputPlayerABtn);
            Assert.Equal(x.Template.Configuration.InputPlayerA, x.Template.InputPlayerA);
        }

        [Fact]
        public void NestedSetter_Test()
        {
            var mapcol = new ControllerElementMappings("Keyboard",
               "TEST_CONTROLLER",
               InputDriver.Keyboard,
               IDeviceEnumerator.VirtualVendorID,
               new XInputDeviceInstance(0).DefaultLayout);
            var x = new InputTemplate<IRetroArchInput>(mapcol);
            x[ControllerElement.ButtonA] = DeviceCapability.Hat0S;
            Assert.Equal(DeviceCapability.Hat0S, x.Template.Configuration.InputPlayerABtn);
        }

        [Fact]
        public void SuperNestedSetter_Test()
        {
            var mapcol = new ControllerElementMappings("Keyboard",
               "TEST_CONTROLLER",
               InputDriver.Keyboard,
               IDeviceEnumerator.VirtualVendorID,
               new XInputDeviceInstance(0).DefaultLayout);
            var x = new InputTemplate<IRetroArchInput>(mapcol);
            x[ControllerElement.ButtonA] = DeviceCapability.Hat0S;
            var y = (x as IConfigurationSection<IRetroArchInput>).Configuration["InputDevice"] = 1;
            var z = x.Template.Configuration.Values;
            var c = (x as IConfigurationSection<IRetroArchInput>).Values;
            Assert.Equal(DeviceCapability.Hat0S, x.Template.Template.Configuration.InputPlayerABtn);
            Assert.Equal(1, (x as IConfigurationSection<IRetroArchInput>).Configuration.InputDevice);
        }
    }
}
