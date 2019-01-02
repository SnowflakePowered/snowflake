using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Romfile;

namespace Snowflake.Plugin.Scraping.FileSignatures.Nintendo
{
    public class SuperNintendoSmcHeaderFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => new byte[7] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            byte[] buffer = new byte[7];
            int offset = 0x200;
            if (romStream.Length % 1024 != 0x200)
            {
                return false; // return false on an smc headered rom
            }

            if (romStream.Length % 1024 != 0 && romStream.Length % 1024 != 0x200)
            {
                return false;
            }

            romStream.Seek(0xFFD5 + offset, SeekOrigin.Begin);
            int hiRom = romStream.ReadByte();
            romStream.Seek(0x7FD5 + offset, SeekOrigin.Begin);
            int loRom = romStream.ReadByte();
            if (Enumerable.Range(0x20, 0x35).Contains(hiRom))
            {
                offset += 0x8000; // 0x7fff + 0x8000 = 0xffff
            }

            if (!Enumerable.Range(0x20, 0x35).Contains(hiRom) && !Enumerable.Range(0x20, 0x35).Contains(loRom))
            {
                return false;
            }

            romStream.Seek(0x7FB6 + offset, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            romStream.Seek(0x7FDA + offset, SeekOrigin.Begin);
            int fixedByte = romStream.ReadByte();
            return buffer.SequenceEqual(this.HeaderSignature) && fixedByte == 0x33;

            // this should equal to 7 bytes of 0 starting from 0x7FB6 (+ 0x200 if HiRom)
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            byte[] buffer = new byte[4];
            int offset = 0x200;
            if (romStream.Length % 1024 != 0x200)
            {
                return null;
            }

            if (romStream.Length % 1024 != 0 && romStream.Length % 1024 != 0x200)
            {
                return null;
            }

            romStream.Seek(0xFFD5 + offset, SeekOrigin.Begin);
            int hiRom = romStream.ReadByte();
            if (Enumerable.Range(0x20, 0x35).Contains(hiRom))
            {
                offset += 0x8000; // 0x7fff + 0x8000 = 0xffff
            }

            romStream.Seek(0x7FB2 + offset, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            byte[] buffer = new byte[21];
            int offset = 0x200;
            if (romStream.Length % 1024 != 0x200)
            {
                return null;
            }

            if (romStream.Length % 1024 != 0 && romStream.Length % 1024 != 0x200)
            {
                return null;
            }

            romStream.Seek(0xFFD5 + offset, SeekOrigin.Begin);
            int hiRom = romStream.ReadByte();
            if (Enumerable.Range(0x20, 0x35).Contains(hiRom))
            {
                offset += 0x8000; // 0x7fff + 0x8000 = 0xffff
            }

            romStream.Seek(0x7FC0 + offset, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0');
            return code;
        }
    }
}
