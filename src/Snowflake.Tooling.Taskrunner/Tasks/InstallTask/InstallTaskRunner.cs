using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;
using Snowflake.Tooling.Taskrunner.Tasks.PackTask;

namespace Snowflake.Tooling.Taskrunner.Tasks.InstallTask
{
    [Task("install", "Install the specified package.")]
    public class InstallTaskRunner : TaskRunner<InstallTaskArguments>
    {
        public override async Task<int> Execute(InstallTaskArguments arguments, string[] args)
        {
            if (arguments.PackagePath == null)
            {
                throw new InvalidOperationException("Must specify a package to install.");
            }

            DirectoryInfo moduleDirectory = arguments.ModuleDirectory != null
                ? new DirectoryInfo(Path.GetFullPath(arguments.ModuleDirectory))
                : PathUtility.GetDefaultModulePath();
            try
            {
                var pkg = new PackageInstaller(File.OpenRead(Path.GetFullPath(arguments.PackagePath)));
                if (!arguments.NoVerify)
                {
                    Console.WriteLine($"Verifying {arguments.PackagePath}...");
                    bool package = await pkg.VerifyPackage();
                    if (!package)
                    {
                        throw new InvalidOperationException(
                            $"Package failed to verify, use --noverify to install regardless.");
                    }
                }

                Console.WriteLine($"Installing package {arguments.PackagePath}...");
                string installPath = await pkg.InstallPackage(moduleDirectory);
                Console.WriteLine($"Installed {arguments.PackagePath} to {installPath}.");
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("The specified package was not found.");
            }

            return 0;
        }
    }
}
