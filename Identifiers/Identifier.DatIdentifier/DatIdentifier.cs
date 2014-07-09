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
using System.ComponentModel.Composition;
namespace Identifier.DatIdentifier
{
    public class DatIdentifier:BasePlugin,IIdentifier
    {
        public DatIdentifier()
            : base(Assembly.GetExecutingAssembly())
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
            Console.WriteLine(Path.Combine(this.PluginDataPath, "dats", this.PluginConfiguration.Configuration["dats"][platformId][0]));
            List<object> datFiles = this.PluginConfiguration.Configuration["dats"][platformId];
            string gameName = String.Empty;
            
            foreach (var datFile  in datFiles)
            {
                string dat = File.ReadAllText(Path.Combine(this.PluginDataPath, "dats", datFile.ToString()));
                Match gameMatch = Regex.Match(dat, String.Format(@"(?<=rom \( name "").*?(?="" size \d+ crc {0})", crc32), RegexOptions.IgnoreCase);
                if (gameMatch.Success)
                {
                    gameName = gameMatch.Value;
                    break;
                }
            }
            gameName = Regex.Match(gameName, @"(\[[^]]*\])*([\w\s]+)").Groups[2].Value;
            
            return gameName;
        }

        private string GetCrc32(FileStream file)
        {
            using (Crc32 crc32 = new Crc32())
            return BitConverter.ToString(crc32.ComputeHash(file)).Replace("-", String.Empty).ToLowerInvariant();

        }
    }
}
