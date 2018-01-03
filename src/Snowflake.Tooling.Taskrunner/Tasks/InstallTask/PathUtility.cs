using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Snowflake.Tooling.Taskrunner.Tasks.InstallTask
{
    internal static class PathUtility
    {
        public static DirectoryInfo GetDefaultModulePath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%appdata%")))
                    .CreateSubdirectory("snowflake")
                    .CreateSubdirectory("modules");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%HOME%"), "Library", "Application Support"))
                    .CreateSubdirectory("snowflake")
                    .CreateSubdirectory("modules");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%HOME%"), ".snowflake"))
                    .CreateSubdirectory("snowflake")
                    .CreateSubdirectory("modules");
            }

            return new DirectoryInfo(Path.Combine(typeof(PathUtility).GetTypeInfo().Assembly.Location))
                .CreateSubdirectory("modules");
        }
    }
}