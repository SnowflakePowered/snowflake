using Snowflake.Extensibility;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Loader
{
    internal class ServiceContainer : IServiceProvider
    {
        private IServiceContainer coreService;
        private IList<string> services;
        internal ServiceContainer(IServiceContainer coreService, IEnumerable<string> serviceList)
        {
            this.coreService = coreService;
            this.services = serviceList.ToList();
        }

        public IEnumerable<string> Services => this.services.AsEnumerable();

        public T Get<T>() where T : class
        {
            if (services.Contains(typeof(T).FullName)) return this.coreService.Get<T>();
            throw new InvalidOperationException($"Service container is not authorized to provide service {typeof(T).FullName} or service does not exist.");
        }
    }
}
