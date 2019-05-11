using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Stone.FileSignatures.Formats.ISO9660;

namespace Snowflake.Stone.FileSignatures.Formats.CDXA
{
    internal class CDXADisc : IDiscReader
    {
        public const int BlockSize = 0x930;
        public const int BlockHeaderSize = 0x18;
        public static readonly byte[] FileRecordDelimeter = {0x8D, 0x55, 0x58, 0x41};
        private readonly Stream diskStream;
        public string VolumeDescriptor { get; }
        public IReadOnlyDictionary<string, CDXAFile> Files { get; }

        public CDXADisc(Stream diskStream)
        {
            this.diskStream = diskStream;
            this.VolumeDescriptor = this.GetVolumeDescriptor();

            uint lba = BitConverter.ToUInt32(this.GetISOPVD().RootDirectoryEntryBytes, 2);
            this.Files = this.GetRecords(string.Empty, lba)
                .ToDictionary(f => f.Path, f => new CDXAFile(this.diskStream, f.LBAStart, f.Path, f.Length));
        }

        public ISOPrimaryVolumeDescriptor GetISOPVD()
        {
            return new ISOPrimaryVolumeDescriptor(this.OpenBlock(16));
        }

        public Stream OpenFile(string fileName)
        {
            if (!this.Files.ContainsKey(fileName)) return null;
            return this.Files[fileName].OpenFile();
        }

        private IList<CDXARecord> GetRecords(string parentDir, uint lbaStart)
        {
            List<CDXARecord> records = new List<CDXARecord>();

            // http://wiki.osdev.org/ISO_9660#Volume_Descriptor_Set_Terminator
            foreach (byte[] entry in this.GetDirectoryRecordEntries(lbaStart))
            {
                using Stream s = new MemoryStream(entry);
                using BinaryReader reader = new BinaryReader(s);
                s.Seek(2, SeekOrigin.Begin);
                uint lba = reader.ReadUInt32();
                s.Seek(10, SeekOrigin.Begin);
                long length = reader.ReadUInt32();
                s.Seek(25, SeekOrigin.Begin);
                byte attr = reader.ReadByte(); // 3 - subdirectory, 2 - root, 1 - file
                s.Seek(0x20, SeekOrigin.Begin);
                int filenameLength = reader.ReadByte();
                string fileName = Encoding.UTF8.GetString(reader.ReadBytes(filenameLength)).Split(';')[0];
                
                if (attr == 1 || attr == 0)
                {
                    records.Add(new CDXARecord(lba, length, $@"{parentDir}{fileName}"));
                }
                else if (attr == 3 && fileName != "\0")
                {
                    records.AddRange(this.GetRecords($@"{fileName}\", lba));
                }
            }

            return records;
        }

        private IEnumerable<byte[]> GetDirectoryRecordEntries(uint lba)
        {
            using (var block = this.OpenBlock(lba))
            {
                byte[] buf = new byte[0x800];
                block.Read(buf, 0, 0x800);
                int pos = 0;
                while (pos < buf.Length && buf[pos] != 0)
                {
                    int recordLength = buf[pos];
                    if (recordLength < 34) throw new InvalidDataException("Unable to resolve directory records.");
                    yield return buf.Skip(pos).Take(recordLength).ToArray();
                    pos += recordLength;
                }
            }
        }

        private string GetVolumeDescriptor()
        {
            using (var block = this.OpenBlock(16))
            {
                byte[] buf = new byte[0x10];
                block.Seek(0x28, SeekOrigin.Begin);
                block.Read(buf, 0, 0x10);
                return Encoding.UTF8.GetString(buf).Trim();
            }
        }

        /// <summary>
        /// Opens an LBA Block as a Stream.
        /// The returned Stream acts simply as a pointer to the LBA and does not protect
        /// against out of bounds reads. In order to read file data, use the much safer
        /// <see cref="CDXAFile.OpenFile"/>.
        /// </summary>
        /// <param name="lba">The LBA to open.</param>
        /// <returns>The LBA block as a Stream.</returns>
        public Stream OpenBlock(uint lba)
        {
            return new CDXABlockStream(lba, this.diskStream);
        }
    }
}
