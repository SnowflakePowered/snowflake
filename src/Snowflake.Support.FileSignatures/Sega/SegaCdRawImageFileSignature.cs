using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Romfile.FileSignatures.Sega
{
    public sealed class SegaCdRawImageFileSignature : IFileSignature
    {
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("SEGADISCSYSTEM");

        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0x10, SeekOrigin.Begin);
            byte[] buffer = new byte[0xE];
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);

        }
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0x193, SeekOrigin.Begin);
            byte[] buffer = new byte[7];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();

        }

        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0x130, SeekOrigin.Begin);
            byte[] buffer = new byte[48];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }
    }
}

