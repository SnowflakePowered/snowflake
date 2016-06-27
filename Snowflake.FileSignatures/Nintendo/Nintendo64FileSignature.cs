using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.FileSignatures;

namespace Snowflake.Romfile.FileSignatures.Nintendo
{
    public abstract class Nintendo64FileSignature<T> : IFileSignature where T : Stream
    {
        private readonly Func<Stream, T> streamConverter;
        private readonly uint formatByte;
        public byte[] HeaderSignature { get; }
        public string CanonicalMimetype { get; }

        protected Nintendo64FileSignature(uint formatByte, string mimetype, Func<Stream, T> streamConverter)
        {
            this.streamConverter = streamConverter;
            this.formatByte = formatByte;
            this.CanonicalMimetype = mimetype;
        }

        public bool HeaderSignatureMatches(Stream fileContents)
        {
            BinaryReader reader = new BinaryReader(new Int32SwapStream(fileContents)); //always read from a 32swapped
            return (reader.ReadUInt32() == this.formatByte);
        }

        public string GetSerial(Stream fileContents)
        {
            Stream swappedRomStream = this.streamConverter(fileContents); 
            byte[] buffer = new byte[8];
            swappedRomStream.Seek(0x38, SeekOrigin.Begin);
            swappedRomStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }

        public string GetInternalName(Stream fileContents)
        {
            Stream swappedRomStream = this.streamConverter(fileContents); //assume it is correct
            byte[] buffer = new byte[20];
            swappedRomStream.Seek(0x20, SeekOrigin.Begin);
            swappedRomStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }

    }
}
