using System.IO;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats.SFO
{
    internal sealed class SFOIndexTableEntry
    {
        /// <summary>
        /// The length of an entry in an SFO Index Table
        /// </summary>
        public static readonly int IndexTableEntryLength = 16;

        public byte[]? OffsetKeyNameInKeyTable { get; set; }
        public byte DataAlignmentRequirements { get; set; }
        public byte DataTypeValue { get; set; }
        public int SizeValueData { get; set; }
        public int SizeValueDataAndPadding { get; set; }
        public int OffsetDataValueInDataTable { get; set; }

        /// <summary>
        /// Reads one entry of the indexTable and return it's values in a SFOIndexTableEntry-object
        /// </summary>
        public static SFOIndexTableEntry ReadEntry(Stream fIn)
        {
            SFOIndexTableEntry sfoIndexTableEntry = new SFOIndexTableEntry();

            byte[] tempByteArray1 = new byte[1];
            byte[] tempByteArray2 = new byte[2];
            byte[] tempByteArray4 = new byte[4];

            // read offsetKeyNameInKeyTable
            fIn.Read(tempByteArray2, 0, 2);
            sfoIndexTableEntry.OffsetKeyNameInKeyTable = tempByteArray2;

            // read dataAlignmentRequirements
            fIn.Read(tempByteArray1, 0, 1);
            sfoIndexTableEntry.DataAlignmentRequirements = tempByteArray1[0];

            // read dataTypeValue
            fIn.Read(tempByteArray1, 0, 1);
            sfoIndexTableEntry.DataTypeValue = tempByteArray1[0];

            // read sizeValueData
            fIn.Read(tempByteArray4, 0, 4);
            sfoIndexTableEntry.SizeValueData = SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray4);

            // read sizeValueDataAndPadding
            fIn.Read(tempByteArray4, 0, 4);
            sfoIndexTableEntry.SizeValueDataAndPadding = SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray4);

            // read offsetDataValueInDataTable
            fIn.Read(tempByteArray4, 0, 4);
            sfoIndexTableEntry.OffsetDataValueInDataTable = SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray4);

            return sfoIndexTableEntry;
        }
    }
}
