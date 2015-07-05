﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snowflake.Ajax;
using Snowflake.Service;
using Snowflake.Platform;
using Snowflake.Game;
using Newtonsoft.Json;
using Snowflake.Constants;
using System.Threading;
using Snowflake.Extensions;
using Snowflake.Information.MediaStore;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using Snowflake.Emulator.Configuration;
using SharpYaml.Serialization;
using System.Text.RegularExpressions;
using System.Dynamic;
using Snowflake.Controller;
using Snowflake.Emulator.Input;
using Snowflake.Emulator.Input.Constants;
using Snowflake.Emulator;
using Snowflake.Utility;
using Snowflake.Service.JSWebSocketServer;
using Snowflake.Service.HttpServer;
using Fleck;
namespace Snowflake.Service.Init
{
    public partial class Form1 : Form
    {
        static CoreService fcRef;
        public Form1()
        {
            InitializeComponent();
            //Console.SetOut(new MultiTextWriter(new ControlWriter(this.textBox1, this), Console.Out));
            start();

 string x_ = @"
[
    {
        key: 'fullscreen_toggle',
        type: 'SELECT_FLAG',
        description: 'Toggles Fullscreen',
        default: 'fullscreen_on',
        max: 10,
        min: 0,
        values: [
            {
                value: 'fullscreen_on',
                description: 'Turn on fullscreen'
            },
            {
                value: 'fullscreen_off',
                description: 'Turn off fullscreen'
            }
        ]
    }
]

";

            var x = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(x_);
            Console.WriteLine(JsonConvert.SerializeObject(            ConfigurationFlag.FromJsonProtoTemplate(x.First())
));
        }

        void start()
        {
            CoreService.InitCore();
            CoreService.InitPluginManager();
            Snowflake.Events.SnowflakeEventSource.EventSource.AjaxRequestReceived += (s, e) =>
            {
                textBox1.BeginInvoke((Action)(() =>
                {

                    textBox1.Text += Regex.Replace(
                    "Received Request " +
                    e.ReceivedRequest.MethodName +
                    " " +
                    JsonConvert.SerializeObject(e.ReceivedRequest.MethodParameters) + 
                    Environment.NewLine
                    , "(?<!\r)\n", "\r\n");
                }));
            };
            Snowflake.Events.SnowflakeEventSource.EventSource.AjaxResponseSending += (s, e) =>
            {
                textBox1.BeginInvoke((Action)(() =>
                {

                    textBox1.Text += Regex.Replace(
                    "Sending Response " +
                    e.SendingResponse.GetJson() +
                    Environment.NewLine
                    , "(?<!\r)\n", "\r\n");
                }));
            };
         //   var x = CoreService.LoadedCore.ControllerPortsDatabase.GetDeviceInPort(CoreService.LoadedCore.LoadedPlatforms.First().Value, 1);
       /*     var homebrew = new GameInfo("NINTENDO_SNES", "SNES_TEST", new FileMediaStore(gameUuid), new Dictionary<string, string>(), gameUuid, "christmascraze.smc");
            CoreService.LoadedCore.GameDatabase.AddGame(homebrew);*/

//            var game = CoreService.LoadedCore.GameDatabase.GetGameByUUID(gameUuid);

           // CoreService.LoadedCore.PluginManager.LoadedEmulators["SnowflakeRA-bSNES-compatibility"].StartRom(game);
//            var emu = CoreService.LoadedCore.PluginManager.LoadedEmulators["SnowflakeRA-bSNES-compatibility"];
  //          emu.StartRom(game);
           // CoreService.LoadedCore.PluginManager.LoadedEmulators.First().Value.StartRom(game);


            //var x = CoreService.LoadedCore.PluginManager.LoadedScrapers["Scraper.TheGamesDB"].GetSearchResults("Super Mario World");
      /*     
            // ;
            Console.WriteLine(gameUuid);
            var controllerTemplate = ControllerTemplate.FromJsonProtoTemplate(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.input.NES_CONTROLLER.yml")));
            var inputTemplate = InputTemplate.FromJsonProtoTemplate(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.input.yml")));
            var configurationTemplate = ConfigurationTemplate.FromJsonProtoTemplate(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.cfg.yml")));
            var controllerDefinition = CoreService.LoadedCore.LoadedPlatforms["NINTENDO_NES"].Controllers["NES_CONTROLLER"];
            var configProfiles = ConfigurationProfile.FromManyDictionaries(new Serializer().Deserialize<IList<IDictionary<string, dynamic>>>(File.ReadAllText("retroarch.profile.yml")));
            var controllerProfile = ControllerProfile.FromJsonProtoTemplate(new Serializer().Deserialize<IDictionary<string, dynamic>>(File.ReadAllText("NES_CONTROLLER.profile.yml")));

            var x = new ConfigurationStore(configProfiles.First());
            Console.WriteLine(JsonConvert.SerializeObject(configProfiles.First()))
; var homebrew = new GameInfo("NINTENDO_SNES", "SNES_TEST", new FileMediaStore(gameUuid), new Dictionary<string, string>(), gameUuid, "christmascraze.smc");
            Console.WriteLine(homebrew.CRC32);
            Console.WriteLine(x[homebrew].ConfigurationValues["aspect_ratio_index"]);
            CoreService.LoadedCore.ControllerDatabase.AddControllerProfile(controllerProfile, 1);
            //  var _conP = FrontendCore.LoadedCore.ControllerDatabase.GetControllerProfile("NES_CONTROLLER", 1);
            Console.WriteLine(CoreService.LoadedCore.ControllerDatabase.GetDeviceName("NES_CONTROLLER", 1));
            CoreService.LoadedCore.ControllerPortsDatabase.SetPort(CoreService.LoadedCore.LoadedPlatforms["NINTENDO_NES"], 1, "NINTENDO_NES");

            Console.WriteLine(CoreService.LoadedCore.ControllerPortsDatabase.GetPort(CoreService.LoadedCore.LoadedPlatforms["NINTENDO_NES"], 1));
           /* var bridge = new EmulatorBridge(
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
                );*/
            //string keyControl = bridge.CompileController(1, CoreService.LoadedCore.LoadedPlatforms["NINTENDO_NES"].Controllers["NES_CONTROLLER"], bridge.ControllerTemplates["NES_CONTROLLER"], controllerProfile, bridge.InputTemplates["retroarch"]);
          //  Console.WriteLine(keyControl);
            // bridge.StartRom(game,controllerProfile);
            int playerIndex = 1;

            //   Console.WriteLine(new EmulatorBridge().CompileController(1, controllerDefinition, controllerTemplate, profile, inputTemplate));
            //  var configuration = ConfigurationTemplate.FromJsonProtoTemplate(new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText("retroarch.cfg.yml")));

            //   Console.WriteLine(new EmulatorBridge().CompileConfiguration(configuration, configuProfiles[0]));
            //FrontendCore.LoadedCore.ConfigurationFlagDatabase.CreateFlagsTable("");

            /*string flags = File.ReadAllText("emulatorflags.json");
            var flagsobj = JsonConvert.DeserializeObject<IList<IDictionary<string, dynamic>>>(flags);
            var cflag = ConfigurationFlag.FromManyDictionaries(flagsobj);*/
          //  CoreService.LoadedCore.ConfigurationFlagDatabase.CreateFlagsTable("test", cflag);
        //    FrontendCore.LoadedCore.ConfigurationFlagDatabase.AddGame(homebrew, "test", cflag, new Dictionary<string, string>());
       //     var val =  (int)CoreService.LoadedCore.ConfigurationFlagDatabase.GetValue(homebrew, "test", "someint", ConfigurationFlagTypes.INTEGER_FLAG);
          ///  Console.WriteLine(val);
            
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

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
    //http://stackoverflow.com/questions/18726852/redirecting-console-writeline-to-textbox
    public class ControlWriter : TextWriter
    {
        private Control textbox;
        private Form form;
        public ControlWriter(Control textbox, Form form)
        {
            this.textbox = textbox;
            this.form = form;
        }

        public override void Write(char value)
        {
            try
            {
                form.BeginInvoke((Action)(() =>
                {
                    textbox.Text += Regex.Replace(value.ToString(), "(?<!\r)\n", "\r\n");
                }));
            }
            catch (InvalidOperationException)
            {
                textbox.Text += Regex.Replace(value.ToString(), "(?<!\r)\n", "\r\n");
            }
           
          
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
