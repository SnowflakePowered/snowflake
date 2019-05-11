using System;
using System.IO;

namespace Snowflake.Stone.FileSignatures.Formats.SFO
{
    internal class SFOValueTableEntry
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
            byte[] entryByteArray = new byte[sfoIndexTableEntry.SizeValueData];

            sfoFile.Read(entryByteArray, 0, sfoIndexTableEntry.SizeValueData);
            this.valueBytesReaded += sfoIndexTableEntry.SizeValueData;

            long offsetNextValue = sfoIndexTableEntry.OffsetDataValueInDataTable +
                                   sfoIndexTableEntry.SizeValueDataAndPadding; // korrekt!
            long skipBytes = offsetNextValue - this.valueBytesReaded;
            sfoFile.Seek(skipBytes, SeekOrigin.Current);
            this.valueBytesReaded += Convert.ToInt32(skipBytes);

            return SFOReaderUtilities.ByteArrayToString(entryByteArray, true);
        }
    }
}
