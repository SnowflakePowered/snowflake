using System;
using System.IO;

namespace Snowflake.Plugin.Scraping.FileSignatures.Formats.SFO
{
    class SFOValueTableEntry
    {
        private int valueBytesReaded;

        public SFOValueTableEntry()
        {
            this.valueBytesReaded = 0;
        }

        /**
         * Reads an entry of the dataValueTable an return it as String
         *
         * @param fIn
         * @param sfoIndexTableEntry
         * @return String
         * @throws IOException
         */
        public string ReadEntry(Stream sfoFile, SFOIndexTableEntry sfoIndexTableEntry)
        {
            byte[] entryByteArray = new byte[sfoIndexTableEntry.GetSizeValueData()];

            sfoFile.Read(entryByteArray, 0, sfoIndexTableEntry.GetSizeValueData());
            this.valueBytesReaded += sfoIndexTableEntry.GetSizeValueData();

            long offsetNextValue = sfoIndexTableEntry.GetOffsetDataValueInDataTable() +
                                   sfoIndexTableEntry.GetSizeValueDataAndPadding(); // korrekt!
            long skipBytes = offsetNextValue - this.valueBytesReaded;
            sfoFile.Seek(skipBytes, SeekOrigin.Current);
            this.valueBytesReaded += Convert.ToInt32(skipBytes);

            return SFOReaderUtilities.ByteArrayToString(entryByteArray, true);
        }
    }
}
