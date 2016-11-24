using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using Snowflake.Configuration;
using Snowflake.Emulator;

using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Scraper;
using Snowflake.Services;
using Snowflake.Utility;

namespace Snowflake.Shell.Linux
{
    internal class SnowflakeShell
    {
        private ICoreService loadedCore;
        private readonly string appDataDirectory = PathUtility.GetSnowflakeDataPath();
        internal SnowflakeShell()
        {
            if(!Directory.Exists(appDataDirectory)) Directory.CreateDirectory(appDataDirectory);
            Console.WriteLine(this.appDataDirectory);
            this.StartCore();
        }
       
        public void StartCore()
        {

            this.loadedCore = new CoreService(this.appDataDirectory);
            this.loadedCore.Get<IPluginManager>()?.Initialize();
          
        }

        public void StartShell() {
            var electronUi = this.loadedCore.Get<IUserInterface>();
            //electronUi.StartUserInterface();
        }

        public void RestartCore()
        {
            this.ShutdownCore();
            this.StartCore();
        }

        public void ShutdownCore()
        {
            this.loadedCore.Dispose();
            GC.WaitForPendingFinalizers();
        }  
    }
}
