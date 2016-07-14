using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Platform;
using Snowflake.Service;
using Snowflake.Utility;

namespace Snowflake.Input
{
    public class InputMappingStore
    {
        private readonly SqliteDatabase backingDatabase;

        internal void AddPlatforms(IStoneProvider stoneProvider)
        {
            //loop through all platforms
            //get valid controllers
            //create tables
            //create a new table for each controller?]

            //controller layouts table
            //PLATFORM Text for platformID
            //PORT Integer 0-15
            //DEVICE Text
            //LAYOUT Text
            //for each ControllerElement key, map to deviceElementKey

            //platform ports table
            //PLATFORM Text
            //LayoutPort0 - LayoutPort15, -> FakeDevice 
            //DevicePort0 - DevicePort15 -> RealDevice
        }

        public IMappedControllerElementCollection GetControllerMappings(string platformInfo, ControllerPort port)
        {
            throw new NotImplementedException();
        }

        public void SetMapping(string platformInfo, ControllerPort port, string layoutName, string deviceName)
        {
            
        }

        public void SetPortDevice(string platfotmInfo, ControllerPort port, string layoutName, string deviceName)
        {
            
        }

        public void CreateDatabase()
        {
            this.backingDatabase.CreateTable("ports", 
                "PlatformId TEXT",
                "PortNumber INTEGER",
                "Layout TEXT",
                "Device TEXT");
        }
    }
}
