using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Input.Controller;
using Snowflake.Platform;

namespace Snowflake.Services
{
    public class StoneProvider : IStoneProvider
    {
        public IDictionary<string, IPlatformInfo> Platforms { get; }
        public IDictionary<string, IControllerLayout> Controllers { get; }
        public Version StoneVersion { get; }
        public StoneProvider()
        {
            string stoneData = this.GetStoneData();
            var stone = JsonConvert.DeserializeAnonymousType(stoneData,
                new
                {
                    Controllers = new Dictionary<string, ControllerLayout>(),
                    Platforms = new Dictionary<string, PlatformInfo>(),
                    version = ""
                });
            this.Platforms = stone.Platforms.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as IPlatformInfo);
            this.Controllers = stone.Controllers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value as IControllerLayout);
            this.StoneVersion = Version.Parse(stone.version);

        }

        private string GetStoneData()
        {
            var assembly = typeof(StoneProvider).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.stone.dist.json"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
               
        }
    }
}
