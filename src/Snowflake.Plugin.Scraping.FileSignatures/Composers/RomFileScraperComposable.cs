using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Loader;
using Snowflake.Plugin.Scraping.FileSignatures.Composers;
using Snowflake.Plugin.Scraping.FileSignatures.Nintendo;
using Snowflake.Plugin.Scraping.FileSignatures.Scrapers;
using Snowflake.Plugin.Scraping.FileSignatures.Sega;
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
            var fileSignatureCollection = new FileSignatureCollection()
            {
                {"application/vnd.stone-romfile.sony.psx-discimage", new PlaystationRawDiscFileSignature()},
                {"application/vnd.stone-romfile.sony.ps2-discimage", new Playstation2Iso9660FileSignature()},
                {"application/vnd.stone-romfile.sony.psp-discimage", new PlaystationPortableIso9660FileSignature()},
                {"application/vnd.stone-romfile.nintendo.wii-wbfs", new WiiWbfsFileSignature()},
                {"application/vnd.stone-romfile.nintendo.wii-discimage", new WiiIso9660FileSignature()},
                {"application/vnd.stone-romfile.nintendo.snes", new SuperNintendoHeaderlessFileSignature()},
                {"application/vnd.stone-romfile.nintendo.snes-magiccard", new SuperNintendoSmcHeaderFileSignature()},
                {"application/vnd.stone-romfile.nintendo.nes-unif", new NintendoEntertainmentSystemUnifFileSignature()},
                {"application/vnd.stone-romfile.nintendo.nes-ines", new NintendoEntertainmentSystemiNesFileSignature()},
                {"application/vnd.stone-romfile.nintendo.nds", new NintendoDSFileSignature()},
                {"application/vnd.stone-romfile.nintendo.gbc", new GameboyColorFileSignature()},
                {"application/vnd.stone-romfile.nintendo.gb", new GameboyFileSignature()},
                {"application/vnd.stone-romfile.nintendo.gba", new GameboyAdvancedFileSignature()},
                {"application/vnd.stone-romfile.nintendo.n64-littleendian", new Nintendo64LittleEndianFileSignature()},
                {"application/vnd.stone-romfile.nintendo.n64-byteswapped", new Nintendo64ByteswappedFileSignature()},
                {"application/vnd.stone-romfile.nintendo.n64-bigendian", new Nintendo64BigEndianFileSignature()},
                {"application/vnd.stone-romfile.sega.32x", new Sega32XFileSignature()},
                {"application/vnd.stone-romfile.sega.scd-discimage", new SegaCdRawImageFileSignature()},
                {"application/vnd.stone-romfile.sega.gen", new SegaGenesisFileSignature()},
                {"application/vnd.stone-romfile.sega.gg", new SegaGameGearFileSignature()},
                {"application/vnd.stone-romfile.sega.sat-discimage", new SegaSaturnFileSignature()},
                {"application/vnd.stone-romfile.sega.32xcd-discimage", new Sega32XCdRawImageFileSignature()},
            };

            var pluginManager = serviceContainer.Get<IPluginManager>();
            var stoneProvider = serviceContainer.Get<IStoneProvider>();
            pluginManager.Register<IScraper>(new RomFileInfoScraper(fileSignatureCollection));
            pluginManager.Register<IScraper>(new StructuredFilenameTitleScraper(stoneProvider));
            pluginManager.Register<IScraper>(new FileMimetypeScraper(stoneProvider, fileSignatureCollection));
            pluginManager.Register<IScraper>(new PlatformInferralScraper(stoneProvider, fileSignatureCollection));
        }
    }
}
