using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Romfile.FileSignatures.Nintendo;
using Snowflake.Romfile.FileSignatures.Sony;
using Snowflake.Services;

namespace Snowflake.Romfile.FileSignatures.Composers
{
    public class FileSignaturesComposer : IComposable
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

        [ImportService(typeof(IFileSignatureMatcher))]
        public void Compose(IModule module, IServiceProvider coreInstance)
        {
            var fileSignatureEngine = coreInstance.Get<IFileSignatureMatcher>();
            this.RegisterFileSignatures(fileSignatureEngine);
        }
    }
}
