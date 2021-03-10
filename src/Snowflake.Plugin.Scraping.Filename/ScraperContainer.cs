using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Loader;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;

namespace Snowflake.Plugin.Scraping.Filename
{
    public class ScraperContainer : IComposable
    {
        [ImportService(typeof(IPluginManager))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            serviceContainer.Get<IPluginManager>().Register<IScraper>(new FilenameScraper());
        }
    }
}
