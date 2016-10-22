using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.DynamicConfiguration;
using Snowflake.DynamicConfiguration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Service;
using Xunit;
namespace Snowflake.Tests.DynamicConfig
{
    public class DynamicProxyInputTests
    {
        [Fact]
        public void TestTemplate()
        {
            var stoneProvider = new StoneProvider();
            var testmappings = stoneProvider.Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            var x = new InputTemplate<IRetroArchInput>(mapcol);
            x[ControllerElement.ButtonA] = ControllerElement.DirectionalS;
            Assert.Equal(x.Template.InputPlayerABtn, ControllerElement.DirectionalS);
            Assert.Equal(x.Template.Configuration.InputPlayerA, x.Template.InputPlayerA);
        }

     
    }
}
