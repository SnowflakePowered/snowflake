using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel.Composition;
using Snowflake.Service;
using Snowflake.Identifier;

namespace Identifier.DatIdentifier
{
    public sealed class ClrMameProDat: BasePlugin, IIdentifier
    {
        public ClrMameProDat([Import("coreInstance")] FrontendCore coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            this.InitConfiguration();
        }

        public string IdentifyGame(string fileName, string platformId)
        {
            return IdentifyGame(File.OpenRead(fileName), platformId);
        }
        public string IdentifyGame(FileStream file, string platformId)
        {
            string crc32 = GetCrc32(file);
            file.Close();
            List<object> datFiles = this.PluginConfiguration.Configuration["dats"][platformId];

            var match = datFiles
                .Select(datFile => File.ReadAllText(Path.Combine(this.PluginDataPath, "dats", datFile.ToString())))
                .Select(datFile =>
                        Regex.Match(datFile, String.Format(@"(?<=rom \( name "").*?(?="" size \d+ crc {0})", crc32),
                            RegexOptions.IgnoreCase))
                            .First(gameMatch => gameMatch.Success);
     
            string gameName = Regex.Match(match.Value, @"(\[[^]]*\])*([\w\s]+)").Groups[2].Value;
            
            return gameName;
        }

        private string GetCrc32(FileStream file)
        {
            using (var crc32 = new Crc32())
            return BitConverter.ToString(crc32.ComputeHash(file)).Replace("-", String.Empty).ToLowerInvariant();

        }
    }
}
