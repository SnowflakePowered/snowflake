using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Installation.Extensibility;
using Snowflake.Loader;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;

namespace Snowflake.Plugin.Installation.BasicInstallers
{
    public class BasicInstallerContainer : IComposable
    {
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IStoneProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var stone = serviceContainer.Get<IStoneProvider>();
            serviceContainer.Get<IPluginManager>().Register<IGameInstaller>(new SingleFileCopyInstaller(stone));
        }
    }
}
