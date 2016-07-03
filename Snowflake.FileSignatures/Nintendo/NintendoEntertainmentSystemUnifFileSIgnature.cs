using System.IO;
using System.Linq;

namespace Snowflake.Romfile.FileSignatures.Nintendo
{
    public sealed class NintendoEntertainmentSystemUnifFileSignature : IFileSignature
    {
        public byte[] HeaderSignature => new byte[4] { 0x55, 0x4E, 0x49, 0x46 }; //'U' 'N' 'I' 'F'



        public string GetInternalName(Stream fileContents) => null;

        public string GetSerial(Stream fileContents) => null;

        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[4];
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);
        }
    }
}


