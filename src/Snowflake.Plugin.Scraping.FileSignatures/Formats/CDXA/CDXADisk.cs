using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Scraping.FileSignatures.Formats.CDXA
{
    public class CDXADisk
    {
        public const int BlockSize = 0x930;
        public const int BlockHeaderSize = 0x18;
        public static readonly byte[] FileRecordDelimeter = {0x8D, 0x55, 0x58, 0x41};
        private readonly Stream diskStream;
        public string VolumeDescriptor { get; }
        public IDictionary<string, CDXAFile> Files;

        public CDXADisk(Stream diskStream)
        {
            this.diskStream = diskStream;
            this.VolumeDescriptor = this.GetVolumeDescriptor();
            this.Files = this.GetRecords(string.Empty, 22)
                .ToDictionary(f => f.Path, f => new CDXAFile(this.diskStream, f.LBAStart, f.Path, f.Length));
        }

        private IList<CDXARecord> GetRecords(string parentDir, int lbaStart)
        {
            List<CDXARecord> records = new List<CDXARecord>();

            // http://wiki.osdev.org/ISO_9660#Volume_Descriptor_Set_Terminator
            foreach (byte[] entry in this.GetPathTableEntries(lbaStart))
            {
                using (Stream s = new MemoryStream(entry))
                using (BinaryReader reader = new BinaryReader(s))
                {
                    s.Seek(2, SeekOrigin.Begin);
                    int lba = reader.ReadInt32();
                    s.Seek(10, SeekOrigin.Begin);
                    long length = reader.ReadInt32();
                    s.Seek(25, SeekOrigin.Begin);
                    byte attr = reader.ReadByte(); // 3 - subdirectory, 2 - root, 1 - file
                    s.Seek(0x20, SeekOrigin.Begin);
                    int filenameLength = reader.ReadByte();
                    string fileName = Encoding.UTF8.GetString(reader.ReadBytes(filenameLength)).Split(';')[0];
                    if (attr == 1)
                    {
                        records.Add(new CDXARecord(lba, length, $@"{parentDir}{fileName}"));
                    }
                    else if (attr == 3 && fileName != "\0")
                    {
                        records.AddRange(this.GetRecords($@"{fileName}\", lba));
                    }
                }
            }

            return records;
        }

        private IEnumerable<byte[]> GetPathTableEntries(int lba)
        {
            using (var block = this.OpenBlock(lba))
            {
                byte[] buf = new byte[0x800];
                block.Read(buf, 0, 0x800);
                int pos = 0;
                while (pos < buf.Length && buf[pos] != 0)
                {
                    int recordLength = buf[pos];
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
        public Stream OpenBlock(int lba)
        {
            return new CDXABlockStream(lba, this.diskStream);
        }
    }
}
