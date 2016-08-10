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
namespace Snowflake.Input.Controller.Mapped
{
    public class SqliteMappedControllerElementCollectionStore : IMappedControllerElementCollectionStore
    {
        private readonly SqliteDatabase backingDatabase;

        public SqliteMappedControllerElementCollectionStore(SqliteDatabase backingDatabase)
        {
            this.backingDatabase = backingDatabase;
            this.CreateDatabase();
        }

        public IMappedControllerElementCollection GetMappingProfile(string controllerId, string deviceId, string profileName = "default")
        {
            return this.backingDatabase.Query<IMappedControllerElementCollection>(dbConnection =>
            {
                dynamic result = dbConnection.Query<dynamic>(@"SELECT * FROM mappings WHERE ControllerId = @controllerId AND DeviceId = @deviceId AND ProfileName = @profileName", 
                                new { controllerId, deviceId, profileName }).FirstOrDefault();
                if (result == null)
                    return null;

                var collection = new MappedControllerElementCollection(result.DeviceId, result.ControllerId);

                foreach (KeyValuePair<string, object> element in (IDictionary<string, object>)result)
                {
                    if (element.Key == "DeviceId" || element.Key == "ControllerId" || element.Key == "ProfileName" || element.Value == null) continue;
                    string deviceElem = (string)element.Value;
                    var controllerElement = new MappedControllerElement((ControllerElement)Enum.Parse(typeof(ControllerElement), element.Key));
                    if (deviceElem.StartsWith("Key"))
                    {
                        controllerElement.DeviceElement = ControllerElement.Keyboard;
                        controllerElement.DeviceKeyboardKey = (KeyboardKey)Enum.Parse(typeof(KeyboardKey), deviceElem);
                    }
                    else
                    {
                        controllerElement.DeviceElement = (ControllerElement)Enum.Parse(typeof(ControllerElement), deviceElem);
                    }
                    collection.Add(controllerElement);
                }
                return collection;
            });
        }

        public IEnumerable<string> GetProfileNames(string controllerId, string deviceId)
        {
            var profileNames = this.backingDatabase.Query(dbConnection =>
            {
                var result = dbConnection.Query<string>(@"SELECT ProfileName FROM mappings WHERE ControllerId = @controllerId AND DeviceId = @deviceId",
                                new { controllerId, deviceId });
                return result;
            });
            return profileNames;
        }

        public void SetMappingProfile(IMappedControllerElementCollection mappedCollection, string profileName = "default")
        {          
            this.backingDatabase.Execute(dbConnection =>
            {
                var query = SqliteMappedControllerElementCollectionStore.BuildQuery(mappedCollection, profileName);
                SqlMapper.Execute(dbConnection, $@"INSERT OR REPLACE INTO mappings ({query.Item2}) VALUES ({query.Item1})", query.Item3); //will this work?
            });
        }

        private static Tuple<string, string, dynamic> BuildQuery(IMappedControllerElementCollection mappedCollection, string profileName)
        {
            dynamic queryObject = new ExpandoObject();
            queryObject.ControllerId = mappedCollection.ControllerId;
            queryObject.DeviceId = mappedCollection.DeviceId;
            queryObject.ProfileName = profileName;
            var parameters = new StringBuilder("@ControllerId, @DeviceId, @ProfileName");
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

        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("mappings",
                "ControllerId TEXT NOT NULL",
                "DeviceId TEXT NOT NULL",
                "ProfileName TEXT NOT NULL",
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
                "Pointer2D TEXT",
                "Pointer3D TEXT",
                "PointerAxisPositiveX TEXT",
                "PointerAxisNegativeX TEXT",
                "PointerAxisPositiveY TEXT",
                "PointerAxisNegativeY TEXT",
                "PointerAxisPositiveZ TEXT",
                "PointerAxisNegativeZ TEXT",
                "Touchscreen TEXT",
                "Keyboard TEXT",
                "PRIMARY KEY (ControllerId, DeviceId, ProfileName)"
                );
        }
    }
}
