using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using NuGet;
using Snowflake.Packaging.Snowball;
namespace Snowflake.Packaging.Publishing
{
    internal static class NuGetPackage
    {
        public static string PackNuget(string snowballFilename)
        {
            Signing.SignSnowball(snowballFilename);
            string sigFile = snowballFilename + ".sig";
            string keyFile = snowballFilename + ".key";
            string packagePath = snowballFilename + ".nupkg";
            PackageInfo packageInfo;
            using(ZipArchive snowball = new ZipArchive(File.Open(snowballFilename, FileMode.Open), ZipArchiveMode.Read))
            {
                packageInfo = JsonConvert.DeserializeObject<PackageInfo>(new StreamReader(snowball.GetEntry("snowball.json").Open()).ReadToEnd());
            }
            ManifestMetadata metadata = new ManifestMetadata()
            {
                Authors = String.Join(", ", packageInfo.Authors),
                Version = packageInfo.Version.ToString(),
                Id = $"snowflake-snowball-{packageInfo.PackageType}-{packageInfo.Name}",
                Description = packageInfo.Description,
                Tags = "snowflake snowball"
            };

            PackageBuilder builder = new PackageBuilder();
            var files = new string[3]
            {
                Path.GetFullPath(snowballFilename),
                Path.GetFullPath(keyFile),
                Path.GetFullPath(sigFile)
            }.Select(f => new ManifestFile { Source = f, Target = f.Replace(Path.GetDirectoryName(f), "") })
            .ToList(); 
            builder.PopulateFiles("", files);
            builder.Populate(metadata);
            using (FileStream stream = File.Open(packagePath, FileMode.OpenOrCreate))
            {
                builder.Save(stream);
            }
            return packagePath;


        }
    }
}
