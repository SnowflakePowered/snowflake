using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Snowflake.Packaging.Snowball;
using Snowflake.Packaging.Publishing;
using Newtonsoft.Json;
using CommandLine;
namespace Snowflake.Packaging
{
    static class Program
    {
        
        public static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<PackOptions,
                MakePluginOptions, MakeThemeOptions, MakeEmulatorAssemblyOptions,
                InstallOptions, UninstallOptions, PublishOptions, SetupOptions, SignOptions, VerifyOptions>(args)
                .WithParsed<PackOptions>(options =>
                {
                    var package = Package.LoadDirectory(options.PluginRoot);
                    package.Pack(options.OutputDirectory ?? Environment.CurrentDirectory, options.PluginRoot);
                })
                .WithParsed<MakePluginOptions>(options =>
                {
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    Package.MakeFromPlugin(options.PluginFile, options.PackageInfoFile, options.OutputDirectory);
                })
                .WithParsed<MakeEmulatorAssemblyOptions>(options =>
                {
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    Package.MakeFromEmulatorDefinition(options.EmulatorDefinitionFile, options.PackageInfoFile, options.OutputDirectory);
                })
                .WithParsed<MakeThemeOptions>(options =>
                {
                    options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
                    Package.MakeFromTheme(options.ThemeDirectory, options.PackageInfoFile, options.OutputDirectory);
                })
                .WithParsed<SignOptions>(options =>
                {
                    Signing.SignSnowball(options.FileName);
                })
                .WithParsed<VerifyOptions>(options =>
                {
                    bool signed = Signing.VerifySnowball(options.FileName, options.FileName + ".sig", options.FileName + ".key");
                    Console.WriteLine(signed);
                })
                .WithParsed<PublishOptions>(options =>
                {
                    if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.githubToken) && !String.IsNullOrWhiteSpace(Properties.Settings.Default.nugetToken))
                    {
                        Task.Run(async () =>
                        {
                            PublishActions.UploadNuget(PublishActions.PackNuget(options.PackageFile));
                            await PublishActions.MakeGithubIndex(Package.FromZip(options.PackageFile).PackageInfo);
                        }).Wait();
                    }
                    else
                    {
                        Console.WriteLine("Unable to find GitHub or NuGet details. Please run snowball setup to enter your GitHub details and NuGet API key.");
                    }
                })
                .WithParsed<SetupOptions>(options =>
                {
                Task.Run(async () => {
                    Account.SaveDetails(await Account.CreateGithubToken(options.GithubUser, options.GithubPassword), options.NuGetAPIKey);
                    await Account.MakeRepoFork(Account.GetGithubToken());
                    }).Wait();
                }); 
            

            var packageInfo = new PackageInfo("name-Test", "desc-Test", new List<string>() {"test-Auth"}, "1.0.0", new List<string>() { "testdep@1.0.0" }, PackageType.Plugin);
            string serialized = JsonConvert.SerializeObject(packageInfo);
            var newPackage = JsonConvert.DeserializeObject<PackageInfo>(serialized);

        }
    }
}
