using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    internal class ServiceEnumerator : IServiceEnumerator
    {
        private readonly IServiceContainer container;
        public ServiceEnumerator(ServiceContainer container)
        {
            this.container = container;
        }

        public IEnumerable<string> Services => this.container.AvailableServices();
    }
}
