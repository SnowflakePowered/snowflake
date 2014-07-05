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
using Snowflake.UI.Theme;

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
            ThemeServer server = new ThemeServer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "snowflake"));
            server.StartServer();

            var scraper = new Scraper.TheGamesDB.ScraperTheGamesDB();
            Console.WriteLine(scraper.GetGameDetails("2").Item1["SNOWFLAKE_GAME_TITLE"]);

        }
    }
}
