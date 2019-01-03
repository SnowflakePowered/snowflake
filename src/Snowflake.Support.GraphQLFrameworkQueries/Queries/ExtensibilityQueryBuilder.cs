using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Execution.Extensibility;
using Snowflake.Extensibility;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Loader;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQL.Types.Module;
using Snowflake.Support.Remoting.GraphQL.Types.Plugin;

namespace Snowflake.Support.Remoting.GraphQL.Queries
{
    public class ExtensibilityQueryBuilder : QueryBuilder
    {
        private IModuleEnumerator ModuleEnumerator { get; }
        private IServiceEnumerator ServiceEnumerator { get; }
        private IPluginManager PluginManager { get; }

        public ExtensibilityQueryBuilder(IModuleEnumerator moduleEnumerator, IServiceEnumerator serviceEnumerator,
            IPluginManager pluginManager)
        {
            this.ServiceEnumerator = serviceEnumerator;
            this.ModuleEnumerator = moduleEnumerator;
            this.PluginManager = pluginManager;
        }

        [Connection("installedModules", "Get a list of modules installed in the module directory.",
            typeof(ModuleGraphType))]
        public IEnumerable<IModule> GetInstalledModules()
        {
            return this.ModuleEnumerator.Modules;
        }

        [Connection("loadedServices", "Get a list of services loaded.", typeof(StringGraphType))]
        public IEnumerable<string> GetLoadedServices()
        {
            return this.ServiceEnumerator.Services;
        }

        [Connection("loadedPlugins", "Gets a list of plugins loaded.", typeof(PluginGraphType))]
        public IEnumerable<IPlugin> GetLoadedPlugins()
        {
            return this.PluginManager;
        }

        [Connection("loadedScrapers", "Gets a list of scrapers loaded.", typeof(ScraperGraphType))]
        public IEnumerable<IScraper> GetLoadedScrapers()
        {
            return this.PluginManager.GetCollection<IScraper>();
        }

        [Connection("loadedCullers", "Gets a list of cullers loaded.", typeof(PluginGraphType))]
        public IEnumerable<IPlugin> GetLoadedCullers()
        {
            return this.PluginManager.GetCollection<ICuller>();
        }

        [Connection("loadedEmulators", "Gets a list of emulators loaded.", typeof(EmulatorGraphType))]
        public IEnumerable<IEmulator> GetLoadedEmulators()
        {
            return this.PluginManager.GetCollection<IEmulator>();
        }
    }
}
