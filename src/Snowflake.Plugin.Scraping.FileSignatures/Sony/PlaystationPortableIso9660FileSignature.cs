using System.IO;
using System.Text;
using DiscUtils.Iso9660;
using Snowflake.Plugin.Scraping.FileSignatures.Formats.SFO;
using Snowflake.Romfile;

namespace Snowflake.Plugin.Scraping.FileSignatures.Sony
{
    public sealed class PlaystationPortableIso9660FileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("PSP_GAME");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            CDReader reader = new CDReader(romStream, true);
            return reader.DirectoryExists(Encoding.UTF8.GetString(this.HeaderSignature));
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            CDReader reader = new CDReader(romStream, true);
            var system = reader.OpenFile(@"PSP_GAME\PARAM.SFO", FileMode.Open);
            var sfo = new SFOReader(system);
            return sfo.KeyValues.ContainsKey("DISC_ID") ? sfo.KeyValues["DISC_ID"] : sfo.KeyValues["TITLE_ID"];
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            CDReader reader = new CDReader(romStream, true);
            var system = reader.OpenFile(@"PSP_GAME\PARAM.SFO", FileMode.Open);
            var sfo = new SFOReader(system);
            return sfo.KeyValues["TITLE"];
        }
    }
}
