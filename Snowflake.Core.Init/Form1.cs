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
using System.IO;
using System.Diagnostics;
using Snowflake.Emulator.Configuration.Template;
using SharpYaml.Serialization;
using Snowflake.Emulator.Configuration;
using System.Text.RegularExpressions;
using System.Dynamic;
namespace Snowflake.Core.Init
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            FrontendCore.InitCore();
            //      Init();
            // var manager = new Core.Manager.EmulatorManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emulators"));
            //manager.LoadEmulatorCores();
            //Process.Start(manager.GetExecutableEmulatorProcess(manager.EmulatorCores["retroarch"]));

           /* string s = File.ReadAllText("c.yml");
             var x = ConfigurationTemplate.FromYaml(s);
             var xx = new Dictionary<string, dynamic>()
                 {
                     {"video_driver", "gl"},
                     {"video_rotation", 0}
                 };
             var xi = new EmulatorConfiguration(x, xx);
           
             Console.WriteLine(xi.Compile());
             Console.WriteLine(JsonConvert.SerializeObject(xi));*/

            foreach (string line in File.ReadAllLines("retroarch.cfg"))
            {
                var key = Regex.Match(line, @".+?(?=\s)").Value;
                var value = Regex.Match(line, "\"([^\"]*)\"").Value.Replace("\"", "");
                Console.WriteLine(line.Replace(value, "{" + key + "}"));
            }

            /*   var xix = new Dictionary<string, Dictionary<string, dynamic>>();
               foreach (string line in File.ReadAllLines("retroarch.cfg"))
               {
                   var key = Regex.Match(line, @".+?(?=\s)").Value;
                   var value = Regex.Match(line, "\"([^\"]*)\"").Value.Replace("\"", "");
                   dynamic _value;
                   int x;
                   string type;
                   if (value == "true" || value == "false")
                   {
                       type = "boolean";
                       _value = Boolean.Parse(value);
                   }
                   else
                   {
                       if (value.Contains("."))
                       {
                           type = "double";
                           _value = Double.Parse(value);
                       }
                       else
                       {
                           if (Int32.TryParse(value, out x))
                           {
                               type = "integer";
                               _value = x;
                           }
                           else
                           {
                               type = "~custom";
                               _value = value;
                           }
                       }
                   }

                   var dx = new Dictionary<string, dynamic>();
                   dx["description"] = "desc";
                   dx["type"] = type;
                   dx["protection"] = "public";
                   dx["default"] = _value;
                   xix.Add(key, dx);

               }
               var s = new Serializer()
                  {
                      Settings =
                      {
                          EmitTags = false,
                      },
                  };

               Console.WriteLine(s.Serialize(xix));
               Console.WriteLine(JsonConvert.SerializeObject(xix));
           }*/
        }



        async void Init()
        {
            //    await FrontendCore.InitPluginManagerAsync();
            //   Console.WriteLine(FrontendCore.LoadedCore.AjaxManager.Registry.Keys.ToArray()[0]);
            //    // Console.WriteLine(FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"].MediaStore.MediaStoreKey);
            //  Console.WriteLine(FrontendCore.LoadedCore.PluginManager.LoadedIdentifiers.First().Value.IdentifyGame("dummysmb.nes", "NINTENDO_NES"));
            //        var test = FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"].GetScrapeEngine().GetGameInfo("dummysmb.nes");
            //Console.WriteLine(test.UUID);
            //   var x = FrontendCore.LoadedCore.GameDatabase.GetGameByUUID("klGNT8l9m0ypDI8IGr0TcA");
            //   var ms = x.MediaStore.Images["boxart_front"];
            //  Console.WriteLine(JsonConvert.SerializeObject(x.MediaStore.Images));

        }
    }
}
