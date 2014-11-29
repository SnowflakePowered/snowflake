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
using Snowflake.Platform;
using Snowflake.Game;
using Newtonsoft.Json;
using Snowflake.Constants;
using System.Threading;
using Snowflake.Extensions;
using DuoVia.FuzzyStrings;
using Snowflake.MediaStore;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using Snowflake.Emulator.Configuration.Template;
using Snowflake.Emulator.Configuration.Mapping;
using SharpYaml.Serialization;
using Snowflake.Emulator.Configuration;
using System.Text.RegularExpressions;
using System.Dynamic;
using Snowflake.Platform.Controller;
using Snowflake.Constants.Input;
using Snowflake.Emulator.Input.Template;
namespace Snowflake.Core.Init
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           FrontendCore.InitCore();

            var controllerMapping = ControllerTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.input.NES_CONTROLLER.yml")));
            var inputMapping = InputTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.input.yml")));
            var controller = FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"].Controllers["NES_CONTROLLER"];

            foreach()

           // Console.WriteLine(FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_SNES"].Controllers["SNES_CONTROLLER"].ControllerID);
         /*   var x = new Dictionary<string, ControllerInput>(){
                {"BTN_A", new ControllerInput("BTN_A", KeyboardConstants.KEY_Z, GamepadConstants.GAMEPAD_A)},
                {"BTN_B", new ControllerInput("BTN_B", KeyboardConstants.KEY_X, GamepadConstants.GAMEPAD_B)},
                {"BTN_START", new ControllerInput("BTN_A", KeyboardConstants.KEY_SPACEBAR, GamepadConstants.GAMEPAD_START)},
                {"BTN_SELECT", new ControllerInput("BTN_A", KeyboardConstants.KEY_ENTER, GamepadConstants.GAMEPAD_SELECT)},
                {"BTN_DPAD_UP", new ControllerInput("BTN_A", KeyboardConstants.KEY_UP, GamepadConstants.GAMEPAD_DPAD_UP)},
                {"BTN_DPAD_DOWN", new ControllerInput("BTN_A", KeyboardConstants.KEY_DOWN, GamepadConstants.GAMEPAD_DPAD_DOWN)},
                {"BTN_DPAD_LEFT", new ControllerInput("BTN_A", KeyboardConstants.KEY_LEFT, GamepadConstants.GAMEPAD_DPAD_LEFT)},
                {"BTN_DPAD_RIGHT", new ControllerInput("BTN_A", KeyboardConstants.KEY_RIGHT, GamepadConstants.GAMEPAD_DPAD_RIGHT)}
            };*/
       //     var y = new ControllerDefinition(x, "NES_CONTROLLER");
    //        Console.WriteLine(JsonConvert.SerializeObject(y));
            //      Init();
            // var manager = new Core.Manager.EmulatorManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emulators"));
            //manager.LoadEmulatorCores();
            //Process.Start(manager.GetExecutableEmulatorProcess(manager.EmulatorCores["retroarch"]));

           /*   string s = File.ReadAllText("c.yml");
              var x = ConfigurationTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(s));
            
              var xx = new Dictionary<string, dynamic>()
                  {
                      {"video_driver", "gl"},
                      {"video_rotation", 0}
                  };
              var xi = new EmulatorConfiguration(x, xx);
           
              Console.WriteLine(xi.Compile());
              Console.WriteLine(JsonConvert.SerializeObject(xi));*/

            /*    foreach (string line in File.ReadAllLines("retroarch.cfg"))
                {
                    var key = Regex.Match(line, @".+?(?=\s)").Value;
                    var value = Regex.Match(line, "\"([^\"]*)\"").Value.Replace("\"", "");
                    Console.WriteLine(line.Replace(value, "{" + key + "}"));
                }*/

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
