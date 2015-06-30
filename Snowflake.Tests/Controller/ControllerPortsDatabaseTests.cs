using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Snowflake.Platform;
using Moq;
using Xunit;

namespace Snowflake.Controller.Tests
{
    public class ControllerPortsDatabaseTests
    {
        [Fact]
        public void CreateDatabase_Test()
        {
            string filename = Path.GetTempFileName();
            IControllerPortsDatabase database = new ControllerPortsDatabase(filename);
            Assert.NotNull(database);
            this.DisposeSqlite();
            File.Delete(filename);
        }

        [Fact]
        public void AddPlatform_Test()
        {
            string filename = Path.GetTempFileName();
            IControllerPortsDatabase database = new ControllerPortsDatabase(filename);
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
            IControllerPortsDatabase database = new ControllerPortsDatabase(filename);
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
            IControllerPortsDatabase database = new ControllerPortsDatabase(filename);
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
