using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Services.Logging;
using Snowflake.Tests.Composable;
using Xunit;

namespace Snowflake.Services.Tests
{
    public class ServiceProviderTests
    {
        [Fact]
        public void ServiceProvider_Test()
        {
            var coreService = new Mock<IServiceContainer>();
            IDictionary<Type, object> serviceContainer = new Dictionary<Type, object>();
            coreService.Setup(c => c.RegisterService(It.IsAny<IDummyComposable>())).Callback<IDummyComposable>(d => serviceContainer
                        .Add(typeof(IDummyComposable), d));
            coreService.Setup(c => c.AvailableServices()).Returns(() => serviceContainer.Keys.Select(service => service.FullName));
            coreService.Setup(c => c.Get<IDummyComposable>()).Returns(() => (IDummyComposable)serviceContainer[typeof(IDummyComposable)]);
            var dummyService = new DummyService();
            var provider = new ServiceRegistrationProvider(coreService.Object);
            provider.RegisterService<IDummyComposable>(dummyService);
            Assert.Contains(typeof(IDummyComposable), serviceContainer.Keys);
            Assert.Equal(dummyService, serviceContainer[typeof(IDummyComposable)]);

            var serviceProvider = new ServiceProvider(coreService.Object,
                new List<string>() { typeof(IDummyComposable).FullName });
            Assert.Equal(dummyService, serviceProvider.Get<IDummyComposable>());
        }

        [Fact]
        public void ServiceProviderNoImport_Test()
        {
            var coreService = new Mock<IServiceContainer>();
            IDictionary<Type, object> serviceContainer = new Dictionary<Type, object>();
            coreService.Setup(c => c.RegisterService(It.IsAny<IDummyComposable>())).Callback<IDummyComposable>(d => serviceContainer
                        .Add(typeof(IDummyComposable), d));
            coreService.Setup(c => c.AvailableServices()).Returns(() => serviceContainer.Keys.Select(service => service.FullName));
            coreService.Setup(c => c.Get<IDummyComposable>()).Returns(() => (IDummyComposable)serviceContainer[typeof(IDummyComposable)]);
            var dummyService = new DummyService();
            var provider = new ServiceRegistrationProvider(coreService.Object);
            provider.RegisterService<IDummyComposable>(dummyService);
            Assert.Contains(typeof(IDummyComposable), serviceContainer.Keys);
            Assert.Equal(dummyService, serviceContainer[typeof(IDummyComposable)]);

            var serviceProvider = new ServiceProvider(coreService.Object,
                new List<string>() { });
            Assert.Throws<InvalidOperationException>(() => serviceProvider.Get<IDummyComposable>());
        }
    }
}
