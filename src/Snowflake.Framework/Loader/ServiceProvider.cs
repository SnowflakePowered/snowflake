using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Services;

namespace Snowflake.Loader
{
    internal class ServiceProvider : IServiceRepository
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

        /// <inheritdoc/>
        public T Get<T>()
            where T : class
        {
            if (services.Contains(typeof(T).FullName))
            {
                return this.coreService.Get<T>();
            }

            throw new InvalidOperationException(
                $"Service container is not authorized to provide service {typeof(T).FullName} or service does not exist.");
        }
    }
}
