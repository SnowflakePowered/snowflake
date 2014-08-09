using System.Collections.Generic;

namespace Snowflake.Information
{
    public class Info: IInfo
    {
        public string PlatformId { get; private set; }
        public string Name { get; private set; }
        public IDictionary<string, string> Images { get; set; }
        public IDictionary<string, string> Metadata { get; set; }

        public Info(string platformId, string name, IDictionary<string,string> images, IDictionary<string,string> metadata)
        {
            this.PlatformId = platformId;
            this.Name = name;
            this.Images = images;
            this.Metadata = metadata;
        }


    }
}
