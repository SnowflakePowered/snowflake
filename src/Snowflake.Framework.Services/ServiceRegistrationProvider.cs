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

        public void RegisterService<T>(T serviceObject) => this.coreService.RegisterService<T>(serviceObject);
    }
}
