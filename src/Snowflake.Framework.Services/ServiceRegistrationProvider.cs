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

        /// <inheritdoc/>
        public void RegisterService<T>(T serviceObject)
            where T : class
        {
            if (typeof(T).Assembly == serviceObject.GetType().Assembly)
            {
                throw new InvalidOperationException(
                    "Can not register a service who's implementation and interface reside in the same assembly!");
            }

            this.coreService.RegisterService(serviceObject);
        }
    }
}
