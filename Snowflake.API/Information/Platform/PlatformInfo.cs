using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Information.MediaStore;

namespace Snowflake.Information.Platform
{
    public class PlatformInfo : Info
    {
        public PlatformInfo(string platformId, string name, IMediaStore mediastore, IDictionary<string, string> metadata, IList<string> fileExtensions, PlatformDefaults platformDefaults): base(platformId, name, mediastore, metadata)
        {
            this.FileExtensions = fileExtensions;
            this.Defaults = platformDefaults;
        }
       
        public IList<string> FileExtensions { get; private set; }
        public PlatformDefaults Defaults { get; set; }

        public static PlatformInfo FromDictionary(IDictionary<string, dynamic> jsonDictionary)
        {
            return new PlatformInfo(
                    jsonDictionary["PlatformId"],
                    jsonDictionary["Name"],
                    new FileMediaStore(jsonDictionary["MediaStoreKey"]),
                    jsonDictionary["Metadata"].ToObject<Dictionary<string, string>>(),
                    jsonDictionary["FileExtensions"].ToObject<List<string>>(),
                    jsonDictionary["Defaults"].ToObject<PlatformDefaults>()
                );
        }

    }
}
