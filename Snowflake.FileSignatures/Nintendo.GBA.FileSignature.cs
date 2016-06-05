using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-NINTENDO_GBA")]
    public sealed class NintendoGBAFileSignature : FileSignature
    {
        public NintendoGBAFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = new byte[8] { 0x24, 0xFF, 0xAE, 0x51, 0x69, 0x9A, 0xA2, 0x21 }; //first 8 bytes of nintendo logo

        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(Stream romStream)
        {

            byte[] buffer = new byte[8]; // read the 8 bytes
            romStream.Seek(4, SeekOrigin.Begin); //ignore first 4 bytes
            romStream.Read(buffer, 0, buffer.Length);

            if (buffer.SequenceEqual(this.HeaderSignature))
            {
                romStream.Seek(0xB2, SeekOrigin.Begin); //check for magic number value 0x96 at offset 0xB2
                int magicNumber = romStream.ReadByte();
                return magicNumber == 0x96;
            }
            return false;
        }


        public override string GetSerial(Stream romStream)
        {

            byte[] buffer = new byte[4]; // the code is 4 bytes long
            romStream.Seek(0xAC, SeekOrigin.Begin); //seek to game code
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer);
            return code;
        }

        public override string GetInternalName(Stream romStream)
        {

            byte[] buffer = new byte[12]; // read 12 bytes
            romStream.Seek(0xA0, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string name = Encoding.UTF8.GetString(buffer).Trim('\0');
            return name;
        }
    }
}
