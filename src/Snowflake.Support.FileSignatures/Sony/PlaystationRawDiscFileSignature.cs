using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DiscUtils.Iso9660;
using Snowflake.Romfile.FileSignatures.Formats.CDXA;

namespace Snowflake.Romfile.FileSignatures.Sony
{
    public sealed class PlaystationRawDiscFileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("PLAYSTATION");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            var disk = new PlaystationDisk(new CDXADisk(romStream));
            return disk.IsPlaystation();
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            var disk = new PlaystationDisk(new CDXADisk(romStream));
            string syscnf = disk.GetSystemCnf();
            string exe = syscnf.Substring(14, 11);
            return exe.Replace(".", string.Empty).Replace("_", "-");
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            var disk = new PlaystationDisk(new CDXADisk(romStream));
            return disk.InternalName;
        }
    }
}
