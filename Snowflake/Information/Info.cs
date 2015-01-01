using System.Collections.Generic;
using Snowflake.Information.MediaStore;

namespace Snowflake.Information
{
    public class Info: IInfo
    {
        public string PlatformId { get; private set; }
        public string Name { get; private set; }
        public IMediaStore MediaStore { get; private set; }
        public IDictionary<string, string> Metadata { get; set; }

        public Info(string platformId, string name, IMediaStore mediastore, IDictionary<string,string> metadata)
        {
            this.PlatformId = platformId;
            this.Name = name;
            this.MediaStore = mediastore;
            this.Metadata = metadata;
        }


    }
}
