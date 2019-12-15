using System.IO;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats.SFO
{
    internal class SFOHeader
    {
        public string? FileType { get; set; }
        public string? SfoVersion { get; set; }
        public int OffsetKeyTable { get; set; }
        public int OffsetValueTable { get; set; }
        public int NumberDataItems { get; set; }

        public static SFOHeader Read(Stream fIn)
        {
            SFOHeader sfoHeader = new SFOHeader();

            byte[] tempByteArray = new byte[4];

            // read FileType
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.FileType = SFOReaderUtilities.ByteArrayToString(tempByteArray);

            // read sfoVerion
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.SfoVersion = SFOReaderUtilities.ByteArrayToString(tempByteArray);

            // read offsetKeyTable
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.OffsetKeyTable = SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray);

            // read offsetValueTable
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.OffsetValueTable = SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray);

            // read numberDataItem
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.NumberDataItems = SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray);

            return sfoHeader;
        }
    }
}
