using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace Snowflake.Controller
{
    public class ControllerProfileStore : IControllerProfileStore
    {
        
        readonly string controllerProfilesLocation;
        public IList<string> AvailableProfiles {
            get
            {
                return Directory.GetFiles(controllerProfilesLocation).Select(x => Path.GetFileNameWithoutExtension(x.Replace("-" + this.ControllerID, ""))).ToList();
            }
        }
        public string ControllerID { get; private set; }
        public ControllerProfileStore (IControllerDefinition controllerDefinition, string controllerProfileStoreRoot)
        {
            this.ControllerID = controllerDefinition.ControllerID;
            this.controllerProfilesLocation = Path.Combine(controllerProfileStoreRoot, this.ControllerID);
            if (!Directory.Exists(controllerProfileStoreRoot)) Directory.CreateDirectory(controllerProfileStoreRoot);
            if (!Directory.Exists(this.controllerProfilesLocation)) Directory.CreateDirectory(this.controllerProfilesLocation);
        }
        public IControllerProfile GetControllerProfile(string deviceName)
        {
            if (deviceName == null) deviceName = "null";
            return ControllerProfile.FromJsonProtoTemplate(
                JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(
                    File.ReadAllText(this.GetControllerProfileFilename(deviceName))));
        }
        public void SetControllerProfile(string deviceName, IControllerProfile controllerProfile)
        {
            File.WriteAllText(this.GetControllerProfileFilename(deviceName), JsonConvert.SerializeObject(controllerProfile.ToSerializable()));
        }
        public IControllerProfile this[string deviceName]
        {
            get
            {
                return this.GetControllerProfile(deviceName);
            }
            set
            {
                this.SetControllerProfile(deviceName, value);
            }
        }
        private string GetControllerProfileFilename(string deviceName)
        {
            return Path.Combine(this.controllerProfilesLocation, String.Format("{0}-{1}.json", deviceName, this.ControllerID));
        }
        public ControllerProfileStore(IControllerDefinition controllerDefinition) : this(controllerDefinition, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "controllerprofiles")) { }

    }
}
