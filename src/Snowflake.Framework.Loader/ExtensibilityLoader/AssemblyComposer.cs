using Snowflake.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Services;
using System.Collections.Concurrent;
using Snowflake.Utility;
using System.Reflection;

namespace Snowflake.Loader.ExtensibilityLoader
{
    public class AssemblyComposableComposer
    {
        private IList<IModule> pluginModules;
        private IList<IComposable> pluginComposables;
        private ICoreService coreService;

        public AssemblyComposableComposer(ICoreService coreService, IModuleEnumerator modules)
        {
            this.coreService = coreService;
            var assemblyLoader = new AssemblyModuleLoader();
            this.pluginModules = modules.Modules.Where(module => module.Loader == "assembly").ToList(); 
            this.pluginComposables = (from module in this.pluginModules
                                     from pluginContainer in assemblyLoader.LoadModule(module)
                                     select pluginContainer).ToList();
        }

        private IList<string> GetImportedServices(IComposable container)
        {
            var attributes = container.GetType().GetMethod(nameof(IComposable.Compose))
                .GetCustomAttributes<ImportServiceAttribute>();
            return attributes.Select(a => a.Service.FullName).ToList();
        }

        public void Compose()
        {
            var toCompose = this.pluginComposables.Select(p => (plugin: p, services: this.GetImportedServices(p))).ToList();
            int count = toCompose.Count();
            while (count > 0)
            {
                int prevCount = count;
                foreach (var uncomposed in toCompose.ToList())
                {
                    if (this.coreService.AvailableServices().ContainsAll(uncomposed.services))
                    {
                        Console.WriteLine($"Composing {uncomposed.plugin.GetType().Name} with services {String.Join(" ", uncomposed.services)}");
                        this.ComposeContainer(uncomposed.plugin, uncomposed.services);
                        toCompose.Remove(uncomposed);
                    }
                }
                count = toCompose.Count();
                if (prevCount == count) break; //no change
            }
        }

        private void ComposeContainer(IComposable pluginContainer, IList<string> services)
        {
            Console.WriteLine($"Composing {pluginContainer.GetType().Name}");
            IServiceContainer container = new ServiceContainer(this.coreService, services);
            pluginContainer.Compose(container);
            Console.WriteLine($"Finished composing {pluginContainer.GetType().Name}");
        }
    }
}
