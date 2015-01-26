using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;
using Snowflake.Tests;
using Newtonsoft.Json;
using Xunit;
namespace Snowflake.Platform.Tests
{
    public class PlatformInfoTests
    {
        /// <summary>
        /// Test platform definition loading
        /// </summary>
        /// <param name="platformId">platform ID to load</param>
        [Theory]
        [MemberData("TestedPlatforms")]
        public void PlatformLoadTests(string platformId)
        {
            string platformDefinition = TestUtilities.GetStringResource("Platforms." + platformId + ".platform");
            var protoTemplate = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(platformDefinition);
            var platform = PlatformInfo.FromJsonProtoTemplate(protoTemplate);
            Assert.Equal(platformId, platform.PlatformId);
        }

        public static IEnumerable<object[]> TestedPlatforms
        {
            get
            {
                // Or this could read from a file. :)
                return new[]
                {
                    new object[] { StonePlatforms.NINTENDO_NES },
                    new object[] { StonePlatforms.NINTENDO_SNES }
                };
            }
        }

    }
}
