using System.IO;
using System.Text;
using DiscUtils.Iso9660;
using Snowflake.Extensibility;
using Snowflake.Plugin.FileSignatures.SFOSharp;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.Plugin.FileSignatures
{
    [Plugin("SnowflakeFileSignature-SONY_PSP_ISO")]
    public sealed class SonyPSPISOFileSignature : FileSignature
    {
        public SonyPSPISOFileSignature(ICoreService coreInstance)
            : base(coreInstance)
        {
            this.HeaderSignature = Encoding.UTF8.GetBytes("PSP_GAME");
        }


        public override byte[] HeaderSignature { get; }

        public override bool HeaderSignatureMatches(string fileName)
        {
            try
            {
                using (FileStream isoStream = File.Open(fileName,
                        FileMode.Open))
                {
                    CDReader reader = new CDReader(isoStream, true);
                    return reader.DirectoryExists(Encoding.UTF8.GetString(this.HeaderSignature));
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
                using (
                    FileStream isoStream = File.Open(fileName,
                        FileMode.Open))
                {
                    CDReader reader = new CDReader(isoStream, true);
                    var system = reader.OpenFile(@"PSP_GAME\PARAM.SFO", FileMode.Open);
                    var sfo = new SFOReader(system);
                    return sfo.KeyValues.ContainsKey("DISC_ID") ? sfo.KeyValues["DISC_ID"] : sfo.KeyValues["TITLE_ID"];
                }
            }
            catch
            {
                return ""; 
            }
        }

        public override string GetInternalName(string fileName)
        {
            try
            {
                using (
                    FileStream isoStream = File.Open(fileName,
                        FileMode.Open))
                {
                    CDReader reader = new CDReader(isoStream, true);
                    var system = reader.OpenFile(@"PSP_GAME\PARAM.SFO", FileMode.Open);
                    var sfo = new SFOReader(system);
                    return sfo.KeyValues["TITLE"];
                }
            }
            catch
            {
                return "";
            }
        }
    }
}
