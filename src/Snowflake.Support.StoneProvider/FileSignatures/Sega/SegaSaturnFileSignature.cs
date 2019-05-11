using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Services;

namespace Snowflake.Stone.FileSignatures.Sega
{
    internal sealed class SegaSaturnFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("SEGA SEGASATURN ");
        // Saturn can differ by at most 0x10 bytes lead-in
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
            romStream.Seek(this.GetHeaderIndex(romStream), SeekOrigin.Begin);
            byte[] buffer = new byte[16];
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0x20 + this.GetHeaderIndex(romStream), SeekOrigin.Begin);
            byte[] buffer = new byte[0xA];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0x60 + this.GetHeaderIndex(romStream), SeekOrigin.Begin);
            byte[] buffer = new byte[0x70];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }
    }
}
