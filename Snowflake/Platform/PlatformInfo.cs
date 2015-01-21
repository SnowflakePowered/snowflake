using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Information.MediaStore;
using Snowflake.Information;
using Snowflake.Controller;
using System.Collections.ObjectModel;
using Snowflake.Extensions;
using Newtonsoft.Json.Linq;

namespace Snowflake.Platform
{
    public class PlatformInfo : Info, IPlatformInfo
    {
        public PlatformInfo(string platformId, string name, IMediaStore mediastore, IDictionary<string, string> metadata, IList<string> fileExtensions, IPlatformDefaults platformDefaults, IDictionary<string, IControllerDefinition> controllers, int maximumInputs): base(platformId, name, mediastore, metadata)
        {
            this.FileExtensions = fileExtensions;
            this.Defaults = platformDefaults;
            this.controllers = controllers;
        }
       
        public IList<string> FileExtensions { get; private set; }
        public IPlatformDefaults Defaults { get; set; }
        public IReadOnlyDictionary<string, IControllerDefinition> Controllers { get { return this.controllers.AsReadOnly(); } }
        private IDictionary<string, IControllerDefinition> controllers;
        public int MaximumInputs { get; private set; }
        public static IPlatformInfo FromJsonProtoTemplate(IDictionary<string, dynamic> jsonDictionary)
        {
            IPlatformDefaults platformDefaults = jsonDictionary["Defaults"].ToObject<PlatformDefaults>();
            var controllers = new Dictionary<string, IControllerDefinition>();

            foreach (var value in jsonDictionary["Controllers"])
            {
                var inputs = ((JObject)value.Value.ControllerInputs).ToObject<IDictionary<object, JObject>>().ToDictionary(x => (string)x.Key, x => (IControllerInput)x.Value.ToObject<ControllerInput>());
                controllers.Add(value.Name, new ControllerDefinition(inputs, value.Name));
            }
          //  string controllerId =
            return new PlatformInfo(
                    jsonDictionary["PlatformId"],
                    jsonDictionary["Name"],
                    new FileMediaStore(jsonDictionary["MediaStoreKey"]),
                    jsonDictionary["Metadata"].ToObject<Dictionary<string, string>>(),
                    jsonDictionary["FileExtensions"].ToObject<List<string>>(),
                    platformDefaults,
                    controllers,
                    (int)jsonDictionary["MaximumInputs"] 
                );
        }

    }
}
