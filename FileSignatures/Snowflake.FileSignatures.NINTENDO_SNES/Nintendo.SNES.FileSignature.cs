using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Romfile;
using Snowflake.Service;
using System.ComponentModel.Composition;
using System.Reflection;
using System.IO;
namespace Snowflake.FileSignatures.NINTENDO_SNES
{
    public sealed class NintendoSNESFileSignature : FileSignature
    {
        [ImportingConstructor]
        public NintendoSNESFileSignature([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            this.HeaderSignature = new byte[7] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.OpenRead(fileName))
                {
                    byte[] buffer = new byte[7];
                    int offset = 0;
                    if (romStream.Length % 1024 == 0x200) offset = 0x200;
                    if (romStream.Length % 1024 != 0 && romStream.Length % 1024 != 0x200) return false;
                    romStream.Seek(0xFFD5 + offset, SeekOrigin.Begin);
                    int hiRom = romStream.ReadByte();
                    romStream.Seek(0x7FD5 + offset, SeekOrigin.Begin);
                    int loRom = romStream.ReadByte();
                    if (Enumerable.Range(0x20, 0x35).Contains(hiRom))
                    {
                        offset += 0x8000; //0x7fff + 0x8000 = 0xffff 
                    }
                    if (!Enumerable.Range(0x20, 0x35).Contains(hiRom) && !Enumerable.Range(0x20, 0x35).Contains(loRom))
                    {
                        return false;
                    }
                    romStream.Seek(0x7FB6 + offset, SeekOrigin.Begin);
                    romStream.Read(buffer, 0, buffer.Length);
                    romStream.Seek(0x7FDA + offset, SeekOrigin.Begin);
                    int fixedByte = romStream.ReadByte();
                    return (buffer.SequenceEqual(this.HeaderSignature) && fixedByte == 0x33); // this should equal to 7 bytes of 0 starting from 0x7FB6 (+ 0x200 if HiRom)

                }
            }
            catch
            {
                return false;
            }
        }
        public override string GetGameId(string fileName)
        {
            using (FileStream romStream = File.OpenRead(fileName))
            {

                byte[] buffer = new byte[4];
                int offset = 0;
                if (romStream.Length % 1024 == 0x200) offset = 0x200;
                if (romStream.Length % 1024 != 0 && romStream.Length % 1024 != 0x200) return "";
                romStream.Seek(0xFFD5 + offset, SeekOrigin.Begin);
                int hiRom = romStream.ReadByte();
                if (Enumerable.Range(0x20, 0x35).Contains(hiRom))
                {
                    offset += 0x8000; //0x7fff + 0x8000 = 0xffff 
                }

                romStream.Seek(0x7FB2 + offset, SeekOrigin.Begin);
                romStream.Read(buffer, 0, buffer.Length);
                string code = Encoding.UTF8.GetString(buffer).Trim('\0');
                return code;
            }
        }

        public override string GetInternalName(string fileName)
        {
            using (FileStream romStream = File.OpenRead(fileName))
            {
                byte[] buffer = new byte[21];
                int offset = 0;
                if (romStream.Length % 1024 == 0x200) offset = 0x200;
                if (romStream.Length % 1024 != 0 && romStream.Length % 1024 != 0x200) return "";
                romStream.Seek(0xFFD5 + offset, SeekOrigin.Begin);
                int hiRom = romStream.ReadByte();
                if (Enumerable.Range(0x20, 0x35).Contains(hiRom))
                {
                    offset += 0x8000; //0x7fff + 0x8000 = 0xffff 
                }
                romStream.Seek(0x7FC0 + offset, SeekOrigin.Begin);
                romStream.Read(buffer, 0, buffer.Length);
                string code = Encoding.UTF8.GetString(buffer).Trim('\0');
                return code;
            }
        }
    }
}
