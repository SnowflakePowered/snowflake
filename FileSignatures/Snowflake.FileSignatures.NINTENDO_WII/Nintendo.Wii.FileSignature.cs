using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures.NINTENDO_WII
{
    public sealed class NintendoWiiFileSignature : FileSignature
    {
        [ImportingConstructor]
        public NintendoWiiFileSignature([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            this.HeaderSignature = new byte[4] { 0x5D, 0x1C, 0x9E, 0xA3 }; // wii magic word

        }
        
        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.OpenRead(fileName))
                {

                    byte[] buffer = new byte[4];
                    byte[] wbfs = new byte[5];
                    int offset = 0;
                    romStream.Position = 0;
                    romStream.Read(wbfs, 0, wbfs.Length);
                    if (wbfs.SequenceEqual(new byte[5] { 0x57, 0x42, 0x46, 0x53, 0x00 })) offset = 0x200; //'W' 'B' 'F' 'S'
                    romStream.Seek(0x18 + offset, SeekOrigin.Begin); 
                    romStream.Read(buffer, 0, buffer.Length);
                    return buffer.SequenceEqual(this.HeaderSignature);
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
                byte[] wbfs = new byte[5];
                int offset = 0;
                romStream.Position = 0;
                romStream.Read(wbfs, 0, wbfs.Length);
                if (wbfs.SequenceEqual(new byte[5] { 0x57, 0x42, 0x46, 0x53, 0x00 })) offset = 0x200; //'W' 'B' 'F' 'S'
                romStream.Seek(offset, SeekOrigin.Begin);
                romStream.Read(buffer, 0, buffer.Length);
                string code = Encoding.UTF8.GetString(buffer);
                return code;
            }
        }

        public override string GetInternalName(string fileName)
        {
            using (FileStream romStream = File.OpenRead(fileName))
            {

                byte[] buffer = new byte[4];
                byte[] wbfs = new byte[5];
                int offset = 0;
                romStream.Position = 0;
                romStream.Read(wbfs, 0, wbfs.Length);
                if (wbfs.SequenceEqual(new byte[5] { 0x57, 0x42, 0x46, 0x53, 0x00 })) offset = 0x200; //'W' 'B' 'F' 'S'
                romStream.Seek(0x20 + offset, SeekOrigin.Begin);
                romStream.Read(buffer, 0, buffer.Length);
                string code = Encoding.UTF8.GetString(buffer).Trim('\0'); 
                return code;
            }
        }
    }
}
