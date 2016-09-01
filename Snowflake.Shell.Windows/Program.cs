using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Newtonsoft.Json;
using Snowflake.Configuration;
using Snowflake.Emulator;
using Snowflake.Events;
using Snowflake.Service;
using Snowflake.Service.Manager;
using Squirrel;

namespace Snowflake.Shell.Windows
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var snowflakeIcon = new ShellIcon();
            SnowflakeEventManager.InitEventSource();
            var snowflakeShell = new SnowflakeShell();

            
            snowflakeShell.StartShell();

            snowflakeIcon.AddMenuItem("Quit Snowflake", Program.menuQuitHandler);
            snowflakeIcon.AddMenuItem("Shutdown Core", (s, e) =>
            {
                snowflakeShell.ShutdownCore();
            });
            snowflakeIcon.AddMenuItem("Restart Core", (s, e) =>
            {
                snowflakeShell.RestartCore();
            });
            Application.Run();
        }

        static void menuQuitHandler(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
