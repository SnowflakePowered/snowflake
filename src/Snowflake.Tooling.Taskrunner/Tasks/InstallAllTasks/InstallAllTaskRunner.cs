using System;
using System.IO;
using System.Threading.Tasks;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;
using Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTask;
using Snowflake.Tooling.Taskrunner.Tasks.InstallTask;

namespace Snowflake.Tooling.Taskrunner.Tasks.InstallAllTask
{
    [Task("install-all", "Installs all the packages in a directory.")]
    public class InstallAllTaskRunner : TaskRunner<InstallAllTaskArguments>
    {
        public override async Task<int> Execute(InstallAllTaskArguments arguments, string[] args)
        {
            DirectoryInfo moduleDirectory = arguments.ModuleDirectory != null
                ? new DirectoryInfo(Path.GetFullPath(arguments.ModuleDirectory))
                : PathUtility.GetDefaultModulePath();

            DirectoryInfo packageDirectory = arguments.PackageDirectory != null
                ? new DirectoryInfo(Path.GetFullPath(arguments.PackageDirectory))
                : DirectoryProvider.WorkingDirectory;

            int exitCode = 0;
            foreach (var package in packageDirectory.EnumerateFiles("*.snowpkg"))
            {
                try
                {
                    var pkg = new PackageInstaller(File.OpenRead(package.FullName));
                    if (!arguments.NoVerify)
                    {
                        Console.WriteLine($"Verifying {package.Name}...");
                        bool verified = await pkg.VerifyPackage();
                        if (!verified)
                        {
                            throw new InvalidOperationException(
                                $"Package failed to verify, use --noverify to install regardless.");
                        }
                    }

                    Console.WriteLine($"Installing package {package.Name}...");
                    string installPath = await pkg.InstallPackage(moduleDirectory);
                    Console.WriteLine($"Installed {package.Name} to {installPath}.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to install package {package.Name} due to {e.Message}.");
                    exitCode = 1;
                    continue;
                }
            }

            return exitCode;
        }
    }
}
