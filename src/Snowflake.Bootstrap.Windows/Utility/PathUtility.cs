using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Snowflake.Bootstrap.Windows.Utility
{
    internal static class PathUtility
    {
        public static DirectoryInfo GetApplicationDataPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%appdata%")));
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%HOME%"), "Library",
                    "Application Support"));
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%HOME%"), ".snowflake"));
            }

            return new DirectoryInfo(Path.Combine(typeof(PathUtility).GetTypeInfo().Assembly.Location));
        }
    }
}
