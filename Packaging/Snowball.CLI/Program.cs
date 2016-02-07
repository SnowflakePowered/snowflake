using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Newtonsoft.Json;
using Snowball.Installation;
using Snowball.Packaging;
using Snowball.Packaging.Packagers;
using Snowball.Publishing;
using Snowball.Secure;


namespace Snowflake.Packaging
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

            var result = Parser.Default.ParseArguments<MakePackageOptions, AuthOptions>(args)
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
                });
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
    //http://stackoverflow.com/a/23947777
    public static class ConsoleEx
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleHistoryInfo(CONSOLE_HISTORY_INFO ConsoleHistoryInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleHistoryInfo(CONSOLE_HISTORY_INFO ConsoleHistoryInfo);

        [StructLayout(LayoutKind.Sequential)]
        private class CONSOLE_HISTORY_INFO
        {
            public uint cbSize;
            public uint BufferSize;
            public uint BufferCount;
            public uint TrimDuplicates;
        }

        public static void ClearConsoleHistory()
        {
            var chi = new CONSOLE_HISTORY_INFO();
            chi.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(CONSOLE_HISTORY_INFO));

            if (!GetConsoleHistoryInfo(chi))
            {
                return;
            }

            var originalBufferSize = chi.BufferSize;
            chi.BufferSize = 0;

            if (!SetConsoleHistoryInfo(chi))
            {
                return;
            }

            chi.BufferSize = originalBufferSize;

            if (!SetConsoleHistoryInfo(chi))
            {
                return;
            }
        }
    }
}