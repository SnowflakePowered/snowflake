using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
namespace Snowflake.Utility
{
    public static class PathUtility
    {
        public static string GetApplicationDataPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%appdata%"));
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%HOME%"), "Library", "Application Support");
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Path.Combine(Environment.ExpandEnvironmentVariables("%HOME%"), ".snowflake");
            }

            return Path.Combine(typeof(PathUtility).GetTypeInfo().Assembly.Location);
        }

        [Obsolete]
        public static string GetApplicationDataPath(params string[] dataPath)
        {
            return Path.Combine(dataPath.Prepend(PathUtility.GetApplicationDataPath()).ToArray());
        }

        [Obsolete]
        public static string GetSnowflakeDataPath()
        {
            return PathUtility.GetApplicationDataPath("snowflake");
        }

        [Obsolete]
        public static string GetSnowflakeDataPath(params string[] dataPath)
        {
            return Path.Combine(dataPath.Prepend(PathUtility.GetSnowflakeDataPath()).ToArray());
        }
    }
}