using System.IO;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.Plugin.FileSignatures
{
    [Plugin("SnowflakeFileSignature-NINTENDO_N64")]
    public sealed class NintendoN64FileSignature : FileSignature
    {
        public NintendoN64FileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {

        }
        
        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.OpenRead(fileName))
                {
                    BinaryReader reader = new BinaryReader(new Int32SwapStream(romStream));
                    switch (reader.ReadUInt32())
                    {
                        case 0x80371240: //.z64
                        case 0x40123780: //.n64
                        case 0x37804012: //.v64
                            return true;
                        default:
                            return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public override string GetGameId(string fileName)
        {
            using (FileStream romStream = File.OpenRead(fileName))
            {
                BinaryReader reader = new BinaryReader(new Int32SwapStream(romStream));
                Stream swappedRomStream;
                switch (reader.ReadUInt32())
                {
                    case 0x80371240: //.z64
                        swappedRomStream = romStream;
                        break;
                    case 0x40123780: //.n64
                        swappedRomStream = new Int32SwapStream(romStream);
                        break;
                    case 0x37804012: //.v64
                        swappedRomStream = new Int16SwapStream(romStream);
                        break;
                    default: return "";
                }
                byte[] buffer = new byte[8];
                swappedRomStream.Seek(0x38, SeekOrigin.Begin);
                swappedRomStream.Read(buffer, 0, buffer.Length);
                string code = Encoding.UTF8.GetString(buffer).Trim('\0');
                return code;
            }
        }

        public override string GetInternalName(string fileName)
        {
            using (FileStream romStream = File.OpenRead(fileName))
            {
                BinaryReader reader = new BinaryReader(new Int32SwapStream(romStream));
                Stream swappedRomStream;
                switch (reader.ReadUInt32())
                {
                    case 0x80371240: //.z64
                        swappedRomStream = romStream;
                        break;
                    case 0x40123780: //.n64
                        swappedRomStream = new Int32SwapStream(romStream);
                        break;
                    case 0x37804012: //.v64
                        swappedRomStream = new Int16SwapStream(romStream);
                        break;
                    default: return "";
                }
                byte[] buffer = new byte[20];
                swappedRomStream.Seek(0x20, SeekOrigin.Begin);
                swappedRomStream.Read(buffer, 0, buffer.Length);
                string code = Encoding.UTF8.GetString(buffer).Trim('\0');
                return code;
            }
        }
    }
}
