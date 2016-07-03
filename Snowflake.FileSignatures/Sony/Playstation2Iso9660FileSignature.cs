using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DiscUtils.Iso9660;

namespace Snowflake.Romfile.FileSignatures.Sony
{
    public sealed class Playstation2Iso9660FileSignature : IFileSignature
    {
        public byte[] HeaderSignature => Encoding.UTF8.GetBytes("BOOT2");


        public bool HeaderSignatureMatches(Stream romStream)
        {
            var reader = new CDReader(romStream, true);
            var system = reader.OpenFile("SYSTEM.CNF", FileMode.Open);
            return new StreamReader(system).ReadToEnd().Contains("BOOT2");
        }

        public string GetSerial(Stream romStream)
        {
            var reader = new CDReader(romStream, true);
            var system = reader.OpenFile("SYSTEM.CNF", FileMode.Open);
            return Regex.Match(new StreamReader(system).ReadToEnd(), "[A-Z]+_[0-9][0-9][0-9].[0-9][0-9]",
                            RegexOptions.IgnoreCase).Value.Replace(".", "").Replace("_", "-");
        }

        public string GetInternalName(Stream fileContents) => null;
    }
}
