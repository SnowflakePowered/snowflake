using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-SEGA_SAT")]
    public sealed class SegaSATFileSignature : FileSignature
    {
        public SegaSATFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = Encoding.UTF8.GetBytes("SEGA SEGASATURN "); //SEGA 32X

        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(Stream romStream)
        {

            romStream.Seek(0x10, SeekOrigin.Begin);
            byte[] buffer = new byte[16];
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);

        }
        public override string GetSerial(Stream romStream)
        {
            romStream.Seek(0x30, SeekOrigin.Begin);
            byte[] buffer = new byte[7];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }

        public override string GetInternalName(Stream romStream)
        {
            romStream.Seek(0x70, SeekOrigin.Begin);
            byte[] buffer = new byte[0x70];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }
    }
}

