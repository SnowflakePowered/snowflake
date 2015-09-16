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

namespace Snowflake.FileSignatures.NINTENDO_GCN
{
    public sealed class NintendoGCNISOFileSignature : FileSignature
    {
        [ImportingConstructor]
        public NintendoGCNISOFileSignature([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            this.HeaderSignature = new byte[4] { 0xC2, 0x33, 0x9F, 0x3D }; //gamecube magic word

        }
        
        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.Open(fileName, FileMode.Open))
                {
                    byte[] buffer = new byte[4]; // read 4 bytes for magic word

                    romStream.Seek(0x1C, SeekOrigin.Begin); 
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
            using (FileStream romStream = File.Open(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[5]; // game ids are 5 bytes long
                romStream.Read(buffer, 0, buffer.Length);
                string code = Encoding.UTF8.GetString(buffer);
                return code;
            }
        }

        public override string GetInternalName(string fileName)
        {
            using (FileStream romStream = File.Open(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[64]; 
                romStream.Seek(0x20, SeekOrigin.Begin);
                romStream.Read(buffer, 0, buffer.Length);
                string code = Encoding.UTF8.GetString(buffer).Trim('\0'); 
                return code;
            }
        }
    }
}
