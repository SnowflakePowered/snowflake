using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Packaging.Snowball;
using Newtonsoft.Json;
namespace Snowflake.Packaging
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var packageInfo = new PackageInfo("name-Test", "desc-Test", new List<string>() {"test-Auth"}, "1.0.0", new List<string>() { "testdep@1.0.0" }, PackageType.Plugin);
            string serialized = JsonConvert.SerializeObject(packageInfo);
            var newPackage = JsonConvert.DeserializeObject<PackageInfo>(serialized);

            var package = Package.LoadDirectory("Package");
            package.Pack(@"C:\dats", "Package");
        }
    }
}
