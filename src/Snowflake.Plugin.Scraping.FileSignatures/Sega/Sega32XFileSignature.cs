using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Romfile;
using Snowflake.Services;

namespace Snowflake.Plugin.Scraping.FileSignatures.Sega
{
    public sealed class Sega32XFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("SEGA 32X");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0x100, SeekOrigin.Begin);
            byte[] buffer = new byte[8];
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);
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
