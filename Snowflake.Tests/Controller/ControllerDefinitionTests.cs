using System.Collections.Generic;
using Newtonsoft.Json;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Controller.Tests
{
    public class ControllerDefinitionTests
    {
        [Theory]
        [InlineData("NES_CONTROLLER")]
        [InlineData("SNES_CONTROLLER")]
        public void LoadControllersFromJson_Test(string controllerId)
        {
            string controllerDefinition = TestUtilities.GetStringResource($"Controllers.{controllerId}.controller");
            var protoTemplate = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(controllerDefinition);
            var controller = ControllerDefinition.FromJsonProtoTemplate(protoTemplate);
            Assert.NotNull(controller);
        }

        [Theory]
        [InlineData("NES_CONTROLLER")]
        [InlineData("SNES_CONTROLLER")]
        public void AssertValidControllerName_Test(string controllerId)
        {
            string controllerDefinition = TestUtilities.GetStringResource($"Controllers.{controllerId}.controller");
            var protoTemplate = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(controllerDefinition);
            var controller = ControllerDefinition.FromJsonProtoTemplate(protoTemplate);
            Assert.Equal(controllerId, controller.ControllerID);
        }
       
    }
}
