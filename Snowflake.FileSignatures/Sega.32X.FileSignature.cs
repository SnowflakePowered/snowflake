using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-SEGA_32X")]
    public sealed class Sega32XFileSignature : FileSignature
    {
        public Sega32XFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = Encoding.UTF8.GetBytes("SEGA 32X"); //SEGA 32X
        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(Stream romStream)
        {

            romStream.Seek(0x100, SeekOrigin.Begin);
            byte[] buffer = new byte[8];
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);
        }
        public override string GetSerial(Stream romStream)
        {

            romStream.Seek(0x183, SeekOrigin.Begin);
            byte[] buffer = new byte[0xB]; //32X serials are 0xB long instead of 0x7
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }

        public override string GetInternalName(Stream romStream)
        {
            romStream.Seek(0x120, SeekOrigin.Begin);
            byte[] buffer = new byte[48];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }
    }
}

