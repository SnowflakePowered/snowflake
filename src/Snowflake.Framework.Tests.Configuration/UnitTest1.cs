using Snowflake.Configuration;
using Snowflake.Configuration.Generators;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Tests;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Services;
using System;
using Xunit;

namespace Snowflake.Framework.Tests.Configuration
{
    public class UnitTest1 
    {
        [Fact]
        public void Test1()
        {
           
            var x = new ConfigurationSection<MyConfiguration>();
            var y = new ConfigurationCollection<ExampleConfigurationCollection>();

            y.Configuration.Sections.MyBoolean = true;
            var b = (y.Configuration as IConfigurationCollectionGeneratedProxy).Values["Sections"].Descriptor;
            x.Configuration.MyBoolean = true;
            var mapcol = new ControllerElementMappingProfile("Keyboard",
                         "TEST_CONTROLLER",
                         InputDriver.Keyboard,
                         IDeviceEnumerator.VirtualVendorID,
                         new XInputDeviceInstance(0).DefaultLayout);

            var z = new InputTemplate<IRetroArchInput>(mapcol, 0);
            z[ControllerElement.ButtonA] = DeviceCapability.Button120;
            z.Template.InputDevice = 15;
        }
    }
}
