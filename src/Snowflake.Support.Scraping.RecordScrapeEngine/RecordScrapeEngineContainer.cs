using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Loader;
using Snowflake.Model.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;

namespace Snowflake.Support.Scraping.RecordScrapeEngine
{
    public class RecordScrapeEngineContainer : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(ITraverser<IGameRecord>))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var traverser = serviceContainer.Get<ITraverser<IGameRecord>>();

            serviceContainer.Get<IServiceRegistrationProvider>()
                .RegisterService<IScrapeEngine<IGameRecord>>(new GameRecordScrapeEngine(traverser));
        }
    }
}
