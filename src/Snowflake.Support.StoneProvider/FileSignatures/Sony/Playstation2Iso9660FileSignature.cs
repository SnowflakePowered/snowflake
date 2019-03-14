using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DiscUtils.Iso9660;
using Snowflake.Romfile;

namespace Snowflake.Stone.FileSignatures.Sony
{
    public sealed class Playstation2Iso9660FileSignature : IFileSignature
    {
        /// <inheritdoc/>
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("BOOT2");

        /// <inheritdoc/>
        public bool HeaderSignatureMatches(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            var reader = new CDReader(romStream, true);
            var system = reader.OpenFile("SYSTEM.CNF", FileMode.Open);
            return new StreamReader(system).ReadToEnd().Contains("BOOT2");
        }

        /// <inheritdoc/>
        public string GetSerial(Stream romStream)
        {
            romStream.Seek(0, SeekOrigin.Begin);
            var reader = new CDReader(romStream, true);
            var system = reader.OpenFile("SYSTEM.CNF", FileMode.Open);
            return Regex.Match(new StreamReader(system).ReadToEnd(), "[A-Z]+_[0-9][0-9][0-9].[0-9][0-9]",
                RegexOptions.IgnoreCase).Value.Replace(".", string.Empty).Replace("_", "-");
        }

        /// <inheritdoc/>
        public string GetInternalName(Stream fileContents) => null;
    }
}
