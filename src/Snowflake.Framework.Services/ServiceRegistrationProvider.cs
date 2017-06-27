using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    internal class ServiceRegistrationProvider : IServiceRegistrationProvider
    {
        private readonly IServiceContainer coreService;
        public ServiceRegistrationProvider(IServiceContainer coreService)
        {
            this.coreService = coreService;
        }

        public void RegisterService<T>(T serviceObject) where T : class
        {
            if (typeof(T).Assembly == serviceObject.GetType().Assembly)
                throw new InvalidOperationException("Can not register a service that implements an interface within the same assembly!");
            this.coreService.RegisterService(serviceObject);
        }
    }
}
