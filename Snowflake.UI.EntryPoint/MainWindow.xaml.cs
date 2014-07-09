using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Snowflake.API;
using Snowflake.API.Constants;
using Snowflake.API.Constants.Plugin;
using System.Diagnostics;
using Snowflake.API.Configuration;
using Snowflake.API.Information;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Snowflake.UI.EntryPoint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            var x = new Snowflake.Core.FrontendCore();
            Console.WriteLine(x.LoadedPlatforms["NINTENDO_NES"].RomIdentifierPlugin);
            //Platform p = new Platform("NINTENDO_NES", "Nintendo Entertainment System", new Dictionary<string,string>(), new Dictionary<string,string>(), new List<string>(), "Snowflake-IdentifierDat");
            //p.Images.Add("snowflake_img_platform_logo", "logo.png");
            //p.FileExtensions.Add(".nes");
            //p.Metadata.Add("snowflake_platform_shortname", "NES");
            //p.Metadata.Add("snowflake_platform_company", "Nintendo");
            //Console.WriteLine(JsonConvert.SerializeObject(p));
            //var id = new Identifier.DatIdentifier.DatIdentifier();
            //Console.WriteLine(id.IdentifyGame("sadsah.sfc", "NINTENDO_SNES"));
           // var fileName = @"[abc][def]Real Name[!].exe";
           // Console.WriteLine(Regex.Match(fileName, @"(\[[^]]*\])*([\w\s]+)").Groups[2].Value);

            //var resultString = Regex.Match(test, @"(?<=rom \( name "").*?(?="" size \d+ crc 2e2bf112)").Value;
           // Console.WriteLine(resultString);
           //var uuid = "ssss";
            //            string query = @"SELECT * FROM `games` WHERE `uuid` == """ + uuid + @"""";

//                        Console.WriteLine(query);
           
 //           var x = new GameScrapeEngine();
  //          Console.WriteLine(x.CalculateCRC32(new FileStream("SuperMarioBros.nes", FileMode.Open)));
            //var scraper = new Scraper.TheGamesDB.ScraperTheGamesDB();
            //var results = scraper.GetSearchResults("Super Mario World", "NINTENDO_SNES");   
            // Console.WriteLine(scraper.GetGameDetails(results[0].ID).Item2.Boxarts[ImagesInfoFields.snowflake_img_boxart_back][0]);
            //Console.WriteLine(scraper.PluginInfo["authors"][0]);
           // var retroarch = new Emulator.RetroArch.EmulatorRetroArch();
           // Console.WriteLine(retroarch.MainExecutable);
           // var proc = retroarch.GetProcessStartInfo("NINTENDO_NES","");
           // //Console.WriteLine(proc.FileName);
           //s Console.WriteLine(proc.Arguments);
            //Process.Start(proc);
            //var s = new Snowflake.API.Configuration.YamlConfiguration("hh");
            //s.LoadConfiguration();
        }
    }
}
