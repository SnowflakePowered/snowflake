using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.MediaStore;
using Snowflake.Information;
using Snowflake.Platform.Controller;
using System.Collections.ObjectModel;
using Snowflake.Extensions;

namespace Snowflake.Platform
{
    public class PlatformInfo : Info
    {
        public PlatformInfo(string platformId, string name, IMediaStore mediastore, IDictionary<string, string> metadata, IList<string> fileExtensions, PlatformDefaults platformDefaults, IDictionary<string, ControllerDefinition> controllers, int maximumInputs): base(platformId, name, mediastore, metadata)
        {
            this.FileExtensions = fileExtensions;
            this.Defaults = platformDefaults;
            this.controllers = controllers;
        }
       
        public IList<string> FileExtensions { get; private set; }
        public PlatformDefaults Defaults { get; set; }
        public IReadOnlyDictionary<string, ControllerDefinition> Controllers { get { return this.controllers.AsReadOnly(); } }
        private IDictionary<string, ControllerDefinition> controllers;
        public int MaximumInputs { get; private set; }
        public static PlatformInfo FromDictionary(IDictionary<string, dynamic> jsonDictionary)
        {
         
            return new PlatformInfo(
                    jsonDictionary["PlatformId"],
                    jsonDictionary["Name"],
                    new FileMediaStore(jsonDictionary["MediaStoreKey"]),
                    jsonDictionary["Metadata"].ToObject<Dictionary<string, string>>(),
                    jsonDictionary["FileExtensions"].ToObject<List<string>>(),
                    jsonDictionary["Defaults"].ToObject<PlatformDefaults>(),
                    jsonDictionary["Controllers"].ToObject<Dictionary<string, ControllerDefinition>>(),
                    (int)jsonDictionary["MaximumInputs"] 
                );
        }

    }
}
