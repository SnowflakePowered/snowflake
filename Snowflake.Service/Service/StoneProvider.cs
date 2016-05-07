using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Platform;

namespace Snowflake.Service
{
    public class StoneProvider : IStoneProvider
    {
        public IDictionary<string, IPlatformInfo> Platforms { get; }
        public Version StoneVersion { get; }
        public StoneProvider()
        {
            string stoneData = this.GetStoneData();
            var stone = JsonConvert.DeserializeAnonymousType(stoneData,
                new
                {
                    Platforms = new Dictionary<string, PlatformInfo>(),
                    version = ""
                });
            this.Platforms = stone.Platforms.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as IPlatformInfo);
            this.StoneVersion = Version.Parse(stone.version);

        }

        private string GetStoneData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.stone.dist.json"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
               
        }
    }
}
