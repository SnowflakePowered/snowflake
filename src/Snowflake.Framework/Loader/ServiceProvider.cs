using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Services;

namespace Snowflake.Loader
{
    internal class ServiceProvider : IServiceRepository, IServiceProvider
    {
        private IServiceContainer coreService;
        private IList<string> services;

        internal ServiceProvider(IServiceContainer coreService, IEnumerable<string> serviceList)
        {
            this.coreService = coreService;
            this.services = serviceList.ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<string> Services => this.services.AsEnumerable();

        public IServiceProvider ToServiceProvider() => this;

        /// <inheritdoc/>
        public T Get<T>()
            where T : class
        {
            return (T)this.GetService(typeof(T));
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.FullName == null)
                throw new InvalidOperationException($"Attempted to get service of a generic type parameter," +
                    $" array type, pointer type, or byref type based on type parameter.");

            if (services.Contains(serviceType.FullName))
            {
                return this.coreService.Get(serviceType);
            }

            throw new InvalidOperationException(
                $"Service container is not authorized to provide service {serviceType.FullName} or service does not exist.");
        }
    }
}
