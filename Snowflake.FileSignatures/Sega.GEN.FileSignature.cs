using System.IO;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
{
    [Plugin("SnowflakeFileSignature-SEGA_GEN")]
    public sealed class SegaGENFileSignature : FileSignature
    {
        public SegaGENFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
        }

        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.OpenRead(fileName))
                {
                    romStream.Seek(0x100, SeekOrigin.Begin);
                    byte[] buffer = new byte[16];
                    romStream.Read(buffer, 0, buffer.Length);
                    return Encoding.UTF8.GetString(buffer).Contains("SEGA GENESIS") || Encoding.UTF8.GetString(buffer).Contains("SEGA MEGA DRIVE");
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
                byte[] buffer = new byte[7];
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
    
