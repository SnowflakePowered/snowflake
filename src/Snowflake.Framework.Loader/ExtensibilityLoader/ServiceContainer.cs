using Snowflake.Extensibility;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Loader.ExtensibilityLoader
{
    internal class ServiceContainer : IServiceContainer
    {
        private ICoreService coreService;
        private IList<string> services;
        internal ServiceContainer(ICoreService coreService, IEnumerable<string> serviceList)
        {
            this.coreService = coreService;
            this.services = serviceList.ToList();
        }

        public IEnumerable<string> Services => this.services.AsEnumerable();

        public string AppDataDirectory => throw new NotImplementedException();

        public T Get<T>()
        {
            if (services.Contains(typeof(T).FullName)) return this.coreService.Get<T>();
            throw new InvalidOperationException($"Service container is not authorized to provide service {typeof(T).FullName} or service does not exist.");
        }
    }
}
