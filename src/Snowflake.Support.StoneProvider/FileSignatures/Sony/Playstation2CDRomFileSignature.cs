using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DiscUtils.Iso9660;
using Snowflake.Romfile;
using Snowflake.Stone.FileSignatures.Formats.CDXA;

namespace Snowflake.Stone.FileSignatures.Sony
{
    internal sealed class Playstation2CDRomFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("BOOT2");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            try
            {
                romStream.Seek(0, SeekOrigin.Begin);
                var reader = new PlaystationDisc(new CDXADisc(romStream));
                string systemcnf = reader.GetMeta();
                return systemcnf?.Contains("BOOT2") ?? false;
            }
            catch 
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            var reader = new PlaystationDisc(new CDXADisc(romStream));
            string systemcnf = reader.GetMeta();
            if (systemcnf == null) return null;
            return Regex.Match(systemcnf, "[A-Z]+_[0-9][0-9][0-9].[0-9][0-9]",
                RegexOptions.IgnoreCase).Value.Replace(".", string.Empty).Replace("_", "-");
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            var disk = new PlaystationDisc(new CDXADisc(romStream));
            return disk.InternalName;
        }
    }
}
