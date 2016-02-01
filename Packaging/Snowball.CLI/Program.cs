using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommandLine;
using Snowball.Packaging;
using Snowball.Packaging.Packagers;


namespace Snowflake.Packaging
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<PackOptions,
                MakePluginOptions, MakeThemeOptions, MakeEmulatorAssemblyOptions,
                InstallOptions, UninstallOptions, PublishOptions, AuthOptions, SignOptions, VerifyOptions>(args)
                .WithParsed<PackOptions>(options =>
                {
                    var package = Package.LoadDirectory(options.PluginRoot);
                    package.Pack(options.OutputDirectory ?? Environment.CurrentDirectory, options.PluginRoot);
                })
                .WithParsed<MakePluginOptions>(options =>
                {
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    Console.WriteLine(new PluginPackager().Make(Path.GetFullPath(options.PluginFile), options.PackageInfoFile));
                })
                .WithParsed<MakeEmulatorAssemblyOptions>(options =>
                {
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    Console.WriteLine(new EmulatorAssemblyPackager().Make(Path.GetFullPath(options.EmulatorDefinitionFile), options.PackageInfoFile));
                })
                .WithParsed<MakeThemeOptions>(options =>
                {
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    Console.WriteLine(new ThemePackager().Make(Path.GetFullPath(options.ThemeDirectory), options.PackageInfoFile));
                });
        }

        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}