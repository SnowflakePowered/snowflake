using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snowflake.Core;
using Snowflake.Information.Platform;
using Snowflake.Information.Game;
using Snowflake.Plugin.Interface;
using Snowflake.Plugin.Scraper;
using Newtonsoft.Json;
using Snowflake.Constants;
using System.Threading;
using DuoVia.FuzzyStrings;

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

        async Task Init()
        {
            await FrontendCore.InitPluginManagerAsync();
            Platform pnes = FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"];
            IIdentifier id = FrontendCore.LoadedCore.PluginManager.LoadedIdentifiers[pnes.Defaults.Identifier];
            IScraper scraper = FrontendCore.LoadedCore.PluginManager.LoadedScrapers[pnes.Defaults.Scraper];
            string gname = id.IdentifyGame("dummysmb.nes", pnes.PlatformId);
            Console.WriteLine(gname);

            var results = scraper.GetSearchResults(gname, pnes.PlatformId).OrderBy(result => result.GameTitle.LevenshteinDistance(gname)).ToList();
            var resultdetails = scraper.GetGameDetails(results[0].ID);
            var gameinfo = resultdetails.Item1;
            var game = new Game(
                pnes.PlatformId,
                gameinfo[GameInfoFields.snowflake_game_title],
                resultdetails.Item2,
                gameinfo,
                ShortGuid.NewShortGuid(),
                "dummysmb.nes",
                new Dictionary<string, dynamic>()
                );

            Console.WriteLine(game.Metadata[GameInfoFields.snowflake_game_title]);
        }
    }
}
