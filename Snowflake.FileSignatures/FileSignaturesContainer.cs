using Snowflake.Extensibility;
using Snowflake.Romfile.FileSignatures.Nintendo;
using Snowflake.Romfile.FileSignatures.Sony;
using Snowflake.Service;

namespace Snowflake.Romfile.FileSignatures
{
    public class FileSignaturesContainer : IPluginContainer
    {
        public void RegisterFileSignatures(IFileSignatureMatcher fileSignatureEngine)
        {
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-gb", new GameboyFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-gba", new GameboyAdvancedFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-gbc", new GameboyColorFileSignature());

            fileSignatureEngine.RegisterFileSignature("application/x-romfile-n64-littleendian",
                new Nintendo64LittleEndianFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-n64-indeterminate",
                new Nintendo64LittleEndianFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-n64-bigendian",
                new Nintendo64BigEndianFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-n64-indeterminate",
                new Nintendo64BigEndianFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-n64-byteswapped",
                new Nintendo64ByteswappedFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-n64-indeterminate",
                new Nintendo64ByteswappedFileSignature());

            fileSignatureEngine.RegisterFileSignature("application/x-romfile-nds", new NintendoDSFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-nes-ines", new NintendoEntertainmentSystemiNesFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-nes-unif", new NintendoEntertainmentSystemUnifFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-snes-headerless",
                new SuperNintendoHeaderlessFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-snes-magiccard",
                new SuperNintendoSmcHeaderFileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-wii-iso9660", new WiiIso9660FileSignature());
            fileSignatureEngine.RegisterFileSignature("application/x-romfile-wbfs", new WiiWbfsFileSignature());

            fileSignatureEngine.RegisterFileSignature("application/x-romfile-ps2-iso9660",
                new Playstation2Iso9660FileSignature());

            fileSignatureEngine.RegisterFileSignature("application/x-romfile-psx-rawimage",
                new PlaystationRawDiscFileSignature());

            fileSignatureEngine.RegisterFileSignature("application/x-romfile-psp-iso9660",
                new PlaystationPortableIso9660FileSignature());
        }

        public void Compose(ICoreService coreInstance)
        {
            var fileSignatureEngine = coreInstance.Get<IFileSignatureMatcher>();
            this.RegisterFileSignatures(fileSignatureEngine);
            /*pluginManager.Register<IFileSignature>(new NintendoN64FileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoGBFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoGBCFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoGBAFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoGCNISOFileSignature(coreInstance));

        
            pluginManager.Register<IFileSignature>(new NintendoNDSFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoNESFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoSNESFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new NintendoWiiFileSignature(coreInstance));

            pluginManager.Register<IFileSignature>(new Sega32XFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaCDFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaDCFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaGENFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaGGFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SegaSATFileSignature(coreInstance));

            pluginManager.Register<IFileSignature>(new SonyPS2ISOFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SonyPSPISOFileSignature(coreInstance));
            pluginManager.Register<IFileSignature>(new SonyPSXISOFileSignature(coreInstance));*/





        }
    }
}
