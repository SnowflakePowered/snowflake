using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snowflake.Ajax;
using Snowflake.Core;
using Snowflake.Information.Platform;
using Snowflake.Information.Game;
using Newtonsoft.Json;
using Snowflake.Constants;
using System.Threading;
using Snowflake.Extensions;
using DuoVia.FuzzyStrings;
using Snowflake.Information.MediaStore;
using Newtonsoft.Json.Linq;

namespace Snowflake.Core.Init
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FrontendCore.InitCore();
            Init();
        }

        void FromJson(Dictionary<string, dynamic> json)
        {
            string platformID = json["PlatformId"];
            string platformName = json["Name"];
            string mediaStoreKey = json["MediaStoreKey"];
            var _metadata = json["Metadata"];
            var platformMetadata = new Dictionary<string, string>();
            foreach (JProperty metadata in _metadata)
            {
                platformMetadata.Add(metadata.Name, metadata.Value.ToString());
            }
        }
        void Init()
        {
            Console.WriteLine(
                JsonConvert.SerializeObject(
                    new PlatformInfo(
                        "NINTENDO_NES",
                        "Nintendo Entertainment System",
                        new FileMediaStore("platform.NINTENDO_NES"),
                        new Dictionary<string, string>()
                            {
                                {"platform_shortname", "NES"},
                                {"platform_company", "Nintendo"},
                                {"platform_release", "18/10/1985"}
                            },
                        new List<string>()
                           {
                                ".nes"
                           },
                           new PlatformDefaults("Scraper.TheGamesDB", "Identifier.ClrMameProDat", "Emulator.RetroArch")
                        )
                    )
                );
            var x =
                JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(@"
                {
    'FileExtensions': [
        '.nes'
    ],
    'Defaults': {
        'Scraper': 'Scraper.TheGamesDB',
        'Identifier': 'Identifier.ClrMameProDat',
        'Emulator': 'Emulator.RetroArch'
    },
    'PlatformId': 'NINTENDO_NES',
    'Name': 'Nintendo Entertainment System',
    'MediaStoreKey': 'platform.NINTENDO_NES',
    'Metadata': {
        'platform_shortname': 'NES',
        'platform_company': 'Nintendo',
        'platform_release': '18/10/1985'
    }
}");
            foreach(JProperty xitem in x["Metadata"]){
                Console.WriteLine(xitem.Name);
                Console.WriteLine(xitem.Value);
                Console.WriteLine(x["Metadata"].GetType());
            }
          //  var test = new FileMediaStore("test");
         //   test.Resources.Add("test", "test.txt");
         //   Console.WriteLine(test.Resources.MediaStoreItems["test"]);
         //   Console.WriteLine(JsonConvert.SerializeObject(test));
           ///   await FrontendCore.InitPluginManagerAsync();
             // Console.WriteLine(FrontendCore.LoadedCore.PluginManager.LoadedIdentifiers.First().Value.IdentifyGame("dummysmb.nes", "NINTENDO_NES"));
        //      Console.WriteLine(JsonConvert.SerializeObject(FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"].GetScrapeEngine().GetGameInfo("dummysmb.nes")));
            
        //    this.textBox1.Text = await FrontendCore.LoadedCore.PluginManager.AjaxNamespace.CallMethodAsync(new JSRequest("Core", "Test", new Dictionary<string, string>()));
              //var dbgame = FrontendCore.LoadedCore.GameDatabase.GetGameByUUID("sWJznptYf0m_qH0_OvHtSg");
             // this.textBox1.Text = JsonConvert.SerializeObject(dbgame);
        }
    }
}
