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
using Snowflake.Emulator;
namespace Snowflake.Core.Init
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           FrontendCore.InitCore();
           FrontendCore.InitPluginManager();

            
           var controllerTemplate = ControllerTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.input.NES_CONTROLLER.yml")));
           var inputTemplate = InputTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.input.yml")));
           var controllerDefinition = FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"].Controllers["NES_CONTROLLER"];
           int playerIndex = 1;

           var profile = ControllerProfile.FromDictionary(new Serializer().Deserialize<IDictionary<string, dynamic>>(File.ReadAllText("NES_CONTROLLER.profile.yml")));

        //   Console.WriteLine(new EmulatorBridge().CompileController(1, controllerDefinition, controllerTemplate, profile, inputTemplate));
           var configuration = ConfigurationTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.cfg.yml")));
           var dictionary = new Dictionary<string, dynamic>();
            foreach (var entry in configuration.ConfigurationEntries){
                dictionary.Add(entry.Name, entry.DefaultValue);
            }
            Console.WriteLine(new Serializer().Serialize(dictionary));
           Console.WriteLine(FrontendCore.LoadedCore.EmulatorManager.EmulatorAssemblies["retroarch"].EmulatorName);
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
