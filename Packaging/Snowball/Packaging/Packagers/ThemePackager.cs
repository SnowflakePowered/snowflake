using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snowball.Packaging.Packagers
{
    public class ThemePackager : Packager
    {
        public ThemePackager() : base("themes", PackageType.Theme)
        {

        }
        /// <summary>
        /// Make a theme package
        /// </summary>
        /// <param name="themeFolder">The input theme folder</param>
        /// <param name="infoFile"></param>
        /// <returns></returns>
        public override string Make(string themeFolder, string infoFile)
        {
            if (!File.Exists(Path.Combine(themeFolder, "theme.json")))
                throw new FileNotFoundException("Unable to find theme.json");
            infoFile = string.IsNullOrWhiteSpace(infoFile)
                ? File.ReadAllText(Path.Combine(themeFolder, "snowball.json"))
                : File.ReadAllText(infoFile);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
          
            return this.Make(themeFolder, packageInfo);
        }

        public override string Make(string themeFolder, PackageInfo packageInfo)
        {
            if (!File.Exists(Path.Combine(themeFolder, "theme.json")))
                throw new FileNotFoundException("Unable to find theme.json");
            string themeName =
                JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(
                    File.ReadAllText(Path.Combine(themeFolder, "theme.json")))["id"];
            string themeRootName = Path.GetFileName(themeFolder);
            if (themeName != themeRootName)
                throw new InvalidOperationException("Theme name and folder name are mismatched");
            string snowballDir = Packager.CopyResourceFiles(themeFolder, packageInfo);
            return snowballDir;
        }
    }
}
