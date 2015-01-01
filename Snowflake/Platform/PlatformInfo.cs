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
        public static IPlatformInfo FromDictionary(IDictionary<string, dynamic> jsonDictionary)
        {
            IPlatformDefaults platformDefaults = jsonDictionary["Defaults"].ToObject<PlatformDefaults>();
          //  string controllerId =
            return new PlatformInfo(
                    jsonDictionary["PlatformId"],
                    jsonDictionary["Name"],
                    new FileMediaStore(jsonDictionary["MediaStoreKey"]),
                    jsonDictionary["Metadata"].ToObject<Dictionary<string, string>>(),
                    jsonDictionary["FileExtensions"].ToObject<List<string>>(),
                    platformDefaults,
                    jsonDictionary["Controllers"].ToObject<Dictionary<string, IControllerDefinition>>(),
                    (int)jsonDictionary["MaximumInputs"] 
                );
        }

    }
}
