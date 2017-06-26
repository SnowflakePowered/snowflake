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
using Snowflake.Loader;

namespace Snowflake.Service.Tests
{
    public class ServiceProviderTests
    {

        [Fact]
        public void ServiceProvider_Test()
        {
            var coreService = new Mock<IServiceContainer>();
            IDictionary<Type, object> serviceContainer = new Dictionary<Type, object>();
            coreService.Setup(c => c.RegisterService(It.IsAny<IDummyService>())).Callback<IDummyService>(d => serviceContainer
                        .Add(typeof(IDummyService), d));
            coreService.Setup(c => c.AvailableServices()).Returns(() => serviceContainer.Keys.Select(service => service.FullName));
            coreService.Setup(c => c.Get<IDummyService>()).Returns(() => (IDummyService)serviceContainer[typeof(IDummyService)]);
            var dummyService = new DummyService();
            var provider = new ServiceRegistrationProvider(coreService.Object);
            provider.RegisterService<IDummyService>(dummyService);
            Assert.Contains(typeof(IDummyService), serviceContainer.Keys);
            Assert.Equal(dummyService, serviceContainer[typeof(IDummyService)]);

            var serviceProvider = new ServiceProvider(coreService.Object, 
                new List<string>() { typeof(IDummyService).FullName });
            Assert.Equal(dummyService, serviceProvider.Get<IDummyService>());
        }

        [Fact]
        public void ServiceProviderNoImport_Test()
        {
            var coreService = new Mock<IServiceContainer>();
            IDictionary<Type, object> serviceContainer = new Dictionary<Type, object>();
            coreService.Setup(c => c.RegisterService(It.IsAny<IDummyService>())).Callback<IDummyService>(d => serviceContainer
                        .Add(typeof(IDummyService), d));
            coreService.Setup(c => c.AvailableServices()).Returns(() => serviceContainer.Keys.Select(service => service.FullName));
            coreService.Setup(c => c.Get<IDummyService>()).Returns(() => (IDummyService)serviceContainer[typeof(IDummyService)]);
            var dummyService = new DummyService();
            var provider = new ServiceRegistrationProvider(coreService.Object);
            provider.RegisterService<IDummyService>(dummyService);
            Assert.Contains(typeof(IDummyService), serviceContainer.Keys);
            Assert.Equal(dummyService, serviceContainer[typeof(IDummyService)]);

            var serviceProvider = new ServiceProvider(coreService.Object,
                new List<string>() { });
            Assert.Throws<InvalidOperationException>(() => serviceProvider.Get<IDummyService>());
        }
    }
}
