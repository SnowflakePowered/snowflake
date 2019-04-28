using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Services;

namespace Snowflake.Stone.FileSignatures.Sega
{
    public sealed class SegaDreamcastDiscJugglerFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("SEGA SEGAKATANA SEGA ENTERPRISES");

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
                long streamPos =
                    stream.Length -
                    (i * buffer.Length); // read 1MiB chunks from the end, as the IP.BIN file is near the end of the ISO file.
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

            // https://github.com/discimagechef/DiscImageChef/blob/master/DiscImageChef.DiscImages/DiscJuggler/Identify.cs#L46
            romStream.Seek(-4, SeekOrigin.End);
            byte[] cdiDescriptorLength = new byte[4];
            romStream.Read(cdiDescriptorLength, 0, 4);
            int descriptorLength = BitConverter.ToInt32(cdiDescriptorLength, 0);

            if (descriptorLength >= romStream.Length) return false;

            byte[] descriptor = new byte[descriptorLength];
            romStream.Seek(-descriptorLength, SeekOrigin.End);
            romStream.Read(descriptor, 0, descriptorLength);

            // Sessions
            if (descriptor[0] > 99 || descriptor[0] == 0) return false;

            // Seems all sessions start with this data
            if (descriptor[1] != 0x00 || descriptor[3] != 0x00 || descriptor[4] != 0x00 || descriptor[5] != 0x00 ||
               descriptor[6] != 0x00 || descriptor[7] != 0x00 || descriptor[8] != 0x00 || descriptor[9] != 0x00 ||
               descriptor[10] != 0x01 || descriptor[11] != 0x00 || descriptor[12] != 0x00 || descriptor[13] != 0x00 ||
               descriptor[14] != 0xFF || descriptor[15] != 0xFF) return false;

            // Too many tracks
            if (descriptor[2] > 99) return false;

            // Finally, look for IP.BIN
            long headerPos = this.GetHeaderOffset(romStream);
            return headerPos != 0;
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            long headerPos = this.GetHeaderOffset(romStream);
            romStream.Seek(headerPos, SeekOrigin.Begin);
            byte[] buffer = new byte[0x100];
            byte[] data = new byte[10];
            romStream.Read(buffer, 0, buffer.Length);
            Array.Copy(buffer, 0x40, data, 0, data.Length);
            /*
            header = 0x0 SEGA SEGAKATANA for 0x10 bytes (0xF bytes without 0x20 space)
            internal name = 0x80 for 0x80 bytes
            serial number (id) = 0x40 for 10 bytes
            */

            return Encoding.UTF8.GetString(data).Trim();
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            long headerPos = this.GetHeaderOffset(romStream);
            romStream.Seek(headerPos, SeekOrigin.Begin);
            byte[] buffer = new byte[0x100];
            byte[] data = new byte[0x7f];
            romStream.Read(buffer, 0, buffer.Length);
            Array.Copy(buffer, 0x7f, data, 0, data.Length);
            /*
            header = 0x0 SEGA SEGAKATANA for 0x10 bytes (0xF bytes without 0x20 space)
            internal name = 0x80 for 0x80 bytes
            serial number (id) = 0x40 for 9 bytes
            */

            return Encoding.UTF8.GetString(data).Trim();
        }
    }
}
