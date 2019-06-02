using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Services;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Configuration.Tests
{
    public class DynamicProxyInputTests
    {
        [Fact]
        public void Init_Test()
        {
            var stoneProvider = new StoneProvider();
            var testmappings = stoneProvider.Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
            var x = new InputTemplate<IRetroArchInput>(mapcol);
            x[ControllerElement.ButtonA] = ControllerElement.DirectionalS;
            Assert.Equal(ControllerElement.DirectionalS, x.Template.InputPlayerABtn);
            Assert.Equal(x.Template.Configuration.InputPlayerA, x.Template.InputPlayerA);
        }

        [Fact]
        public void Setter_Test()
        {
            var stoneProvider = new StoneProvider();
            var testmappings = stoneProvider.Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
            var x = new InputTemplate<IRetroArchInput>(mapcol);
            x[ControllerElement.ButtonA] = ControllerElement.DirectionalS;
            Assert.Equal(ControllerElement.DirectionalS, x.Template.InputPlayerABtn);
        }

        [Fact]
        public void NestedSetter_Test()
        {
            var stoneProvider = new StoneProvider();
            var testmappings = stoneProvider.Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
            var x = new InputTemplate<IRetroArchInput>(mapcol);
            x[ControllerElement.ButtonA] = ControllerElement.DirectionalS;
            Assert.Equal(ControllerElement.DirectionalS, x.Template.Configuration.InputPlayerABtn);
        }

        [Fact]
        public void SuperNestedSetter_Test()
        {
            var stoneProvider = new StoneProvider();
            var testmappings = stoneProvider.Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));
            var mapcol = ControllerElementMappings.GetDefaultMappings(realmapping, testmappings);
            var x = new InputTemplate<IRetroArchInput>(mapcol);
            x[ControllerElement.ButtonA] = ControllerElement.DirectionalS;
            var y = (x as IConfigurationSection<IRetroArchInput>).Configuration["InputDevice"] = 1;
            var z = x.Template.Configuration.Values;
            var c = (x as IConfigurationSection<IRetroArchInput>).Values;
            Assert.Equal(ControllerElement.DirectionalS, x.Template.Template.Configuration.InputPlayerABtn);
            Assert.Equal(1, (x as IConfigurationSection<IRetroArchInput>).Configuration.InputDevice);
        }
    }
}
