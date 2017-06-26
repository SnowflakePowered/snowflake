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

        public void RegisterService<T>(T serviceObject) where T : class => this.coreService.RegisterService<T>(serviceObject);
    }
}
