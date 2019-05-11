using System.Collections.Generic;
using System.IO;

namespace Snowflake.Stone.FileSignatures.Formats.SFO
{
    internal class SFOReader
    {
        internal const int headerSize = 20;

        private SFOHeader sfoHeader;
        public IDictionary<string, string> KeyValues { get; }
        public IList<SFOIndexTableEntry> IndexTableEntries { get; }

        public SFOReader(Stream sfoFile)
        {
            this.KeyValues = new Dictionary<string, string>();
            this.IndexTableEntries = new List<SFOIndexTableEntry>();
            this.Parse(sfoFile);
        }

        private void Parse(Stream sfoFile)
        {
            // sfoHeader lesen
            this.sfoHeader = SFOHeader.Read(sfoFile);

            for (int i = 0; i < this.sfoHeader.NumberDataItems; i++)
            {
                this.IndexTableEntries.Add(SFOIndexTableEntry.ReadEntry(sfoFile));
            }

            // Zum KeyTable Anfang springen
            // (offset der KeyTabelle - Header-Lהnge - Anzahl * IndexEntry Lהnge = restl. zu ignorierende Bytes)
            int skipBytesToKeyTable = this.sfoHeader.OffsetKeyTable - SFOReader.headerSize -
                                      (this.sfoHeader.NumberDataItems * SFOIndexTableEntry.IndexTableEntryLength);
            sfoFile.Seek(skipBytesToKeyTable, SeekOrigin.Current);

            // read KeyTable
            var sfoKeyTableEntry = new SFOKeyTableEntry();
            var keyTableEntries = new List<string>();
            for (int i = 0; i < this.sfoHeader.NumberDataItems; i++)
            {
                keyTableEntries.Add(sfoKeyTableEntry.ReadEntry(sfoFile));
            }

            long skipBytesToValueTable = this.sfoHeader.OffsetValueTable - this.sfoHeader.OffsetKeyTable -
                                         sfoKeyTableEntry.KeyTableLength;
            sfoFile.Seek(skipBytesToValueTable, SeekOrigin.Current);

            // read ValueTable
            SFOValueTableEntry sfoValueTableEntry = new SFOValueTableEntry();
            var valueTableEntries = new List<string>();

            for (int i = 0; i < this.sfoHeader.NumberDataItems; i++)
            {
                valueTableEntries.Add(sfoValueTableEntry.ReadEntry(sfoFile, this.IndexTableEntries[i])
                    .Replace("\0", string.Empty));
            }

            for (int i = 0; i < keyTableEntries.Count; i++)
            {
                this.KeyValues.Add(keyTableEntries[i], valueTableEntries[i]);
            }
        }
    }
}
