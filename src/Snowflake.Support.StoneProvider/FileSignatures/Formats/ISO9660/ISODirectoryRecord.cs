using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/*
namespace Snowflake.Stone.FileSignatures.Formats.ISO9660
{
    public class ISODirectoryRecord
    {
        public ISODirectoryRecord(byte[] directoryRecordBytes)
        {
            this.Length = directoryRecordBytes[0];
            this.XALength = directoryRecordBytes[1];
            this.Extent = BitConverter.ToInt32(directoryRecordBytes, 2);
            this.DataSize = BitConverter.ToInt32(directoryRecordBytes, 10);
            this.Flags = directoryRecordBytes[25];
            this.FileUnitSize = directoryRecordBytes[26];
            this.InterleaveGap = directoryRecordBytes[27];
            this.VolumeSequenceNumber = BitConverter.ToInt16(directoryRecordBytes, 28);
            this.FileNameLength = directoryRecordBytes[32];
        }

        public ISODirectoryRecord(Stream directoryRecordStream)
        {
            using var reader = new BinaryReader(directoryRecordStream, Encoding.UTF8, true);
            this.Length = reader.ReadByte();
            this.XALength = reader.ReadByte();
            this.Extent = reader.ReadInt32();
            reader.ReadInt32(); // skip msb
            this.DataSize = reader.ReadInt32();
            reader.ReadInt32();
            reader.BaseStream.Seek(7, SeekOrigin.End);
            this.Flags = reader.ReadByte();
            this.FileUnitSize = reader.ReadByte();
            this.InterleaveGap = reader.ReadByte();
            this.VolumeSequenceNumber = reader.ReadInt16();
            reader.ReadInt16();
            this.FileNameLength = reader.ReadByte();
        }

        public byte Length { get; }
        public byte XALength { get; }
        public int Extent { get; }
        public int DataSize { get; }
        public byte Flags { get; }
        public string FileName { get; set; }
        public byte FileUnitSize { get; }
        public byte InterleaveGap { get; }
        public short VolumeSequenceNumber { get; }
        public byte FileNameLength { get; }
    }
}
*/