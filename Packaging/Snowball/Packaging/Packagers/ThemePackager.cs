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
        /// <param name="inputFile">The input theme.json</param>
        /// <param name="infoFile"></param>
        /// <returns></returns>
        public override string Make(string inputFile, string infoFile)
        {
            if (!File.Exists(inputFile))
                throw new FileNotFoundException("Unable to find theme.json");
            infoFile = string.IsNullOrWhiteSpace(infoFile)
                ? File.ReadAllText(Path.Combine(inputFile, "snowball.json"))
                : File.ReadAllText(infoFile);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
          
            return this.Make(inputFile, packageInfo);
        }

        public override string Make(string inputFile, PackageInfo packageInfo)
        {
            if (!File.Exists(inputFile))
                throw new FileNotFoundException("Unable to find theme.json");
            string themeName =
                JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(
                    File.ReadAllText(inputFile))["id"];
            string themeRoot = Path.GetDirectoryName(inputFile);
            string themeFolderName = Path.GetFileName(themeRoot);
            if (themeName != themeFolderName)
                throw new InvalidOperationException("Theme name and folder name are mismatched");
            string snowballDir = Packager.CopyResourceFiles(themeRoot, packageInfo);
            return snowballDir;
        }
    }
}
