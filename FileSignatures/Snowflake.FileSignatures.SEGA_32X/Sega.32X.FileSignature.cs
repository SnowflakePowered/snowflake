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
namespace Snowflake.FileSignatures.SEGA_32X
{
    public sealed class Sega32XFileSignature : FileSignature
    {
        [ImportingConstructor]
        public Sega32XFileSignature([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            this.HeaderSignature = Encoding.UTF8.GetBytes("SEGA 32X"); //SEGA 32X
        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.OpenRead(fileName))
                {
                    romStream.Seek(0x100, SeekOrigin.Begin);
                    byte[] buffer = new byte[8];
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

                romStream.Seek(0x183, SeekOrigin.Begin);
                byte[] buffer = new byte[0xB]; //32X serials are 0xB long instead of 0x7
                romStream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
            }
        }

        public override string GetInternalName(string fileName)
        {
            using (FileStream romStream = File.OpenRead(fileName))
            {
                romStream.Seek(0x120, SeekOrigin.Begin);
                byte[] buffer = new byte[48];
                romStream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
            }
        }
    }
}
    
