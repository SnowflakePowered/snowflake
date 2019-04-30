﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
/// Implementation based off
/// https://github.com/inolen/redream
/// 
/// Licensed under GPLv3
/// 
/// This is very slightly off for some reason, but it seems to work well enough for 
/// just getting lba block 1 so...
/// 
/// May fix in a later version.
/// </summary>
namespace Snowflake.Stone.FileSignatures.Formats.CDI
{
    public class DiscJugglerDisc : IDiscReader
    {
        private const int GDROM_PREGAP = 150;
        private static readonly byte[] TrackStartMarker = new byte[] { 0, 0, 1, 0, 0, 0, 255, 255, 255, 255 };

        public uint DiscJugglerRawVersion { get; }
        public DiscJugglerVersions DiscJugglerVersion { get; }
        public string VolumeDescriptor { get; }
        private uint HeaderOffset { get; }
        private Stream ImageStream { get; }

        private List<Session> _sessions { get; }
        private List<Track> _tracks { get; }

        internal IEnumerable<Track> Tracks => _tracks.AsEnumerable();

        public IReadOnlyList<Session> Sessions => _sessions.AsReadOnly();
        public DiscJugglerDisc(Stream imageStream)
        {
            this.ImageStream = imageStream;
             new List<Session>();
            (this.DiscJugglerRawVersion, this.HeaderOffset, this.DiscJugglerVersion) = this.ParseHeader();
            this._tracks = new List<Track>();
            this._sessions = this.ParseSessions().ToList();
            this.VolumeDescriptor = this.GetVolumeDescriptor();
        }

        #region Parse 
        private IEnumerable<Session> ParseSessions()
        {
            using var reader = new BinaryReader(this.ImageStream, Encoding.UTF8, true);
            
            if (this.DiscJugglerVersion == DiscJugglerVersions.Version35)
            {
                this.ImageStream.Seek(-this.HeaderOffset, SeekOrigin.End);
            }
            else
            {
                this.ImageStream.Seek(this.HeaderOffset, SeekOrigin.Begin);
            }

            ushort numSessions = reader.ReadUInt16();
            long trackOffset = 0;
            for (int i = 0; i < numSessions; i++)
            {
                var session = this.ParseSingleSession(reader, ref trackOffset);
                yield return session;
                /* seek to the next session */
                int offset = 4 + 8;
                if (this.DiscJugglerVersion != DiscJugglerVersions.Version2)
                {
                    offset += 1;
                }
                this.ImageStream.Seek(offset, SeekOrigin.Current);
            }
        }

        private Session ParseSingleSession(BinaryReader reader, ref long trackOffset)
        {
            ushort numTracks = reader.ReadUInt16();

            int firstTrack = this._tracks.Count;

            if (numTracks == 0) throw new DiscJugglerParseException("Session contains no tracks!");

            for (int i = 0; i < numTracks; i++)
            {
                var track = this.ParseTrack(reader, ref trackOffset);

                this._tracks.Add(track);

                /* seek to the next track */

                /* extra data (DJ 3.00.780 and up) */
                this.ImageStream.Seek(29, SeekOrigin.Current);
                if (this.DiscJugglerVersion != DiscJugglerVersions.Version2)
                {
                    this.ImageStream.Seek(5, SeekOrigin.Current);
                    uint tmp = reader.ReadUInt32();
                    if (tmp == 0xffffffff)
                    {
                        this.ImageStream.Seek(78, SeekOrigin.Current);
                    }
                }
            }

            int lastTrack = this._tracks.Count - 1;

            return new Session(this, this._tracks[firstTrack].FrameAddr,
                this._tracks.Last().FrameAddr + this._tracks.Last().TrackLength, 
                firstTrack, lastTrack);
           
        }

        private Track ParseTrack(BinaryReader reader, ref long trackOffset)
        {
            uint tmp = reader.ReadUInt32();
            if (tmp != 0)
            {
                this.ImageStream.Seek(8, SeekOrigin.Current);
            }
            byte[] startMark = reader.ReadBytes(10);
            if (!startMark.SequenceEqual(DiscJugglerDisc.TrackStartMarker))
            {
                throw new DiscJugglerParseException("First expected track marker was not found!");
            }
            startMark = reader.ReadBytes(10);
            if (!startMark.SequenceEqual(DiscJugglerDisc.TrackStartMarker))
            {
                throw new DiscJugglerParseException("Second expected track marker was not found!");
            }

            this.ImageStream.Seek(4, SeekOrigin.Current);
            byte fileNameLength = reader.ReadByte();
            string fileName = Encoding.UTF8.GetString(reader.ReadBytes(fileNameLength));
            this.ImageStream.Seek(11 + 4 + 4, SeekOrigin.Current);
            tmp = reader.ReadUInt32();
            if (tmp == 0x80000000)
            {
                this.ImageStream.Seek(8, SeekOrigin.Current);
            }
            this.ImageStream.Seek(2, SeekOrigin.Current);

            uint pregapLength = reader.ReadUInt32();
            uint trackLength = reader.ReadUInt32();
            this.ImageStream.Seek(6, SeekOrigin.Current);
            uint sectorMode = reader.ReadUInt32();
            this.ImageStream.Seek(12, SeekOrigin.Current);
            uint lba = reader.ReadUInt32();
            uint totalLength = reader.ReadUInt32();
            this.ImageStream.Seek(16, SeekOrigin.Current);
            uint sectorType = reader.ReadUInt32();

            if (totalLength != pregapLength + trackLength)
            {
                throw new DiscJugglerParseException("Track length is invalid");
            }

            if (sectorType > 2)
            {
                throw new DiscJugglerParseException("Sector type is invalid");
            }

            int sectorSize = ((DiscJugglerSectorSizes)sectorType).GetSectorSize();
            long dataOffset = trackOffset + pregapLength * sectorSize;


            long fad = pregapLength + lba;

            Track t = new Track
            {
                FrameAddr = fad,
                adr = 0,
                ctrl = sectorMode == 0 ? 0 : 4,
                FileName = fileName,
                file_offset = dataOffset - fad * sectorSize,
                TrackLength = trackLength,
                PregapLength = pregapLength,
                TotalLength = totalLength,
                SectorSize = sectorSize,
                    
            };
            (t.SectorFormat, t.HeaderSize, t.ErrorSize, t.DataSize) 
                = this.GetTrackLayout(sectorMode, sectorSize);

            trackOffset += totalLength * sectorSize;
            return t;
        }


        private (GDRomSectorFormats sectorFormat,
            int headerSize, 
            int errorSize, 
            int dataSize) 
            GetTrackLayout(uint sectorMode, int sectorSize)
        {

            if (sectorMode == 0 && sectorSize == 2352)
            {
                return (GDRomSectorFormats.CDDA, 0, 0, 2352);
            }
            else if (sectorMode == 1 && sectorSize == 2048)
            {

                return (GDRomSectorFormats.Mode1, 0, 0, 2048);
            }
            else if (sectorMode == 1 && sectorSize == 2352)
            {
                return (GDRomSectorFormats.Mode1, 16, 288, 2048);
            }
            else if (sectorMode == 1 && sectorSize == 2336)
            {
                return (GDRomSectorFormats.Mode1, 0, 288, 2048);
            }
            else if (sectorMode == 2 && sectorSize == 2048)
            {
                return (GDRomSectorFormats.Mode2Form1, 0, 0, 2048);
            }
            else if (sectorMode == 2 && sectorSize == 2352)
            {
                return (GDRomSectorFormats.Mode2Form1, 24, 280, 2048);
            }
            else if (sectorMode == 2 && sectorSize == 2336)
            {
                return (GDRomSectorFormats.Mode1, 0, 280, 2048);
            }

            throw new DiscJugglerParseException("Unsupported GD-ROM mode.");

        }
        private (uint rawVersion, uint headerOffset, DiscJugglerVersions djVersion) ParseHeader()
        {
           
            using var reader = new BinaryReader(this.ImageStream, Encoding.UTF8, true);
            this.ImageStream.Seek(-8, SeekOrigin.End);
            uint rawVersion = reader.ReadUInt32();
            uint headerOffset = reader.ReadUInt32();
            if (headerOffset == 0) throw new DiscJugglerParseException("Header offset was not valid.");
            DiscJugglerVersions djVersion;
            switch (rawVersion)
            {
                case (uint)DiscJugglerVersions.Version2:
                    djVersion = DiscJugglerVersions.Version2;
                    break;
                case (uint)DiscJugglerVersions.Version3:
                    djVersion = DiscJugglerVersions.Version3;
                    break;
                case (uint)DiscJugglerVersions.Version35:
                    djVersion = DiscJugglerVersions.Version35;
                    break;
                default:
                    djVersion = DiscJugglerVersions.UnknownVersion;
                    break;
            }
            return (rawVersion, headerOffset, djVersion);
        }
        #endregion

        private byte[] ReadSector(Track track, long frameAddr)
        {
            long offset = track.file_offset + frameAddr * track.SectorSize;
            using var reader = new BinaryReader(this.ImageStream, Encoding.UTF8, true);
            this.ImageStream.Seek(offset, SeekOrigin.Begin);
            this.ImageStream.Seek(track.HeaderSize, SeekOrigin.Current);
            return reader.ReadBytes(track.DataSize);
        }

        /*
        private Stream ReadBytes(long frameAddr, long len)
        {
            using MemoryStream writeStream = new MemoryStream();
            long remaining = len;
            while(remaining != 0)
            {
                var read = this.ReadSectors(frameAddr, 1);
                writeStream.Write(read);
                remaining -= read.LongLength;
                frameAddr++;
            }
            return writeStream;
        }*/

        /// <summary>
        /// Opens the given LBA block from the first track of the data session.
        /// 
        /// The returned Stream acts simply as a pointer to the LBA and does not protect
        /// against out of bounds reads. In order to read file data, use the much safer
        /// <see cref="DiscJugglerDisc.OpenFile"/>.
        /// </summary>
        /// <param name="lba">The block index to open from</param>
        /// <returns></returns>
        public Stream OpenBlock(int lba)
        {
            var session = this.Sessions[1]; // get data session
            var track = session.Tracks.First();
            return new DiscJugglerBlockStream(lba, track, this.ImageStream);
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

        /*
        private IEnumerable<ISODirectoryRecord> GetDirectoryRecords()
        {
            var bytes = this.ReadSectors(this.Sessions[1].Tracks.First().FrameAddr + 16, 1);
            using var isoBlock = new MemoryStream(bytes);
            var pvd = new ISOPrimaryVolumeDescriptor(isoBlock);
            var rootDir = new ISODirectoryRecord(pvd.RootDirectoryEntryBytes);
            yield return rootDir;
            int rootLength = rootDir.DataSize;
            int rootFad = GDROM_PREGAP + rootDir.Extent;
            using var rootBlock = this.ReadBytes(rootFad, rootLength);
            using var binaryReader = new BinaryReader(rootBlock);
            
            long readBytes = 0;
            while (readBytes < rootLength)
            {
                rootBlock.Seek(readBytes, SeekOrigin.Begin);
                var dir = new ISODirectoryRecord(rootBlock);
                string fname = binaryReader.ReadString(dir.FileNameLength);
                dir.FileName = fname;
                yield return dir;
                readBytes += dir.Length;
            }
        } */

        public byte[] ReadSectors(Session session, int sectorsToRead) => this.ReadSectors(session.LeadInFrameAddr, sectorsToRead);

        public byte[] ReadSectors(long frameAddr, int sectorsToRead)
        {
            Track start = LookupTrack(frameAddr);
            if (start == null) throw new IndexOutOfRangeException("Frame Address provided was past the bounds of the disc.");
            long endfad = frameAddr + sectorsToRead;

            using MemoryStream sectorStream = new MemoryStream();
            for (long i = frameAddr; i < endfad; i++)
            {
                sectorStream.Write(this.ReadSector(start, i));
            }
            return sectorStream.ToArray();
        }

        Track? LookupTrack(long frameAddr)
        {
            int numTracks = this._tracks.Count;
            for (int i = 0; i < numTracks; i++)
            {
                if (frameAddr < _tracks[i].FrameAddr) continue;
                if ((i < numTracks - 1) && (frameAddr >= _tracks[i + 1].FrameAddr)) continue;
                return _tracks[i];
            }

            // todo: check if foreach loop works here?
            return null;
        }

        public class Track
        {
            /* frame adddress, equal to lba + 150 */
            public long FrameAddr { get; set; }
            public long TrackLength { get; set; }
            public long PregapLength { get; set; }

            public long TotalLength { get; set; }
            public string FileName { get; set; }
            /* type of information encoded in the sub q channel */
            public int adr { get; set; }
            /* type of track */
            public int ctrl { get; set; }

            /* sector layout */
            public GDRomSectorFormats SectorFormat { get; set; }
            public int SectorSize { get; set; }
            public int HeaderSize { get; set; }
            public int ErrorSize { get; set; }
            public int DataSize { get; set; }
            /* backing file */
            public long file_offset { get; set; }
        };

        public class Session
        {
            internal Session(DiscJugglerDisc disc, long leadIn, long leadOut, int firstTrack, int lastTrack)
            {
                this.Disc = disc;
                this.LeadInFrameAddr = leadIn;
                this.LeadOutFrameAddr = leadOut;
                this.FirstTrack = firstTrack;
                this.LastTrack = lastTrack;
            }

            private DiscJugglerDisc Disc { get; }
            public long LeadInFrameAddr { get; }
            public long LeadOutFrameAddr { get; }
            private int FirstTrack { get; }
            private int LastTrack { get; }
            public IEnumerable<Track> Tracks => Disc._tracks.Skip(FirstTrack).Take(LastTrack - FirstTrack + 1);
        };

    }


}