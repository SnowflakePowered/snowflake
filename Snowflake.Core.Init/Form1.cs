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
        static FrontendCore fcRef;
        public Form1()
        {
            InitializeComponent();
            Console.SetOut(new MultiTextWriter(new ControlWriter(this.textBox1), Console.Out));

            start();
        }

        void start()
        {
            FrontendCore.InitCore();
            FrontendCore.InitPluginManager();

            var gameUuid = ShortGuid.NewShortGuid();
            //  var homebrew = new GameInfo("NINTENDO_SNES", "SNES_TEST", new FileMediaStore(gameUuid), new Dictionary<string, string>(), gameUuid, "christmascraze.smc");
            //FrontendCore.LoadedCore.GameDatabase.AddGame(homebrew);
            Console.WriteLine(gameUuid);
            var controllerTemplate = ControllerTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.input.NES_CONTROLLER.yml")));
            var inputTemplate = InputTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.input.yml")));
            var configurationTemplate = ConfigurationTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.cfg.yml")));
            var controllerDefinition = FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"].Controllers["NES_CONTROLLER"];
            var configProfiles = ConfigurationProfile.FromManyDictionaries(new Serializer().Deserialize<IList<IDictionary<string, dynamic>>>(File.ReadAllText("retroarch.profile.yml")));
            var controllerProfile = ControllerProfile.FromDictionary(new Serializer().Deserialize<IDictionary<string, dynamic>>(File.ReadAllText("NES_CONTROLLER.profile.yml")));

            var x = new ConfigurationStore(configProfiles.First());
            Console.WriteLine(JsonConvert.SerializeObject(configProfiles.First()))
; var homebrew = new GameInfo("NINTENDO_SNES", "SNES_TEST", new FileMediaStore(gameUuid), new Dictionary<string, string>(), gameUuid, "christmascraze.smc");
            Console.WriteLine(homebrew.CRC32);
            Console.WriteLine(x[homebrew].ConfigurationValues["aspect_ratio_index"]);
            FrontendCore.LoadedCore.ControllerDatabase.AddControllerProfile(controllerProfile, 1);
            //  var _conP = FrontendCore.LoadedCore.ControllerDatabase.GetControllerProfile("NES_CONTROLLER", 1);
            Console.WriteLine(FrontendCore.LoadedCore.ControllerDatabase.GetDeviceName("NES_CONTROLLER", 1));
            FrontendCore.LoadedCore.ControllerPortsDatabase.SetPort(FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"], 1, "NINTENDO_NES");

            Console.WriteLine(FrontendCore.LoadedCore.ControllerPortsDatabase.GetPort(FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"], 1));
            var bridge = new EmulatorBridge(
                new Dictionary<string, ControllerTemplate>(){
                        {"NES_CONTROLLER", controllerTemplate}
                    },
                    new Dictionary<string, InputTemplate>(){
                        {"retroarch", inputTemplate}
                    },
                    new Dictionary<string, ConfigurationTemplate>(){
                        {"retroarch", configurationTemplate}
                    },
                    new List<string>{
                        "NINTENDO_SNES"
                    }
                );
            string keyControl = bridge.CompileController(1, FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"].Controllers["NES_CONTROLLER"], bridge.ControllerTemplates["NES_CONTROLLER"], controllerProfile, bridge.InputTemplates["retroarch"]);
            Console.WriteLine(keyControl);
            // bridge.StartRom(game,controllerProfile);
            int playerIndex = 1;


            //   Console.WriteLine(new EmulatorBridge().CompileController(1, controllerDefinition, controllerTemplate, profile, inputTemplate));
            //  var configuration = ConfigurationTemplate.FromDictionary(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.cfg.yml")));

            //   Console.WriteLine(new EmulatorBridge().CompileConfiguration(configuration, configuProfiles[0]));
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
    //http://stackoverflow.com/questions/18726852/redirecting-console-writeline-to-textbox
    public class ControlWriter : TextWriter
    {
        private Control textbox;
        public ControlWriter(Control textbox)
        {
            this.textbox = textbox;
        }

        public override void Write(char value)
        {
            textbox.Text += Regex.Replace(value.ToString(), "(?<!\r)\n", "\r\n");
        }

        public override void Write(string value)
        {

            textbox.Text += Regex.Replace(value, "(?<!\r)\n", "\r\n");
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
    public class MultiTextWriter : TextWriter
    {
        private IEnumerable<TextWriter> writers;
        public MultiTextWriter(IEnumerable<TextWriter> writers)
        {
            this.writers = writers.ToList();
        }
        public MultiTextWriter(params TextWriter[] writers)
        {
            this.writers = writers;
        }

        public override void Write(char value)
        {
            foreach (var writer in writers)
                writer.Write(value);
        }

        public override void Write(string value)
        {
            foreach (var writer in writers)
                writer.Write(value);
        }

        public override void Flush()
        {
            foreach (var writer in writers)
                writer.Flush();
        }

        public override void Close()
        {
            foreach (var writer in writers)
                writer.Close();
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }

}
