using System.IO;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats.SFO
{
    internal sealed class SFOKeyTableEntry
    {
        public const byte delimiterByte = 0;

        /// <summary>
        /// The keyTable-length in bytes
        /// </summary>
        public int KeyTableLength { get; set; }

        public SFOKeyTableEntry()
        {
            this.KeyTableLength = 0;
        }

        /// <summary>
        /// Reads a key from the keyTable and return the value
        /// </summary>
        /// <param name="fIn"></param>
        /// <returns></returns>
        public string ReadEntry(Stream fIn)
        {
            byte[] tempByteArray1 = new byte[1];
            StringBuilder sb = new StringBuilder();

            fIn.Read(tempByteArray1, 0, 1);
            this.KeyTableLength++;
            while (tempByteArray1[0] != SFOKeyTableEntry.delimiterByte)
            {
                sb.Append((char) tempByteArray1[0]);
                fIn.Read(tempByteArray1, 0, 1);
                this.KeyTableLength++;
            }

            return sb.ToString();
        }
    }
}
