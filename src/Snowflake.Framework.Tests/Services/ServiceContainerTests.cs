﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Loader;
using Snowflake.Tests.Composable;
using Xunit;

namespace Snowflake.Services.Tests
{
    public class ServiceContainerTests
    {
        [Fact]
        public void ServiceContainer_Init()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var container = new ServiceContainer(appDataDirectory.FullName, "https://localhost:9797");
        }

        [Fact]
        public void ServiceContainer_HasBasicServices()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var container = new ServiceContainer(appDataDirectory.FullName, "https://localhost:9797");
            Assert.NotNull(container.Get<IServiceRegistrationProvider>());
            Assert.NotNull(container.Get<IContentDirectoryProvider>());
            Assert.NotNull(container.Get<ILogProvider>());
            container.Get<ILogProvider>().GetLogger("Test").Info("Test");
            Assert.NotNull(container.Get<IModuleEnumerator>());
        }

        [Fact]
        public void ServiceContainer_RegisterTest()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var container = new ServiceContainer(appDataDirectory.FullName, "https://localhost:9797");
            IDummyComposable dummy = new DummyService();
            container.RegisterService(dummy);
            Assert.Equal(dummy, container.Get<IDummyComposable>());
            Assert.Contains(typeof(IDummyComposable).FullName, container.AvailableServices());
        }
    }
}
