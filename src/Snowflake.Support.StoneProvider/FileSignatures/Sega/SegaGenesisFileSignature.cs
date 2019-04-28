using System.IO;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Services;

namespace Snowflake.Stone.FileSignatures.Sega
{
    public sealed class SegaGenesisFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature { get; }

        private static readonly Sega32XFileSignature ThirtyTwoXSignature = new Sega32XFileSignature();
        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0x100, SeekOrigin.Begin);
            byte[] buffer = new byte[16];
            romStream.Read(buffer, 0, buffer.Length);
            // hack to ensure 32X games aren't considered genesis games.
            return (Encoding.UTF8.GetString(buffer).Contains("SEGA GENESIS") ||
                   Encoding.UTF8.GetString(buffer).Contains("SEGA MEGA DRIVE")) && 
                   !ThirtyTwoXSignature.HeaderSignatureMatches(romStream);
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0x183, SeekOrigin.Begin);
            byte[] buffer = new byte[7];
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
