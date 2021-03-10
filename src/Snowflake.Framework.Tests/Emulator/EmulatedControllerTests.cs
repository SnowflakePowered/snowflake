using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Xunit;

namespace Snowflake.Orchestration.Tests
{
    public class EmulatedControllerTests
    {
        [Fact]
        public void EmulatedControllerCreation_Test()
        {
            var physicalDevice = new Mock<IInputDevice>();
            var targetLayout = new Mock<IControllerLayout>();
            var layoutMapping = new Mock<IControllerElementMappingProfile>();
            var driverInstance = new Mock<IInputDeviceInstance>();
            IEmulatedController emulatedcontroller = new EmulatedController(0, physicalDevice.Object,
                driverInstance.Object,
                targetLayout.Object, layoutMapping.Object);

            Assert.Same(physicalDevice.Object, emulatedcontroller.PhysicalDevice);
            Assert.Same(targetLayout.Object, emulatedcontroller.TargetLayout);
            Assert.Same(layoutMapping.Object, emulatedcontroller.LayoutMapping);
            Assert.Equal(0, emulatedcontroller.PortIndex);
        }
    }
}
