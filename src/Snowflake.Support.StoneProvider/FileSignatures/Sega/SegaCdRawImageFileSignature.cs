using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Romfile;

namespace Snowflake.Stone.FileSignatures.Sega
{
    internal sealed class SegaCdRawImageFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("SEGADISCSYSTEM");

        private byte[] Sega32x = Encoding.UTF8.GetBytes("SEGA 32X");

        // SCD can differ by at most 0x10 bytes lead-in
        private int GetHeaderIndex(Stream romStream)
        {
            romStream.Seek(0x10, SeekOrigin.Begin);
            byte[] buffer = new byte[0x10];
            romStream.Read(buffer, 0, buffer.Length);
            bool diskSystem = buffer.SequenceEqual(this.HeaderSignature);
            return diskSystem ? 0x10 : 0x0;
        }

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0x0 + this.GetHeaderIndex(romStream), SeekOrigin.Begin);
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
            romStream.Seek(0x180 + this.GetHeaderIndex(romStream), SeekOrigin.Begin);
            byte[] buffer = new byte[0x0f];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0x120 + this.GetHeaderIndex(romStream), SeekOrigin.Begin);
            byte[] buffer = new byte[0x60];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }
    }
}
