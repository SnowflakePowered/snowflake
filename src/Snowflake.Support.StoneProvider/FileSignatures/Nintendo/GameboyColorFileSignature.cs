using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Romfile;

namespace Snowflake.Stone.FileSignatures.Nintendo
{
    public sealed class GameboyColorFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => new byte[8] {0xCE, 0xED, 0x66, 0x66, 0xCC, 0x0D, 0x00, 0x0B};

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            byte[] buffer = new byte[8]; // read the 8 bytes
            romStream.Seek(0x104, SeekOrigin.Begin); // seek to nntendo logo
            romStream.Read(buffer, 0, buffer.Length);
            romStream.Seek(0x143, SeekOrigin.Begin);
            int cgbMagic = romStream.ReadByte();
            return buffer.SequenceEqual(this.HeaderSignature) && (cgbMagic == 0xC0 || cgbMagic == 0x80);
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            byte[] buffer = new byte[11]; // cgb internal names are only 11 bytes long
            romStream.Seek(0x134, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream) => null;
    }
}
