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

namespace Snowflake.Input.Tests
{
    public class InputMapperStoreTests
    {
        public void AddMappedInputCollection_Test()
        {
            var testmappings = new StoneProvider().Controllers.First().Value;
            var realmapping =
                JsonConvert.DeserializeObject<ControllerLayout>(
                    TestUtilities.GetStringResource("InputMappings.xinput_device.json"));
            var mapcol = MappedControllerElementCollection.GetDefaultMappings(realmapping, testmappings);
            new MappedControllerElementCollectionStore().SetMapping(mapcol);
        }
    }
}
