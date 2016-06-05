using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-NINTENDO_NDS")]
    public sealed class NintendoNDSFileSignature : FileSignature
    {
        public NintendoNDSFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = new byte[8] { 0xAC, 0x72, 0x21, 0xD4, 0xF8, 0x07, 0x56, 0xCF }; //last six bytes of nintendo logo + crc value 0x56 0xCF
        }


        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(Stream romStream)
        {

            byte[] buffer = new byte[8]; // read the 8 bytes
            romStream.Seek(0x156, SeekOrigin.Begin); //read from 342 to 350 (last bytes of nintendo logo and nintendo logo crc)
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);


        }


        public override string GetSerial(Stream romStream)
        {

            byte[] buffer = new byte[4]; // read the first 16 bytes
            romStream.Seek(0x0C, SeekOrigin.Begin); //seek 12 bytes after the game name
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer);
            return code;

        }

        public override string GetInternalName(Stream romStream)
        {

            byte[] buffer = new byte[12]; // read 12 bytes
            romStream.Read(buffer, 0, buffer.Length);
            string name = Encoding.UTF8.GetString(buffer).Trim('\0');
            return name;
        }
    }
}
