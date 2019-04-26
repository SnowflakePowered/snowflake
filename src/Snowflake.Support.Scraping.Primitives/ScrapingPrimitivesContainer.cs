using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Loader;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;

namespace Snowflake.Support.Scraping.Primitives
{
    public class ScrapingPrimitivesContainer : IComposable
    {
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            serviceContainer.Get<IPluginManager>().Register<ICuller>(new ResultCuller());
            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<IGameMetadataTraverser>(new GameMetadataTraverser());
        }
    }
}
