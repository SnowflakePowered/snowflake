using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Services;

namespace Snowflake.Plugin.Scraping.FileSignatures.Nintendo
{
    public sealed class NintendoDSFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => new byte[8] { 0xAC, 0x72, 0x21, 0xD4, 0xF8, 0x07, 0x56, 0xCF }; // 8 bytes of nintendo logo

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            byte[] buffer = new byte[8]; // read the 8 bytes
            romStream.Seek(0x156, SeekOrigin.Begin); // read from 342 to 350 (last bytes of nintendo logo and nintendo logo crc)
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            byte[] buffer = new byte[4]; // read the first 16 bytes
            romStream.Seek(0x0C, SeekOrigin.Begin); // seek 12 bytes after the game name
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer);
            return code;
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[12]; // read 12 bytes
            romStream.Read(buffer, 0, buffer.Length);
            string name = Encoding.UTF8.GetString(buffer).Trim('\0');
            return name;
        }
    }
}
