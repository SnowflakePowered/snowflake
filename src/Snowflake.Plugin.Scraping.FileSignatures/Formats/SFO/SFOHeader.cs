using System.IO;
using System.Text;

namespace Snowflake.Plugin.Scraping.FileSignatures.Formats.SFO
{
    public class SFOHeader
    {
        private string fileType;
        private string sfoVersion;
        private int offsetKeyTable;
        private int offsetValueTable;
        private int numberDataItems;

        public static SFOHeader Read(Stream fIn)
        {
            SFOHeader sfoHeader = new SFOHeader();

            byte[] tempByteArray = new byte[4];

            // read FileType
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.SetFileType(SFOReaderUtilities.ByteArrayToString(tempByteArray));

            // read sfoVerion
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.SetSfoVersion(SFOReaderUtilities.ByteArrayToString(tempByteArray));

            // read offsetKeyTable
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.SetOffsetKeyTable(SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray));

            // read offsetValueTable
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.SetOffsetValueTable(SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray));

            // read numberDataItem
            fIn.Read(tempByteArray, 0, 4);
            sfoHeader.SetNumberDataItems(SFOReaderUtilities.ByteArrayReverseToInt(tempByteArray));

            return sfoHeader;
        }

        public string GetFileType()
        {
            return this.fileType;
        }

        public void SetFileType(string fileType)
        {
            this.fileType = fileType;
        }

        public string GetSfoVersion()
        {
            return this.sfoVersion;
        }

        public void SetSfoVersion(string sfoVersion)
        {
            this.sfoVersion = sfoVersion;
        }

        public int GetOffsetKeyTable()
        {
            return this.offsetKeyTable;
        }

        public void SetOffsetKeyTable(int offsetKeyTable)
        {
            this.offsetKeyTable = offsetKeyTable;
        }

        public int GetOffsetValueTable()
        {
            return this.offsetValueTable;
        }

        public void SetOffsetValueTable(int offsetValueTable)
        {
            this.offsetValueTable = offsetValueTable;
        }

        public int GetNumberDataItems()
        {
            return this.numberDataItems;
        }

        public void SetNumberDataItems(int numberDataItems)
        {
            this.numberDataItems = numberDataItems;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("== SFO Header Data ==\n")
            .Append("fileType:         ").Append(this.fileType).Append("\n")
            .Append("sfoVersion:       ").Append(this.sfoVersion).Append("\n")
            .Append("offsetKeyTable:   ").Append(this.offsetKeyTable).Append("\n")
            .Append("offsetValueTable: ").Append(this.offsetValueTable).Append("\n")
            .Append("numberDataItems:  ").Append(this.numberDataItems).Append("\n");

            return sb.ToString();
        }
    }
}
