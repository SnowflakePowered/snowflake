using System.IO;
using System.Text;
using DiscUtils.Iso9660;
using Snowflake.Extensibility;
using Snowflake.FileSignatures.SFOSharp;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Snowflake.FileSignatures
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

        public override bool HeaderSignatureMatches(Stream romStream)
        {

            CDReader reader = new CDReader(romStream, true);
            return reader.DirectoryExists(Encoding.UTF8.GetString(this.HeaderSignature));
        }

        public override string GetSerial(Stream romStream)
        {

            CDReader reader = new CDReader(romStream, true);
            var system = reader.OpenFile(@"PSP_GAME\PARAM.SFO", FileMode.Open);
            var sfo = new SFOReader(system);
            return sfo.KeyValues.ContainsKey("DISC_ID") ? sfo.KeyValues["DISC_ID"] : sfo.KeyValues["TITLE_ID"];

        }

        public override string GetInternalName(Stream romStream)
        {
            CDReader reader = new CDReader(romStream, true);
            var system = reader.OpenFile(@"PSP_GAME\PARAM.SFO", FileMode.Open);
            var sfo = new SFOReader(system);
            return sfo.KeyValues["TITLE"];
        }
    }
}
