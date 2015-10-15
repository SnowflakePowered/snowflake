using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace Snowflake.Packaging.Installing
{
    public class InstallManager
    {
        public IDictionary<string, IList<string>> FileManifest { get; }
        public InstallManager(string appDataPath)
        {
            this.FileManifest = JsonConvert.DeserializeObject<IDictionary<string, IList<string>>>(File.ReadAllText(Path.Combine(appDataPath, ".snowballdb")));
        }


        public void InstallPackages (IList<string> packageQueue, string tempPath)
        {

            foreach(string packageString in packageQueue)
            {

                string packageId = packageString.Split('@')[0];
                string packageVersion = packageString.Split('@')[1];
                if (String.IsNullOrWhiteSpace(Directory.EnumerateFiles(tempPath).FirstOrDefault(path => path.Contains($"!{packageId}-"))))
                {

                };
            }
        }
    }
}
