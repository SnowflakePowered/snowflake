using System.IO;
using System.Linq;
using Snowflake.Extensibility;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.Plugin.FileSignatures
{
    [Plugin("SnowflakeFileSignature-SEGA_GG")]
    public sealed class SegaGGFileSignature : FileSignature
    {
        public SegaGGFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = new byte[8] { 0x54, 0x4D, 0x52, 0x20, 0x53, 0x45, 0x47, 0x41 }; // 'TMR SEGA'
        }


        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream romStream = File.OpenRead(fileName))
                {
                    byte[] buffer = new byte[8];
                    romStream.Seek(0x7FF0, SeekOrigin.Begin);
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
            try
            {
                using (FileStream romStream = File.OpenRead(fileName))
                {
                    byte[] buffer = new byte[3];
                    romStream.Seek(0x7FFC, SeekOrigin.Begin);
                    romStream.Read(buffer, 0, buffer.Length);
                    string part1 = ((int)buffer[0]).ToString("X");
                    string part2 = ((int)buffer[1]).ToString("X");
                    string part3 = ((int)buffer[2]).ToString();
                    part3 = part3.Remove(part3.Length - 1);
                    if (part1.Length == 1) part1 = "0" + part1;
                    if (part2.Length == 1) part2 = "0" + part2;
                    string productCode = $"{part3}{part2}{part1}".Trim('0');
                    return productCode;
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
