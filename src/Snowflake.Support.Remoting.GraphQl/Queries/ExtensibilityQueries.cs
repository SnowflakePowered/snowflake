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

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class ExtensibilityQueries : QueryBuilder
    {
        private IModuleEnumerator ModuleEnumerator { get; }
        private IPluginManager PluginManager { get; }

        [Field("installedModules", "Get a list of modules installed in the module directory.", typeof(ModuleGraphType))]
        public IEnumerable<IModule> GetInstalledModules()
        {
            return this.ModuleEnumerator.Modules;
        }

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
