using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Romfile;

namespace Snowflake.Plugin.Scraping.FileSignatures.Sega
{
    public sealed class SegaCdRawImageFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("SEGADISCSYSTEM");
        private byte[] Sega32x = Encoding.UTF8.GetBytes("SEGA 32X");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0x10, SeekOrigin.Begin);
            byte[] buffer = new byte[0xE];
            romStream.Read(buffer, 0, buffer.Length);
            bool diskSystem = buffer.SequenceEqual(this.HeaderSignature);

            romStream.Seek(0x100, SeekOrigin.Begin);
            byte[] thiryTwoXbuffer = new byte[8];
            romStream.Read(buffer, 0, buffer.Length);
            bool thirtyTwoX = thiryTwoXbuffer.SequenceEqual(Sega32x);

            return diskSystem && !thirtyTwoX;
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0x193, SeekOrigin.Begin);
            byte[] buffer = new byte[7];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0x130, SeekOrigin.Begin);
            byte[] buffer = new byte[48];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }
    }
}
