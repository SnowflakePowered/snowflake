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
    public class EmulatorAssemblyPackager : Packager
    {
        public EmulatorAssemblyPackager() : base("emulators", PackageType.EmulatorAssembly)
        {

        }
        /// <summary>
        /// Make an emulator package
        /// </summary>
        /// <param name="inputFile">The input emulatordef</param>
        /// <param name="infoFile"></param>
        /// <returns></returns>
        public override string Make(string inputFile, string infoFile)
        {
            if (!File.Exists(inputFile))
                throw new FileNotFoundException("Unable to find emulatordef");
            string defId =
                JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(inputFile))["id"];
            string emulatorRoot = Path.Combine(Path.GetDirectoryName(inputFile), defId);
            if (!Directory.Exists(emulatorRoot))
                throw new FileNotFoundException($"Unable to find emulator contents at {emulatorRoot}");
            infoFile = string.IsNullOrWhiteSpace(infoFile)
               ? File.ReadAllText(Path.Combine(emulatorRoot, "snowball.json"))
               : File.ReadAllText(infoFile);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
            return this.Make(inputFile, packageInfo);
        }

        public override string Make(string inputFile, PackageInfo packageInfo)
        {
            if (!File.Exists(inputFile))
                throw new FileNotFoundException("Unable to find emulatordef");
            string defId =
                JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(inputFile))[
                    "id"];
            string emulatorRoot = Path.Combine(Path.GetDirectoryName(inputFile), defId);
            if (!Directory.Exists(emulatorRoot))
                throw new FileNotFoundException($"Unable to find emulator contents at {emulatorRoot}");

            string snowballDir = Packager.CopyResourceFiles(emulatorRoot, packageInfo);
            File.Copy(inputFile, Path.Combine(snowballDir, Path.GetFileName(inputFile)));
            return snowballDir;
        }
    }
}
