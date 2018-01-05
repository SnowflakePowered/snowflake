using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Services;

namespace Snowflake.Plugin.Scraping.FileSignatures.Sega
{
    public sealed class SegaDcRawImageFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => new byte[15] { 0x53, 0x45, 0x47, 0x41, 0x20, 0x53, 0x45, 0x47, 0x41, 0x4B, 0x41, 0x54, 0x41, 0x4E, 0x41 };

        // adapted from http://stackoverflow.com/posts/332667
        private List<int> IndexOfSequence(byte[] buffer, byte[] pattern, int startIndex)
        {
            List<int> positions = new List<int>();
            int i = Array.IndexOf<byte>(buffer, pattern[0], startIndex);
            while (i >= 0 && i <= buffer.Length - pattern.Length)
            {
                byte[] segment = new byte[pattern.Length];
                Buffer.BlockCopy(buffer, i, segment, 0, pattern.Length);
                if (segment.SequenceEqual<byte>(pattern))
                {
                    positions.Add(i);
                }

                i = Array.IndexOf<byte>(buffer, pattern[0], i + pattern.Length);
            }

            return positions;
        }

        private long GetHeaderOffset(Stream stream)
        {
            byte[] buffer = new byte[1024 * 1024]; // read a MiB at a time

            for (int i = 1; i < stream.Length / 1024; i++)
            {
                long streamPos = stream.Length - (i * buffer.Length); // read 1MiB chunks from the end, as the IP.BIN file is near the end of the ISO file.
                if (streamPos < 0)
                {
                    break;
                }

                stream.Position = streamPos;
                stream.Read(buffer, 0, buffer.Length);
                var index = this.IndexOfSequence(buffer, this.HeaderSignature, 0);
                if (index.Count <= 0)
                {
                    continue;
                }

                int bufferIndex = index[0];
                long streamIndex = streamPos + bufferIndex;
                return streamIndex;
            }

            return 0;
        }

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            long headerPos = this.GetHeaderOffset(romStream);
            return headerPos != 0;
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            long headerPos = this.GetHeaderOffset(romStream);
            romStream.Seek(headerPos, SeekOrigin.Begin);
            byte[] buffer = new byte[0x100];
            byte[] data = new byte[0x9];
            romStream.Read(buffer, 0, buffer.Length);
            Array.Copy(buffer, 0x40, data, 0, data.Length);
            /*
            header = 0x0 SEGA SEGAKATANA for 0x10 bytes (0xF bytes without 0x20 space)
            internal name = 0x80 for 0x80 bytes
            serial number (id) = 0x40 for 9 bytes
            */

            return Encoding.UTF8.GetString(data);
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            long headerPos = this.GetHeaderOffset(romStream);
            romStream.Seek(headerPos, SeekOrigin.Begin);
            byte[] buffer = new byte[0x100];
            byte[] data = new byte[0x80];
            romStream.Read(buffer, 0, buffer.Length);
            Array.Copy(buffer, 0x80, data, 0, data.Length);
            /*
            header = 0x0 SEGA SEGAKATANA for 0x10 bytes (0xF bytes without 0x20 space)
            internal name = 0x80 for 0x80 bytes
            serial number (id) = 0x40 for 9 bytes
            */

            return Encoding.UTF8.GetString(data).Trim();
        }
    }
}
