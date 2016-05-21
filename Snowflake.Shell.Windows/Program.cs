using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Snowflake.Configuration;
using Snowflake.Events;
using Snowflake.Service;
using Snowflake.Service.Manager;
using Snowflake.Utility;
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
         /*   using (var mgr = new UpdateManager(@"C:\squirrel\snowflake\rel"))
            {
                SquirrelAwareApp.HandleEvents(
                onInitialInstall: v => mgr.CreateShortcutForThisExe(),
                onAppUpdate: v => mgr.CreateShortcutForThisExe(),
                onAppUninstall: v => mgr.RemoveShortcutForThisExe(),
                onFirstRun: () =>
                {
                    Process.Start("snowball.exe install builtins.snowball -l"); //todo call snowball from dll
                });

            }*/

            Stopwatch sw = new Stopwatch();
            sw.Start();
            FileHash.GetCRC32(@"E:\ROMs\Playstation Portable\Hatsune Miku Project Diva.ISO");
            sw.Stop();
            var ts = sw.Elapsed;
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
            Console.WriteLine(@"RunTime " + elapsedTime);
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
