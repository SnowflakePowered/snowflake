using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Xunit;
namespace Snowflake.Model.Tests
{
    public class IdVerificationTests
    {
        [Fact]
        public void ControllerIdVerification_Tests()
        {
            var device = (ControllerId)"SOME_DEVICE";
            var device2 = (ControllerId)"some_deViCe";
            Assert.Equal(device, device2);
            Assert.True(device == device2);
            Assert.True(device == "some_device");

            var layout = (ControllerId)"SOME_LAYOUT";
            var layout2 = (ControllerId)"Some_layout";
            Assert.Equal(layout, layout2);

            var controller = (ControllerId)"SOME_CONTROLLER";
            var controller2 = (ControllerId)"some_controller";
            Assert.Equal(controller, controller2);

            Assert.NotEqual(controller, device);
            Assert.True(controller != device2);
            Assert.True(controller != "not_ok");

            Assert.Throws<InvalidControllerIdException>(() => (ControllerId)"NOT_OK");
        }




        [Fact]
        public void PlatformIdVerification_Tests()
        {
            // Platform ids have no restrictions, we can simply reuse the controller ids.
            var device = (PlatformId)"SOME_DEVICE";
            var device2 = (PlatformId)"some_deViCe";
            Assert.Equal(device, device2);
            Assert.True(device == device2);
            Assert.True(device == "some_device");

            var layout = (PlatformId)"SOME_LAYOUT";
            var layout2 = (PlatformId)"Some_layout";
            Assert.Equal(layout, layout2);

            var controller = (PlatformId)"SOME_CONTROLLER";
            var controller2 = (PlatformId)"some_controller";
            Assert.Equal(controller, controller2);

            Assert.NotEqual(controller, device);
            Assert.True(controller != device2);
            Assert.True(controller != "device_2");


        }
    }
}
