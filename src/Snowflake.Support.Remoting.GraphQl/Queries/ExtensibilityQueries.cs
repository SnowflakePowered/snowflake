using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Execution.Extensibility;
using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Types.Module;
using Snowflake.Support.Remoting.GraphQl.Types.Plugin;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class ExtensibilityQueryBuilder : QueryBuilder
    {
        private IModuleEnumerator ModuleEnumerator { get; }
        private IPluginManager PluginManager { get; }

        public ExtensibilityQueryBuilder(IModuleEnumerator enumerator, IPluginManager pluginManager)
        {
            this.ModuleEnumerator = enumerator;
            this.PluginManager = pluginManager;
        }

        [Connection("installedModules", "Get a list of modules installed in the module directory.", typeof(ModuleGraphType))]
        public IEnumerable<IModule> GetInstalledModules()
        {
            return this.ModuleEnumerator.Modules;
        }

        [Connection("loadedPlugins", "Gets a list of plugins loaded.", typeof(PluginGraphType))]
        public IEnumerable<IPlugin> GetLoadedPlugins()
        {
            return this.PluginManager;
        }

        public IEnumerable<IScraper> GetLoadedScrapers()
        {
            return this.PluginManager.GetCollection<IScraper>();
        }

        public IEnumerable<IEmulator> GetLoadedEmulators()
        {
            return this.PluginManager.GetCollection<IEmulator>();
        }
    }
}
