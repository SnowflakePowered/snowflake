using Newtonsoft.Json;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Service;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Snowflake.Input.Tests
{
    public class MappedControllerElementCollectionStore
    {
        [Fact]
        public void AddMappedInputCollectionGamepad_Test()
        {
            var testmappings = new StoneProvider().Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            var elementStore = new Input.MappedControllerElementCollectionStore();
            elementStore.SetMappedElements(mapcol);
            Assert.NotNull(elementStore.GetMappedElements(mapcol.DeviceId, mapcol.ControllerId));
        }

        [Fact]
        public void AddMappedInputCollectionKeyboard_Test()
        {
            var testmappings = new StoneProvider().Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.keyboard_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            var elementStore = new Input.MappedControllerElementCollectionStore();
            elementStore.SetMappedElements(mapcol);
            Assert.NotNull(elementStore.GetMappedElements(mapcol.DeviceId, mapcol.ControllerId));
        }
    }
}
