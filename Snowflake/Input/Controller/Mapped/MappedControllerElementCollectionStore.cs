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
using System.Dynamic;
using Dapper;
namespace Snowflake.Input
{
    public class MappedControllerElementCollectionStore
    {
        private readonly SqliteDatabase backingDatabase;

        public MappedControllerElementCollectionStore()
        {
            this.backingDatabase = new SqliteDatabase("elements.db");
            this.CreateDatabase();
        }

        internal void AddPlatforms(IStoneProvider stoneProvider)
        {
            //loop through all platforms
            //get valid controllers
            //create tables
            //create a new table for each controller?]

            //controller layouts table
            //map a layout to device keys. 
            //same across platforms
            //LAYOUT
            //DEVICE
            //for each ControllerElement key, map to deviceElementKey

            //platform ports table
            //PLATFORM Text
            //LayoutPort0 - LayoutPort15, -> Fake Device 
            //DevicePort0 - DevicePort15 -> Real Device
        }
        public IMappedControllerElementCollection GetMappedElements(string layoutName, string deviceName)
        {
            throw new NotImplementedException();
        }

        public void SetMappedElements(IMappedControllerElementCollection mappedCollection)
        {          
            this.backingDatabase.Execute(dbConnection =>
            {
                var query = MappedControllerElementCollectionStore.BuildQuery(mappedCollection);
                SqlMapper.Execute(dbConnection, $@"INSERT OR REPLACE INTO mappings ({query.Item2}) VALUES ({query.Item1})", query.Item3); //will this work?
            });
        }
     
        public void SetMapping(string layoutName, string deviceName, string layoutKey, string realDeviceKey)
        {

        }

        public void SetPortDevice(string platfotmInfo, ControllerPort port, string layoutName, string deviceName)
        {
            
        }

        private static Tuple<string, string, dynamic> BuildQuery(IMappedControllerElementCollection mappedCollection)
        {
            dynamic queryObject = new ExpandoObject();
            queryObject.ControllerId = mappedCollection.ControllerId;
            queryObject.DeviceId = mappedCollection.DeviceId;
            var parameters = new StringBuilder("@ControllerId, @DeviceId");
            foreach(var element in mappedCollection)
            {
                string layoutElementName = Enum.GetName(typeof(ControllerElement), element.LayoutElement);
                string deviceElement = (element.DeviceElement == ControllerElement.Keyboard) ? 
                    Enum.GetName(typeof(KeyboardKey), element.DeviceKeyboardKey) : Enum.GetName(typeof(ControllerElement), element.DeviceElement);
                parameters.Append(", @");
                parameters.Append(layoutElementName);
                ((IDictionary <string, object>)queryObject)[layoutElementName] = deviceElement;
            }
            return new Tuple<string, string, dynamic>(parameters.ToString(), parameters.Replace("@", "").ToString(), queryObject);

        }

        public void CreateDatabase()
        {
            this.backingDatabase.CreateTable("mappings",
                "ControllerId TEXT NOT NULL",
                "DeviceId TEXT NOT NULL",
                "ButtonA TEXT",
                "ButtonB TEXT",
                "ButtonC TEXT",
                "ButtonX TEXT",
                "ButtonY TEXT",
                "ButtonZ TEXT",
                "ButtonL TEXT",
                "ButtonR TEXT",
                "ButtonStart TEXT",
                "ButtonSelect TEXT",
                "ButtonGuide TEXT",
                "ButtonClickL TEXT",
                "ButtonClickR TEXT",
                "Button0 TEXT",
                "Button1 TEXT",
                "Button2 TEXT",
                "Button3 TEXT",
                "Button4 TEXT",
                "Button5 TEXT",
                "Button6 TEXT",
                "Button7 TEXT",
                "Button8 TEXT",
                "Button9 TEXT",
                "Button10 TEXT",
                "Button11 TEXT",
                "Button12 TEXT",
                "Button13 TEXT",
                "Button14 TEXT",
                "Button15 TEXT",
                "Button16 TEXT",
                "Button17 TEXT",
                "Button18 TEXT",
                "Button19 TEXT",
                "Button20 TEXT",
                "Button21 TEXT",
                "Button22 TEXT",
                "Button23 TEXT",
                "Button24 TEXT",
                "Button25 TEXT",
                "Button26 TEXT",
                "Button27 TEXT",
                "Button28 TEXT",
                "Button29 TEXT",
                "Button30 TEXT",
                "Button31 TEXT",
                "DirectionalN TEXT",
                "DirectionalE TEXT",
                "DirectionalS TEXT",
                "DirectionalW TEXT",
                "DirectionalNE TEXT",
                "DirectionalNW TEXT",
                "DirectionalSE TEXT",
                "DirectionalSW TEXT",
                "TriggerLeft TEXT",
                "TriggerRight TEXT",
                "AxisLeftAnalogPositiveX TEXT",
                "AxisLeftAnalogNegativeX TEXT",
                "AxisLeftAnalogPositiveY TEXT",
                "AxisLeftAnalogNegativeY TEXT",
                "AxisRightAnalogPositiveX TEXT",
                "AxisRightAnalogNegativeX TEXT",
                "AxisRightAnalogPositiveY TEXT",
                "AxisRightAnalogNegativeY TEXT",
                "RumbleBig TEXT",
                "RumbleSmall TEXT",
                "PointerMouse TEXT",
                "PointerAxisPositiveX TEXT",
                "PointerAxisNegativeX TEXT",
                "PointerAxisPositiveY TEXT",
                "PointerAxisNegativeY TEXT",
                "PointerAxisPositiveZ TEXT",
                "PointerAxisNegativeZ TEXT",
                "PRIMARY KEY (ControllerId, DeviceId)"
                );

            this.backingDatabase.CreateTable("ports", 
                "PlatformId TEXT NOT NULL",
                "PortNumber INTEGER NOT NULL",
                "Layout TEXT NOT NULL",
                "Device TEXT NOT NULL");
        }
    }
}
