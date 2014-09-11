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

        async void Init()
        {
          //  var test = new FileMediaStore("test");
         //   test.Resources.Add("test", "test.txt");
         //   Console.WriteLine(test.Resources.MediaStoreItems["test"]);
         //   Console.WriteLine(JsonConvert.SerializeObject(test));
              await FrontendCore.InitPluginManagerAsync();
             // Console.WriteLine(FrontendCore.LoadedCore.PluginManager.LoadedIdentifiers.First().Value.IdentifyGame("dummysmb.nes", "NINTENDO_NES"));
              Console.WriteLine(JsonConvert.SerializeObject(FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"].GetScrapeEngine().GetGameInfo("dummysmb.nes")));
            
            this.textBox1.Text = await FrontendCore.LoadedCore.PluginManager.AjaxNamespace.CallMethodAsync(new JSRequest("Core", "Test", new Dictionary<string, string>()));
              //var dbgame = FrontendCore.LoadedCore.GameDatabase.GetGameByUUID("sWJznptYf0m_qH0_OvHtSg");
             // this.textBox1.Text = JsonConvert.SerializeObject(dbgame);
        }
    }
}
