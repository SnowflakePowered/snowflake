using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    internal class ServiceRegistrationProvider : IServiceRegistrationProvider
    {
        private readonly ICoreService coreService;
        public ServiceRegistrationProvider(ICoreService coreService)
        {
            this.coreService = coreService;
        }

        public void RegisterService<T>(T serviceObject) where T : class => this.coreService.RegisterService<T>(serviceObject);
    }
}
