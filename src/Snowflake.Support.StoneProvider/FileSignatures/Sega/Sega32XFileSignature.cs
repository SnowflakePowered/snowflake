using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Romfile;
using Snowflake.Services;

namespace Snowflake.Stone.FileSignatures.Sega
{
    public sealed class Sega32XFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => new byte[] {
            // http://devster.monkeeh.com/sega/32xguide1.txt
            0x46, 0xFC, 0x27, 0x00, 0x4B, 0xF9, 0x00, 0xA1,
            0x00, 0x00, 0x70, 0x01, 0x0C, 0xAD, 0x4D, 0x41,
            0x52, 0x53, 0x30, 0xEC
        };

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0x100, SeekOrigin.Begin);
            byte[] buffer = new byte[8];
            romStream.Read(buffer, 0, buffer.Length);

            byte[] secBuffer = new byte[20];
            romStream.Seek(0x400, SeekOrigin.Begin);
            romStream.Read(secBuffer, 0, secBuffer.Length);
            return Encoding.UTF8.GetString(buffer).StartsWith("SEGA") && secBuffer.SequenceEqual(this.HeaderSignature);
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0x183, SeekOrigin.Begin);
            byte[] buffer = new byte[0xB]; // 32X serials are 0xB long instead of 0x7
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0x120, SeekOrigin.Begin);
            byte[] buffer = new byte[48];
            romStream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
        }
    }
}
