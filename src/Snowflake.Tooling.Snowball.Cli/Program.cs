﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Newtonsoft.Json;
using Octokit;
using Snowball.Installation;
using Snowball.Packaging;
using Snowball.Packaging.Packagers;
using Snowball.Publishing;
using Snowball.Publishing.Secure;
using Snowflake.Packaging;


namespace Snowball.CLI
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        public static void Main(string[] args)
        {
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.WriteLine($"A fatal error occured: {((Exception) e.ExceptionObject).Message}");
                Environment.Exit(1);
            };
#endif
            string appDataDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowball");
            var localRepository = new LocalRepository(appDataDirectory);
            var accountKeyStore = new AccountKeyStore(appDataDirectory);
            var packageKeyStore = new PackageKeyStore(appDataDirectory);
            var installManager = new InstallManager(appDataDirectory, localRepository, packageKeyStore);
            var result = Parser.Default.ParseArguments<MakePackageOptions, AuthOptions, PublishOptions, InstallOptions, UninstallOptions>(
                args)
                .WithParsed<MakePackageOptions>(options =>
                {
                    try
                    {
                        Program.ProcessMakePackageOptions(options, packageKeyStore);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error ocurred when building your package: {e.Message}");
                    }
                })
                .WithParsed<AuthOptions>(options =>
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            await Program.ProcessAuthOptions(options, accountKeyStore);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error ocurred when saving your details: {e.Message}");
                        }
                    }).Wait();
                    Console.WriteLine("Waiting 5 seconds to clear your console for security purposes...");
                    Thread.Sleep(5000);
                    Console.Clear();
                    ConsoleEx.ClearConsoleHistory();
                })
                .WithParsed<PublishOptions>(options =>
                {
                    try
                    {
                        Program.ProcessPublishOptions(options, packageKeyStore, accountKeyStore, localRepository);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error ocurred when publishing your package: {e.Message}");

                    }
                })
                .WithParsed<InstallOptions>(options =>
                {
                    try
                    {
                        Program.ProcessInstallOptions(options, installManager, localRepository);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error ocurred when installing your package: {e.Message}");

                    }
                })
                .WithParsed<UninstallOptions>(options =>
                {
                    try
                    {
                        installManager.UninstallPackage(options.PackageId);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error ocurred when uninstalling your package: {e.Message}");

                    }
                });

        }

        private static void ProcessInstallOptions(InstallOptions options, InstallManager installManager, LocalRepository localRepository)
        {
            if (options.LocalInstall)
            {
                using (var snowball = new ZipArchive(File.OpenRead(options.Package)))
                    installManager.InstallRawPackage(snowball);
                return;
            }

            if (!localRepository.PluginExistsInRepository(options.Package))
                throw new FileNotFoundException("Package not found in repository");
            ReleaseInfo releaseInfo = localRepository.GetReleaseInfo(options.Package);
            var dependencies = localRepository.ResolveDependencies(options.Package);
            string downloadPath = Program.GetTemporaryDirectory();
            IDictionary<ReleaseInfo, string> nugetFiles = new Dictionary<ReleaseInfo, string>();
            Task.Run(async () =>
            {
                using (var webClient = new WebClient())
                {
                    var progress = new ProgressBar();
                    webClient.DownloadProgressChanged += (s, e) =>
                    {
                        progress?.Report(e.ProgressPercentage);
                    };
                    foreach (var dependency in dependencies)
                    {
                        string nugetDownloadPath = downloadPath + Path.GetRandomFileName();
                        string version = dependency.Item2?.ToString() ??
                                         dependency.Item1.ReleaseVersions.OrderByDescending(
                                             _version => _version.Key).First().Key.ToString();
                        string downloadUri = LocalRepository.GetNugetDownload(dependency.Item1, version);
                        Console.WriteLine(downloadUri);
                        Console.Write($"Downloading {dependency.Item1.Name} {version} | ");

                        await webClient.DownloadFileTaskAsync(downloadUri, nugetDownloadPath);
                        nugetFiles.Add(dependency.Item1, nugetDownloadPath);
                    }
                }
            }).Wait();
            foreach (KeyValuePair<ReleaseInfo, string> nugetPackages in nugetFiles)
            {
                using (var nupkg = new ZipArchive(File.OpenRead(nugetPackages.Value)))
                    installManager.InstallNupkg(releaseInfo, nupkg);
            }
        }
        private static void ProcessPublishOptions(PublishOptions options , PackageKeyStore packageKeyStore,
            AccountKeyStore accountKeyStore, LocalRepository localRepository)
        {
            NugetWrapper wrappedPackage = null;
            if (options.MakePackage)
            {
                if (Program.AtLeastTwo(options.MakePlugin, options.MakeTheme, options.MakeEmulator))
                    throw new InvalidOperationException("You can only specify a single type.");
                Packager packager = null;
                if (options.MakePlugin)
                {
                    Console.WriteLine("Building a plugin snowball...");
                    packager = new PluginPackager();
                }
                else if (options.MakeEmulator)
                {
                    Console.WriteLine("Building an emulator assembly snowball...");
                    packager = new EmulatorAssemblyPackager();
                }
                else if (options.MakeTheme)
                {
                    Console.WriteLine("Building a theme snowball...");
                    packager = new ThemePackager();
                }
                if (packager == null)
                    throw new InvalidOperationException("No package type specified.");
                //todo probably a more elegant way to this
                string packageRoot =
                    Path.GetDirectoryName(packager.Make(Path.GetFullPath(options.FileName),
                        options.PackageInfoFile));
                wrappedPackage = new NugetWrapper(Package.LoadDirectory(packageRoot), packageKeyStore);
            }
            else if (options.Prebuilt)
            {
                if (options.FullPath)
                {
                    Console.WriteLine(
                        "WARNING: --fullpath specified, will not confirm proper filename format. Your package may be rejected!");
                    if (!options.FileName.EndsWith(".snowball"))
                        throw new InvalidDataException("Snowball is not valid.");
                    wrappedPackage = new NugetWrapper(Package.LoadZip(options.FileName), packageKeyStore);
                }
                else
                {
                    string fileName =
                        Directory.EnumerateFiles(Environment.CurrentDirectory)
                            .FirstOrDefault(
                                _fileName =>
                                    (_fileName.EndsWith(".snowball") &&
                                     _fileName.Contains($"!{options.FileName}-")));
                    if (String.IsNullOrWhiteSpace(fileName))
                        throw new FileNotFoundException("Unable to find matching snowball in current directory");
                    wrappedPackage = new NugetWrapper(Package.LoadZip(fileName), packageKeyStore);
                }
            }
            if (wrappedPackage == null) throw new FileNotFoundException($"Unable to find snowball {options.FileName}");
            var publisher = new Publisher(wrappedPackage, accountKeyStore, localRepository);

            if (!options.GithubOnly)
            {
                for (int i = 0; i < options.RetryCount; i++)
                {
                    try
                    {
                        publisher.PushPackageToNuGet(options.Timeout);
                        break;
                    }
                    catch (WebException)
                    {
                        if (i == options.RetryCount) throw;
                        Console.WriteLine("ERROR: Web exception encountered, attempting to retry...");
                        continue;
                    }
                }
            }
            PullRequest prResult = null;
            for (int i = 0; i < options.RetryCount; i++)
            {
                try
                {
                    var pullRequest = publisher.MakePullRequest();
                    prResult = publisher.PublishPullRequest(pullRequest);
                    break;
                }
                catch (Exception e)
                {
                    if (i == options.RetryCount) throw;
                    Console.WriteLine("ERROR: Exception encountered, attempting to retry...");
                    Console.WriteLine(e.InnerException?.Message);
                    Console.WriteLine(e.Message);
                    continue;
                }
            }

            Console.WriteLine($"Package published for review at {prResult?.HtmlUrl}.");
        }
        private static void ProcessMakePackageOptions(MakePackageOptions options, PackageKeyStore packageKeyStore)
        {
            if (Program.AtLeastTwo(options.MakePlugin, options.MakeTheme, options.MakeEmulator))
                throw new InvalidOperationException("You can only specify a single type.");
            Packager packager = null;
            if (options.MakePlugin)
            {
                Console.WriteLine("Building a plugin snowball...");
                packager = new PluginPackager();
            }
            else if (options.MakeEmulator)
            {
                Console.WriteLine("Building an emulator assembly snowball...");
                packager = new EmulatorAssemblyPackager();
            } else if (options.MakeTheme)
            {
                Console.WriteLine("Building a theme snowball...");
                packager = new ThemePackager();
            }
            if (packager == null)
                throw new InvalidOperationException("No package type specified.");
            //todo probably a more elegant way to this
            options.OutputDirectory = options.OutputDirectory ?? Environment.CurrentDirectory;
            string packageRoot =
                Path.GetDirectoryName(packager.Make(Path.GetFullPath(options.FileName),
                    options.PackageInfoFile));
            if (!options.WrapNuget)
            {
                Console.WriteLine($"Snowball built to : {Package.LoadDirectory(packageRoot).Pack(options.OutputDirectory)}");
            }
            else
            {
                var nugetWrapper = new NugetWrapper(Package.LoadDirectory(packageRoot), packageKeyStore);
                var nugetPackage = nugetWrapper.MakeNugetPackage();
                File.Copy(nugetPackage.Item1,
                    Path.Combine(options.OutputDirectory, Path.GetFileName(nugetPackage.Item1)), true);
                File.WriteAllText(Path.Combine(options.OutputDirectory,
                    $"{nugetWrapper.Package.PackageInfo.Name}.{nugetWrapper.Package.PackageInfo.PackageType}.rel.json"
                    .ToLowerInvariant()),
                    JsonConvert.SerializeObject(nugetPackage.Item2));
                Console.WriteLine($"NuGet wrapped snowball built to {Path.Combine(options.OutputDirectory, Path.GetFileName(nugetPackage.Item1))}");

            }
        }

        private static async Task ProcessAuthOptions(AuthOptions options, AccountKeyStore accountKeyStore)
        {

            Console.WriteLine("Saving your account details securely...");
            string githubToken = await GithubAccountManager.InitializeGithubDetails(options.GithubUser, options.GithubPassword,
                options.GitHub2FA);
            accountKeyStore.SetKeys(options.NuGetAPIKey, githubToken);
        }

        private static bool AtLeastTwo(bool a, bool b, bool c)
        {
            //http://stackoverflow.com/questions/3076078/check-if-at-least-two-out-of-three-booleans-are-true
            return a ? (b || c) : (b && c);
        }
        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

    }
}
