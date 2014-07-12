using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Interface.Core;
using Snowflake.Core;
using Snowflake.WPF;
using System.Windows;
namespace Snowflake
{
    class Program : Application
    {
        [STAThread]
        static void Main(string[] args)
        {
            App app = new App();
            app.Run();
            
        }
        public void RunX()
        {
            
        }
    }
}
