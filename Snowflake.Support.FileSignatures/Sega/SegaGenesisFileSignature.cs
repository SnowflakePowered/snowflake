using System.IO;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Services;

namespace Snowflake.FileSignatures
{
    public sealed class SegaGenesisFileSignature : IFileSignature
    {
        public byte[] HeaderSignature { get; }

        public bool HeaderSignatureMatches(Stream romStream)
        {

            romStream.Seek(0x100, SeekOrigin.Begin);
            byte[] buffer = new byte[16];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Contains("SEGA GENESIS") || Encoding.UTF8.GetString(buffer).Contains("SEGA MEGA DRIVE");

        }
        public string GetSerial(Stream romStream)
        {

            romStream.Seek(0x183, SeekOrigin.Begin);
            byte[] buffer = new byte[7];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();

        }

        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0x120, SeekOrigin.Begin);
            byte[] buffer = new byte[48];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }
    }
}

