using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Base;
using Snowflake.API.Interface;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

namespace Identifier.DatIdentifier
{
    public class DatIdentifier:BasePlugin,IIdentifier
    {
        public DatIdentifier()
            : base(Assembly.GetExecutingAssembly())
        {

        }

        public string IdentifyGame(string fileName, string platformId)
        {
            return IdentifyGame(File.Open(fileName, FileMode.Open), platformId);
        }
        public string IdentifyGame(FileStream file, string platformId)
        {
            string crc32 = GetCrc32(file);
            string datFile = String.Empty;
            string gameName = Regex.Match(datFile, String.Format(@"(?<=rom \( name "").*?(?="" size \d+ crc {0})", crc32.ToLower()), RegexOptions.IgnoreCase).Value;
            gameName = Regex.Match(gameName, @"(\[[^]]*\])*([\w\s]+)").Groups[2].Value;

            return gameName;
        }

        public string GetCrc32(FileStream file)
        {
            using (Crc32 crc32 = new Crc32())
            return crc32.ComputeHash(file).ToString().Replace("-", String.Empty).ToLowerInvariant();

        }
    }
}
