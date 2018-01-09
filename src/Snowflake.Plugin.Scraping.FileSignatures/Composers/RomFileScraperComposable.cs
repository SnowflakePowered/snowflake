using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Loader;
using Snowflake.Plugin.Scraping.FileSignatures.Comoposers;
using Snowflake.Plugin.Scraping.FileSignatures.Scrapers;
using Snowflake.Plugin.Scraping.FileSignatures.Sony;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;

namespace Snowflake.Plugin.Scraping.FileSignatures.Composers
{
    public class RomFileScraperComposable : IComposable
    {
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IStoneProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var fileSignatureCollection = new FileSignatureCollection();
            fileSignatureCollection.Add("application/vnd.stone-romfile.sony.psx-discimage",
                new PlaystationRawDiscFileSignature());
            var pluginManager = serviceContainer.Get<IPluginManager>();
            var stoneProvider = serviceContainer.Get<IStoneProvider>();
            pluginManager.Register<IScraper>(new RomFileInfoScraper(fileSignatureCollection));
            pluginManager.Register<IScraper>(new StructuredFilenameTitleScraper(stoneProvider));
            pluginManager.Register<IScraper>(new FileMimetypeScraper(stoneProvider));
            pluginManager.Register<IScraper>(new PlatformInferralScraper(stoneProvider, fileSignatureCollection));
        }
    }
}
