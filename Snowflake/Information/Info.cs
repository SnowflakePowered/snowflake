using System.Collections.Generic;

namespace Snowflake.Information
{
    public class Info: IInfo
    {
        public string PlatformID { get; }
        public string Name { get; }
        public IDictionary<string, string> Metadata { get; set; }

        public Info(string platformId, string name, IDictionary<string,string> metadata)
        {
            this.PlatformID = platformId;
            this.Name = name;
            this.Metadata = metadata;
        }


    }
}
