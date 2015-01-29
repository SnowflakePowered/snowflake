using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Controller;
using Snowflake.Tests;
using Newtonsoft.Json;
using Xunit;

namespace Snowflake.Controller.Tests
{
    public class ControllerProfileTests
    {
        [Theory]
        [MemberData("TestedControllers")]
        public void LoadProfileFromJson_Test(string controllerId, string deviceName)
        {
            string controllerProfile = TestUtilities.GetStringResource("ControllerProfiles." + controllerId + String.Format(".{0}-{1}.json", deviceName, controllerId));
            var protoTemplate = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(controllerProfile);
            var profile = ControllerProfile.FromJsonProtoTemplate(protoTemplate);
            Assert.NotNull(profile);
        }

        [Theory]
        [MemberData("TestedControllers")]
        public void SaveProfileFromJson_Test(string controllerId, string deviceName)
        {
            string platformDefinition = TestUtilities.GetStringResource("ControllerProfiles." + controllerId + String.Format(".{0}-{1}.json", deviceName, controllerId));
            var protoTemplate = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(platformDefinition);
            var profile = ControllerProfile.FromJsonProtoTemplate(protoTemplate);
            var result = profile.ToSerializable();
            Assert.Equal(controllerId, result["ControllerID"]);
        }

        public static IEnumerable<object[]> TestedControllers
        {
            get
            {
                // Or this could read from a file. :)
                return new[]
                {
                    new object[] { "NES_CONTROLLER" , "KeyboardDevice" },
                    new object[] { "NES_CONTROLLER" , "KeyboardDevice"},
                    new object[] { "SNES_CONTROLLER", "XInputDevice1" },
                    new object[] { "SNES_CONTROLLER" , "XInputDevice1"}
                };
            }
        }
    }
}
