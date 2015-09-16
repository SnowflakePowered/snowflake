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
namespace Snowflake.FileSignatures.NINTENDO_NES
{
    public sealed class NintendoNESFileSignature : FileSignature
    {
        [ImportingConstructor]
        public NintendoNESFileSignature([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            this.HeaderSignature = new byte[4] { 0x4E, 0x45, 0x53, 0x1A }; //'N' 'E' 'S' <EOF>
        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.Open(fileName, FileMode.Open))
                {
                    byte[] buffer = new byte[4];
                    romStream.Read(buffer, 0, buffer.Length);
                    return buffer.SequenceEqual(this.HeaderSignature);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
    
