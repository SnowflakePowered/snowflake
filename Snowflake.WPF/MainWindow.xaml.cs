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
using System.Windows.Shapes;
using Snowflake.Core;
using CefSharp.Wpf;
namespace Snowflake.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrontendCore.InitCore();
            FrontendCore.LoadedCore.CoreLoaded += (s, e) => { MessageBox.Show("loaded"); };
            WebView view = new WebView();
            view.Address = "http://google.ca";
            this.MainControl.Children.Add(view);

        }
    }
}
