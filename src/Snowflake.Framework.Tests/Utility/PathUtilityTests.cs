using System;
using System.IO;
using System.Runtime.InteropServices;
using Snowflake.Utility;
using Xunit;
namespace Snowflake.Utility.Tests
{
    public class PathUtilityTests
    {
        [Fact]
        public void GetApplicationDataPath_Tests()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Equal(Path.Combine(Environment.ExpandEnvironmentVariables("%appdata%")),
                 PathUtility.GetApplicationDataPath());
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Assert.Equal(Path.Combine(Environment.ExpandEnvironmentVariables("%HOME%"), "Library", "Application Support"),
                 PathUtility.GetApplicationDataPath());
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                 Assert.Equal(Path.Combine(Environment.ExpandEnvironmentVariables("%HOME%"), ".snowflake"),
                 PathUtility.GetApplicationDataPath());
            }
        }
    }
}
