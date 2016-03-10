using System;
using System.IO;
using Moq;
using Snowflake.Platform;
using Xunit;

namespace Snowflake.Controller.Tests
{
    public class ControllerPortsStoreTests
    {
        [Fact]
        public void CreateDatabase_Test()
        {
            string filename = Path.GetTempFileName();
            IControllerPortStore database = new ControllerPortStore(filename);
            Assert.NotNull(database);
            this.DisposeSqlite();
            File.Delete(filename);
        }

        [Fact]
        public void AddPlatform_Test()
        {
            string filename = Path.GetTempFileName();
            IControllerPortStore database = new ControllerPortStore(filename);
            var platform = new Mock<IPlatformInfo>();
            platform.SetupGet(platformInfo => platformInfo.PlatformID).Returns("TEST_PLATFORM"); database.AddPlatform(platform.Object);
            string deviceName = database.GetDeviceInPort(platform.Object, 1);
            Assert.Null(deviceName);
            this.DisposeSqlite();
            File.Delete(filename);
        }

        [Fact]
        public void SetPort_Test()
        {
            string filename = Path.GetTempFileName();
            IControllerPortStore database = new ControllerPortStore(filename);
            var platform = new Mock<IPlatformInfo>();
            platform.SetupGet(platformInfo => platformInfo.PlatformID).Returns("TEST_PLATFORM");
            database.AddPlatform(platform.Object);
            database.SetDeviceInPort(platform.Object, 1, "TEST_DEVICE");
            this.DisposeSqlite();
            File.Delete(filename);
        }
        
        [Fact]
        public void GetPort_Test()
        {
            string filename = Path.GetTempFileName();
            IControllerPortStore database = new ControllerPortStore(filename);
            var platform = new Mock<IPlatformInfo>();
            platform.SetupGet(platformInfo => platformInfo.PlatformID).Returns("TEST_PLATFORM");
            database.AddPlatform(platform.Object);
            database.SetDeviceInPort(platform.Object, 1, "TEST_DEVICE");
            string deviceName =  database.GetDeviceInPort(platform.Object, 1);
            Assert.Equal("TEST_DEVICE", deviceName);
            this.DisposeSqlite();
            File.Delete(filename);
        }

        private void DisposeSqlite()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

    }
}
