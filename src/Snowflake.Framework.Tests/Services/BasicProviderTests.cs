using Snowflake.Extensibility;
using Snowflake.Services;
using Snowflake.Services.Logging;
using Snowflake.Services.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Moq;

namespace Snowflake.Services.Tests
{
    public class BasicProviderTests
    {
        [Fact]
        public void ContentDirectoryProvider_Test()
        {
            string tmp = Path.GetTempPath();
            IContentDirectoryProvider cdp = new ContentDirectoryProvider(tmp);
            Assert.Equal(tmp, cdp.ApplicationData.FullName);
        }

        [Fact]
        public void NlogLogger_Test()
        {
            ILogger logger = new NlogLogger("testLogger");
            logger.Debug("Test");
            logger.Error("Test");
            logger.Fatal("Test");
            logger.Info("Test");
            logger.Trace("Test");
            logger.Warn("Test");

            logger.Log("Test", LogLevel.Debug);
            logger.Log("Test", LogLevel.Error);
            logger.Log("Test", LogLevel.Fatal);
            logger.Log("Test", LogLevel.Info);
            logger.Log("Test", LogLevel.Warn);
            logger.Log("Test", LogLevel.Trace);

            logger.Error(new Exception("Test"), "Test");
        }

        [Fact]
        public void SqliteDatabaseProvider_Test()
        {
            DirectoryInfo dbRoot = new DirectoryInfo(Path.GetTempPath());
            string fileName = Guid.NewGuid().ToString();
            ISqliteDatabaseProvider dbProvider = new SqliteDatabaseProvider(dbRoot);
            dbProvider.CreateDatabase(fileName);
            Assert.True(File.Exists(Path.Combine(dbRoot.FullName, $"{fileName}.db")));
        }

        [Fact]
        public void SqliteDatabaseProviderUniverse_Test()
        {
            DirectoryInfo dbRoot = new DirectoryInfo(Path.GetTempPath());
            string fileName = Guid.NewGuid().ToString();
            ISqliteDatabaseProvider dbProvider = new SqliteDatabaseProvider(dbRoot);
            dbProvider.CreateDatabase("test", fileName);
            Assert.True(File.Exists(Path.Combine(dbRoot.CreateSubdirectory("test").FullName, $"{fileName}.db")));
        }

        [Fact]
        public void ServiceRegistration_Test()
        {
            var coreService = new Mock<IServiceContainer>();
            IDictionary<Type, object> serviceContainer = new Dictionary<Type, object>();
            coreService.Setup(c => c.RegisterService(It.IsAny<IDummyService>())).Callback<IDummyService>(d => serviceContainer
                        .Add(typeof(IDummyService), d));
            coreService.Setup(c => c.AvailableServices()).Returns(() => serviceContainer.Keys.Select(service => service.FullName));

            var dummyService = new DummyService();
            var provider = new ServiceRegistrationProvider(coreService.Object);
            provider.RegisterService<IDummyService>(dummyService);
            Assert.Contains(typeof(IDummyService), serviceContainer.Keys);
            Assert.Equal(dummyService, serviceContainer[typeof(IDummyService)]);
        }
    }
}
