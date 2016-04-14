using System.Collections.Generic;
using Newtonsoft.Json;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Platform.Tests
{
    public class PlatformInfoTests
    {
       
        [Theory]
        [MemberData("TestedPlatforms")]
        public void LoadPlatformFromJson_Test(string platformId)
        {
            string platformDefinition = TestUtilities.GetStringResource($"Platforms.{platformId}.platform");
            var platform = JsonConvert.DeserializeObject<PlatformInfo>(platformDefinition);
            Assert.NotNull(platform);
        }
        [Theory]
        [MemberData("TestedPlatforms")]
        public void AssertPlatformDefinitionIDs_Test(string platformId)
        {
            string platformDefinition = TestUtilities.GetStringResource($"Platforms.{platformId}.platform");
            var platform = JsonConvert.DeserializeObject<PlatformInfo>(platformDefinition);
            Assert.Equal(platformId, platform.PlatformID);
        }

        public static IEnumerable<object[]> TestedPlatforms => new[]
        {
            new object[] { StonePlatforms.NINTENDO_NES },
            new object[] { StonePlatforms.NINTENDO_SNES }
        };
    }
}
