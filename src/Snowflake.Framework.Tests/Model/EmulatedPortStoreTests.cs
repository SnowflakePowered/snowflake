using Microsoft.EntityFrameworkCore;
using Moq;
using Snowflake.Input.Device;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Orchestration.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Snowflake.Model.Tests
{
    public class EmulatedPortStoreTests
    {
        [Fact]
        public void SetPort_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            IEmulatedPortStore store = new EmulatedPortStore(optionsBuilder);
            var orch = new Mock<IEmulatorOrchestrator>();
            orch.SetupGet(p => p.Name).Returns("Test");
            var device = new Mock<IInputDevice>();
            device.SetupGet(p => p.DeviceName).Returns("Test Controller");

            var instance = new Mock<IInputDeviceInstance>();
            instance.SetupGet(p => p.Driver).Returns(InputDriverType.Passthrough);
            instance.SetupGet(p => p.NameEnumerationIndex).Returns(0);

            store.SetPort(orch.Object, "TEST_PLATFORM", 0, "TEST_CONTROLLER", device.Object, instance.Object, "default");

            var ret = store.GetPort(orch.Object, "TEST_PLATFORM", 0);
            Assert.NotNull(ret);
            Assert.Equal("Test Controller", ret.DeviceName);
            Assert.Equal("TEST_CONTROLLER", ret.ControllerID);
            Assert.Equal(InputDriverType.Passthrough, ret.Driver);
        }

        [Fact]
        public void ClearPort_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            IEmulatedPortStore store = new EmulatedPortStore(optionsBuilder);
            var orch = new Mock<IEmulatorOrchestrator>();
            orch.SetupGet(p => p.Name).Returns("Test");
            var device = new Mock<IInputDevice>();
            device.SetupGet(p => p.DeviceName).Returns("Test Controller");

            var instance = new Mock<IInputDeviceInstance>();
            instance.SetupGet(p => p.Driver).Returns(InputDriverType.Passthrough);
            instance.SetupGet(p => p.NameEnumerationIndex).Returns(0);

            store.SetPort(orch.Object, "TEST_PLATFORM", 0, "TEST_CONTROLLER", device.Object, instance.Object, "default");

            var ret = store.GetPort(orch.Object, "TEST_PLATFORM", 0);
            Assert.NotNull(ret);
            Assert.Equal("Test Controller", ret.DeviceName);
            Assert.Equal("TEST_CONTROLLER", ret.ControllerID);
            Assert.Equal(InputDriverType.Passthrough, ret.Driver);

            store.ClearPort(orch.Object, "TEST_PLATFORM", 0);
            var ret2 = store.GetPort(orch.Object, "TEST_PLATFORM", 0);
            Assert.Null(ret2);
        }

        [Fact]
        public void EnumeratePort_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            IEmulatedPortStore store = new EmulatedPortStore(optionsBuilder);
            var orch = new Mock<IEmulatorOrchestrator>();
            orch.SetupGet(p => p.Name).Returns("Test");
            var device = new Mock<IInputDevice>();
            device.SetupGet(p => p.DeviceName).Returns("Test Controller");

            var instance = new Mock<IInputDeviceInstance>();
            instance.SetupGet(p => p.Driver).Returns(InputDriverType.Passthrough);
            instance.SetupGet(p => p.NameEnumerationIndex).Returns(0);

            store.SetPort(orch.Object, "TEST_PLATFORM", 0, "TEST_CONTROLLER", device.Object, instance.Object, "default");
            store.SetPort(orch.Object, "TEST_PLATFORM", 2, "TEST_CONTROLLER", device.Object, instance.Object, "default");

            var ret = store.EnumeratePorts(orch.Object, "TEST_PLATFORM");
            Assert.Equal(2, ret.Count());
        }
    }
}
