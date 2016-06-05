using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-NINTENDO_GBC")]
    public sealed class NintendoGBCFileSignature : FileSignature
    {
        public NintendoGBCFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = new byte[8] { 0xCE, 0xED, 0x66, 0x66, 0xCC, 0x0D, 0x00, 0x0B }; //first 8 bytes of nintendo logo

        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(Stream romStream)
        {

            byte[] buffer = new byte[8]; // read the 8 bytes

            romStream.Seek(0x104, SeekOrigin.Begin); //seek to nntendo logo
            romStream.Read(buffer, 0, buffer.Length);
            romStream.Seek(0x143, SeekOrigin.Begin);
            int cgb_magic = romStream.ReadByte();
            return buffer.SequenceEqual(this.HeaderSignature) && (cgb_magic == 0xC0 || cgb_magic == 0x80);

        }

        public override string GetInternalName(Stream romStream)
        {
            byte[] buffer = new byte[11]; // cgb internal names are only 11 bytes long
            romStream.Seek(0x134, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code; 
        }
    }
}
