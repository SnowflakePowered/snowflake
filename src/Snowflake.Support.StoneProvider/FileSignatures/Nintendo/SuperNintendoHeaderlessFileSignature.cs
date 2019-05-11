using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Romfile;

namespace Snowflake.Stone.FileSignatures.Nintendo
{
    internal sealed class SuperNintendoHeaderlessFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => new byte[] { };

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            int offset = 0;
            if (romStream.Length % 1024 == 0x200)
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

            // 0x21 = HiROM, 0x23 = SA-1 ROM, 0x31 = HiROM + FastROM, 0x35 = ExHiROM
            // 0x20 = LoROM, 0x30 = LoROM + FastROM, 0x32 = ExLoROM
            bool isHiRom = (hiRom == 0x21 || hiRom == 0x23 || hiRom == 0x31 || hiRom == 0x35);
            bool isLoRom = (loRom == 0x20 || loRom == 0x30 || loRom == 0x32);

            if (isHiRom)
            {
                offset += 0x8000;
            }

            // Neither HiROM nor LoROM
            if (!isHiRom && !isLoRom) return false;

            byte[] checksumBuf = new byte[4];
            romStream.Seek(0x7FDC + offset, SeekOrigin.Begin);
            romStream.Read(checksumBuf, 0, 4);

            // Sum of checksum bytes should be 0x1FE
            return checksumBuf[0] + checksumBuf[1] + checksumBuf[2] + checksumBuf[3] == 0x1FE;
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            byte[] buffer = new byte[4];
            int offset = 0;
            if (romStream.Length % 1024 == 0x200)
            {
                return null;
            }

            if (romStream.Length % 1024 != 0 && romStream.Length % 1024 != 0x200)
            {
                return null;
            }

            romStream.Seek(0xFFD5 + offset, SeekOrigin.Begin);
            int hiRom = romStream.ReadByte();

            // 0x21 = HiROM, 0x23 = SA-1 ROM, 0x31 = HiROM + FastROM, 0x35 = ExHiROM
            // 0x20 = LoROM, 0x30 = LoROM + FastROM, 0x32 = ExLoROM
            bool isHiRom = (hiRom == 0x21 || hiRom == 0x23 || hiRom == 0x31 || hiRom == 0x35);

            if (isHiRom)
            {
                offset += 0x8000;
            }

            romStream.Seek(0x7FB2 + offset, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            if (BitConverter.ToUInt32(buffer, 0) == 0xFFFFFFFF) return String.Empty;

            string code = Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
            return code;
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            byte[] buffer = new byte[21];
            int offset = 0;
            if (romStream.Length % 1024 == 0x200)
            {
                return null;
            }

            if (romStream.Length % 1024 != 0 && romStream.Length % 1024 != 0x200)
            {
                return null;
            }

            romStream.Seek(0xFFD5 + offset, SeekOrigin.Begin);
            int hiRom = romStream.ReadByte();

            // 0x21 = HiROM, 0x23 = SA-1 ROM, 0x31 = HiROM + FastROM, 0x35 = ExHiROM
            // 0x20 = LoROM, 0x30 = LoROM + FastROM, 0x32 = ExLoROM
            bool isHiRom = (hiRom == 0x21 || hiRom == 0x23 || hiRom == 0x31 || hiRom == 0x35);

            if (isHiRom)
            {
                offset += 0x8000;
            }

            romStream.Seek(0x7FC0 + offset, SeekOrigin.Begin);
            romStream.Read(buffer, 0, buffer.Length);
            string code = Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
            return code;
        }
    }
}
