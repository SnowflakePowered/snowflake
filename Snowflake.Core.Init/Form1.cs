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

        public static PlatformInfo FromDictionary(IDictionary<string, dynamic> jsonDictionary)
        {
            return new PlatformInfo(
                    json["PlatformId"],
                    json["Name"],
                    new FileMediaStore(json["MediaStoreKey"]),
                    json["Metadata"].ToObject<Dictionary<string, string>>(),
                    json["FileExtensions"].ToObject<List<string>>(),
                    json["Defaults"].ToObject<PlatformDefaults>()
                );
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
            
                var x = Form1.FromDictionary(JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(@"
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
}"));
            Console.WriteLine(x.Name);
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
