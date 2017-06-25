using Snowflake.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Services;
using System.Collections.Concurrent;
using Snowflake.Utility;
using System.Reflection;
using Snowflake.Loader;

namespace Snowflake.Services.AssemblyLoader
{
    public class AssemblyComposer
    {
        private readonly IList<IModule> modules;
        private readonly IList<(IModule Module, IComposable Composable)> moduleComposables;
        private readonly ICoreService coreService;
        private readonly ILogger logger;

        public AssemblyComposer(ICoreService coreService, IModuleEnumerator modules)
        {
            this.coreService = coreService;
            this.logger = new LogProvider().GetLogger("AssemblyComposer"); //Unknown if logging service is available.
            this.modules = modules.Modules.Where(module => module.Loader == "assembly").ToList();
            var assemblyLoader = new AssemblyModuleLoader();
            this.moduleComposables = (from module in this.modules
                                     from pluginContainer in this.LoadComposables(assemblyLoader, module)
                                     select (module, pluginContainer)).ToList();
        }

        private IEnumerable<IComposable> LoadComposables(AssemblyModuleLoader loader, IModule module)
        {
            try
            {
                var modules = loader.LoadModule(module).ToList();
                return modules;
            }
            catch (Exception ex)
            {
                this.logger.Error($"Could not load module {module.Entry} due to {ex.GetType().Name} : {ex.Message}");
                return Enumerable.Empty<IComposable>();
            }
        }

        private IList<string> GetImportedServices(IComposable container)
        {
            var attributes = container.GetType().GetMethod(nameof(IComposable.Compose))
                .GetCustomAttributes<ImportServiceAttribute>();
            return attributes.Select(a => a.Service.FullName).ToList();
        }

        public void Compose()
        {
            var toCompose = this.moduleComposables.Select(p => (module: p.Module, 
                composable: p.Composable, 
                services: this.GetImportedServices(p.Composable))).ToList();
            int count = toCompose.Count;
            while (count > 0)
            {
                int prevCount = count;
                foreach (var uncomposed in toCompose.ToList())
                {
                    if (this.coreService.AvailableServices().ContainsAll(uncomposed.services))
                    {
                        try
                        {
                            this.logger.Info($"Composing {uncomposed.composable.GetType().Name} with services {String.Join(" ", uncomposed.services)}");
                            this.ComposeContainer(uncomposed.module, uncomposed.composable, uncomposed.services);
                            this.logger.Info($"Finished composing {uncomposed.composable.GetType().Name}");
                        }
                        catch (Exception ex)
                        {
                            this.logger.Error($"Exception {ex.GetType()}: {ex.Message} occured when composing {uncomposed.composable.GetType().Name}.");
                            this.logger.Error($"Stack Trace:{Environment.NewLine + ex.StackTrace}");
                        }
                        finally
                        {
                            toCompose.Remove(uncomposed);
                        }
                    }
                }
                count = toCompose.Count;
                if (prevCount == count) break; //no change
            }
        }

        private void ComposeContainer(IModule module, IComposable moduleComposable, IList<string> services)
        {
            IServiceContainer container = new ServiceContainer(this.coreService, services);
            moduleComposable.Compose(module, container);
        }
    }
}
