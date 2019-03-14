using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Romfile;

namespace Snowflake.Stone.FileSignatures.Nintendo
{
    public sealed class GamecubeIso9660FileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => new byte[4] {0xC2, 0x33, 0x9F, 0x3D}; // gamecube magic word

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            byte[] buffer = new byte[4]; // read 4 bytes for magic word

            romStream.Seek(0x1C, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[5]; // game ids are 5 bytes long
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer);
            return code;
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            byte[] buffer = new byte[64];
            romStream.Seek(0x20, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }
    }
}
