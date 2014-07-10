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
using Snowflake.API.Information.Platform;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Snowflake.Core;
using Snowflake.API.Interface;

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
            this.AllowDrop = true;
            FrontendCore.InitCore();
            var x = FrontendCore.LoadedCore;           
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

               

                var defaults = FrontendCore.LoadedCore.LoadedPlatforms["NINTENDO_NES"].Defaults;
              
                IScraper scraper = FrontendCore.LoadedCore.PluginManager.LoadedScrapers[defaults.Scraper];
                string name = FrontendCore.LoadedCore.PluginManager.LoadedIdentifiers[defaults.Identifier].IdentifyGame(files[0], "NINTENDO_NES");
                var info = scraper.GetSearchResults(name);
                var xinfo = scraper.GetGameDetails(info[0].ID);
                
                MessageBox.Show(xinfo.Item1[GameInfoFields.snowflake_game_description], name);

            }
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.All;
            else
                e.Effects = DragDropEffects.None;
        }
    }
}
