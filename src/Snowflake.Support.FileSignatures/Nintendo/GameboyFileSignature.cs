using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Romfile.FileSignatures.Nintendo
{
    public sealed class GameboyFileSignature : IFileSignature
    {
        public byte[] HeaderSignature => new byte[8] { 0xCE, 0xED, 0x66, 0x66, 0xCC, 0x0D, 0x00, 0x0B }; //first 8 bytes of nintendo logo

        public bool HeaderSignatureMatches(Stream romStream)
        {

            byte[] buffer = new byte[8]; // read the 8 bytes
            romStream.Seek(0x104, SeekOrigin.Begin); //seek to nntendo logo
            romStream.Read(buffer, 0, buffer.Length);
            romStream.Seek(0x143, SeekOrigin.Begin);
            return buffer.SequenceEqual(this.HeaderSignature);

        }

        public string GetInternalName(Stream romStream)
        {
            byte[] buffer = new byte[16]; //gb internal names are 16 bytes long
            romStream.Seek(0x134, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }

        public string GetSerial(Stream fileContents) => null;
    }
}
