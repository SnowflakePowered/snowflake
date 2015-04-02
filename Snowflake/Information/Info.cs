using System.Collections.Generic;
using Snowflake.Information.MediaStore;

namespace Snowflake.Information
{
    public class Info: IInfo
    {
        public string PlatformID { get; private set; }
        public string Name { get; private set; }
        public IDictionary<string, string> Metadata { get; set; }

        public Info(string platformId, string name, IDictionary<string,string> metadata)
        {
            this.PlatformID = platformId;
            this.Name = name;
            this.Metadata = metadata;
        }


    }
}
