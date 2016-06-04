using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-SEGA_CD")]
    public sealed class SegaCDFileSignature : FileSignature
    {
        public SegaCDFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = Encoding.UTF8.GetBytes("SEGADISCSYSTEM");
        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.OpenRead(fileName))
                {
                    romStream.Seek(0x10, SeekOrigin.Begin);
                    byte[] buffer = new byte[0xE];
                    romStream.Read(buffer, 0, buffer.Length);
                    return buffer.SequenceEqual(this.HeaderSignature);
                }
            }
            catch
            {
                return false;
            }
        }
        public override string GetSerial(string fileName)
        {
            using (FileStream romStream = File.OpenRead(fileName))
            {

                romStream.Seek(0x193, SeekOrigin.Begin);
                byte[] buffer = new byte[7];
                romStream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
            }
        }

        public override string GetInternalName(string fileName)
        {
            using (FileStream romStream = File.OpenRead(fileName))
            {
                romStream.Seek(0x130, SeekOrigin.Begin);
                byte[] buffer = new byte[48];
                romStream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer).Trim('\0').Trim();
            }
        }
    }
}
    
