using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DiscUtils.Iso9660;
using Snowflake.Romfile;

namespace Snowflake.Stone.FileSignatures.Sony
{
    internal sealed class Playstation2Iso9660FileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("BOOT2");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            try
            {
                romStream.Seek(0, SeekOrigin.Begin);
                using var reader = new CDReader(romStream, true);
                using var system = reader.OpenFile("SYSTEM.CNF", FileMode.Open);
                using var streamReader = new StreamReader(system);

                return streamReader.ReadToEnd().Contains("BOOT2");
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
            using var reader = new CDReader(romStream, true);
            using var system = reader.OpenFile("SYSTEM.CNF", FileMode.Open);
            using var streamReader = new StreamReader(system);
            return Regex.Match(streamReader.ReadToEnd(), "[A-Z]+_[0-9][0-9][0-9].[0-9][0-9]",
                RegexOptions.IgnoreCase).Value.Replace(".", string.Empty).Replace("_", "-");
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream romStream)
        {
           romStream.Seek(0, SeekOrigin.Begin);
           using var reader = new CDReader(romStream, true);
           return reader.VolumeLabel;
        }
    }
}
