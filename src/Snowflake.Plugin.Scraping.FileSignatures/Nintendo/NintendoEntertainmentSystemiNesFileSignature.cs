using System.IO;
using System.Linq;
using Snowflake.Romfile;

namespace Snowflake.Plugin.Scraping.FileSignatures.Nintendo
{
    public sealed class NintendoEntertainmentSystemiNesFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => new byte[4] {0x4E, 0x45, 0x53, 0x1A}; // 'N' 'E' 'S' <EOF>

        /// <inheritdoc/>
        public string GetInternalName(Stream fileContents) => null;

        /// <inheritdoc/>
        public string GetSerial(Stream fileContents) => null;

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[4];
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);
        }
    }
}
