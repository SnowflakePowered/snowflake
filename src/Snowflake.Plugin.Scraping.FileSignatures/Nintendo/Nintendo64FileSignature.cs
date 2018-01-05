using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin.Scraping.FileSignatures;
using Snowflake.Romfile;

namespace Snowflake.Plugin.Scraping.FileSignatures.Nintendo
{
    public abstract class Nintendo64FileSignature<T> : IFileSignature
        where T : Stream
    {
        private readonly Func<Stream, T> streamConverter;
        private readonly uint formatByte;

        /// <inheritdoc/>
        public byte[] HeaderSignature { get; }

        protected Nintendo64FileSignature(uint formatByte, Func<Stream, T> streamConverter)
        {
            this.streamConverter = streamConverter;
            this.formatByte = formatByte;
        }

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream fileContents)
        {
            fileContents.Seek(0, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(new Int32SwapStream(fileContents)); // always read from a 32swapped
            return reader.ReadUInt32() == this.formatByte;
        }

        /// <inheritdoc/>
        public string GetSerial(Stream fileContents)
        {
            Stream swappedRomStream = this.streamConverter(fileContents);
            byte[] buffer = new byte[8];
            swappedRomStream.Seek(0x38, SeekOrigin.Begin);
            swappedRomStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream fileContents)
        {
            Stream swappedRomStream = this.streamConverter(fileContents); // assume it is correct
            byte[] buffer = new byte[20];
            swappedRomStream.Seek(0x20, SeekOrigin.Begin);
            swappedRomStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }
    }
}
