using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Romfile;

namespace Snowflake.Stone.FileSignatures.Nintendo
{
    public class WiiIso9660FileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => new byte[4] {0x5D, 0x1C, 0x9E, 0xA3}; // wii magic word

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            byte[] buffer = new byte[4];
            byte[] wbfs = new byte[5];
            romStream.Position = 0;
            romStream.Read(wbfs, 0, wbfs.Length);
            if (wbfs.SequenceEqual(new byte[5] {0x57, 0x42, 0x46, 0x53, 0x00}))
            {
                return false; // 'W' 'B' 'F' 'S'
            }

            romStream.Seek(0x18, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            return buffer.SequenceEqual(this.HeaderSignature);
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            byte[] buffer = new byte[6];
            byte[] wbfs = new byte[5];
            romStream.Position = 0;
            romStream.Read(wbfs, 0, wbfs.Length);
            if (wbfs.SequenceEqual(new byte[5] {0x57, 0x42, 0x46, 0x53, 0x00}))
            {
                return null; // 'W' 'B' 'F' 'S'
            }

            romStream.Seek(0, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer);
            return code;
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            byte[] buffer = new byte[64];
            byte[] wbfs = new byte[5];
            romStream.Position = 0;
            romStream.Read(wbfs, 0, wbfs.Length);
            if (wbfs.SequenceEqual(new byte[5] {0x57, 0x42, 0x46, 0x53, 0x00}))
            {
                return null; // 'W' 'B' 'F' 'S'
            }

            romStream.Seek(0x20, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }
    }
}
