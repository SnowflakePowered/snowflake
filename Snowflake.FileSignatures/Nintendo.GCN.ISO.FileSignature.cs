using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-NINTENDO_GCN_ISO")]
    public sealed class NintendoGCNISOFileSignature : FileSignature
    {
        public NintendoGCNISOFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = new byte[4] { 0xC2, 0x33, 0x9F, 0x3D }; //gamecube magic word

        }
        
        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.OpenRead(fileName))
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
            using (FileStream romStream = File.OpenRead(fileName))
            {
                byte[] buffer = new byte[5]; // game ids are 5 bytes long
                romStream.Read(buffer, 0, buffer.Length);
                string code = Encoding.UTF8.GetString(buffer);
                return code;
            }
        }

        public override string GetInternalName(string fileName)
        {
            using (FileStream romStream = File.OpenRead(fileName))
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
