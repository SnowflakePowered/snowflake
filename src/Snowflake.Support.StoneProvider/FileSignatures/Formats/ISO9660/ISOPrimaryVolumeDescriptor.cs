using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/*
namespace Snowflake.Stone.FileSignatures.Formats.ISO9660
{
    public class ISOPrimaryVolumeDescriptor
    {
        public ISOPrimaryVolumeDescriptor(Stream pvdBlock)
        {
            using var reader = new BinaryReader(pvdBlock, Encoding.UTF8, true);
            this.TypeCode = reader.ReadByte();
            this.StandardIdentifier = reader.ReadString(5);
            this.Version = reader.ReadByte();
            reader.BaseStream.Seek(1, SeekOrigin.Current);
            this.SystemIdentifier = reader.ReadString(32);
            this.VolumeIdentifier = reader.ReadString(32);
            reader.BaseStream.Seek(8, SeekOrigin.Current);
            this.VolumeSpaceSize = reader.ReadInt32();
            reader.ReadInt32(); // skip msb
            reader.BaseStream.Seek(32, SeekOrigin.Current);
            this.VolumeSetSize = reader.ReadInt16();
            reader.ReadInt16(); // skip msb
            this.VolumeSequenceNumber = reader.ReadInt16();
            reader.ReadInt16(); // skip msb
            this.LogicalBlockSize = reader.ReadInt16();
            reader.ReadInt16(); // skip msb
            this.PathTableSize = reader.ReadInt32();
            reader.ReadInt32(); // skip msb
            this.LPathTableLocation = reader.ReadInt32();
            this.OptionalLPathTableLocation = reader.ReadInt32();
            this.MPathTableLocation = reader.ReadInt32();
            this.OptionalMPathTableLocation = reader.ReadInt32();
            this.RootDirectoryEntryBytes = reader.ReadBytes(34);
            this.VolumeSetIdentifier = reader.ReadString(128);
            this.PublisherIdentifier = reader.ReadString(128);
            this.DataPreparerIdentifier = reader.ReadString(128);
            this.ApplicationIdentifier = reader.ReadString(128);
            this.CopyrightFileIdentifier = reader.ReadString(38);
            this.AbstractFileIdentifier = reader.ReadString(36);
            this.BibliographicFileIdentifier = reader.ReadString(37);
            reader.BaseStream.Seek(17 * 4, SeekOrigin.Current); // skip datetime information
            this.FileStructureVersion = reader.ReadByte();
    
        }

        public byte TypeCode { get; }
        public string StandardIdentifier { get; }
        public byte Version { get; }
        public string SystemIdentifier { get; }
        public string VolumeIdentifier { get; }
        public int VolumeSpaceSize { get; }
        public short VolumeSetSize { get; }
        public short VolumeSequenceNumber { get; }
        public short LogicalBlockSize { get; }
        public int PathTableSize { get; }
        public int LPathTableLocation { get; }
        public int OptionalLPathTableLocation { get; }
        public int MPathTableLocation { get; }
        public int OptionalMPathTableLocation { get; }
        public byte[] RootDirectoryEntryBytes { get; }
        public string VolumeSetIdentifier { get; }
        public string PublisherIdentifier { get; }
        public string DataPreparerIdentifier { get; }
        public string ApplicationIdentifier { get; }
        public string CopyrightFileIdentifier { get; }
        public string AbstractFileIdentifier { get; }
        public string BibliographicFileIdentifier { get; }
        public byte FileStructureVersion { get; }
    }
}
*/