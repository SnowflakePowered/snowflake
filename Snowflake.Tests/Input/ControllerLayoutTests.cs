using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Snowflake.Input.Controller;
using Snowflake.JsonConverters;
using System.Linq;
using Xunit;
namespace Snowflake.Tests.Input
{
    public class ControllerLayoutTests
    {

        [Fact]
        public void ControllerParse_Test()
        {
            string controllerLayout = TestUtilities.GetStringResource("Controllers.NES_CONTROLLER.json");
            var controller = JsonConvert.DeserializeObject<ControllerLayout>(controllerLayout);
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            string x = JsonConvert.SerializeObject(controller, settings);
        }
    }

    
}
    