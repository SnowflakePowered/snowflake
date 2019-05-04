using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DiscUtils.Iso9660;
using Snowflake.Stone.FileSignatures.Formats.CDXA;
using Snowflake.Romfile;

namespace Snowflake.Stone.FileSignatures.Sony
{
    public sealed class PlaystationCDRomFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("PLAYSTATION");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            try
            {
                romStream.Seek(0, SeekOrigin.Begin);
                var disk = new PlaystationDisc(new CDXADisc(romStream));

                return disk.IsPlaystation();
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
            var disk = new PlaystationDisc(new CDXADisc(romStream));
            string syscnf = disk.GetMeta();
            string exe = syscnf.Substring(14, 11);
            return exe.Replace(".", string.Empty).Replace("_", "-");
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
