﻿using System;
using System.Diagnostics;
using System.Linq;
using Snowflake.Bootstrap.Windows.Utility;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Orchestration.Process;
using Snowflake.Services;
using Snowflake.Services.AssemblyLoader;

namespace Snowflake.Shell.Windows
{
    internal class SnowflakeShell
    {
        private readonly string appDataDirectory =
            PathUtility.GetApplicationDataPath().CreateSubdirectory("snowflake").FullName;

        private IServiceContainer loadedCore;

        internal SnowflakeShell()
        {
            this.StartCore();
        }

        public void StartCore()
        {
            this.loadedCore = new ServiceContainer(this.appDataDirectory);
            var loader = this.loadedCore.Get<IModuleEnumerator>();
            var composer = new AssemblyComposer(this.loadedCore, loader);
            composer.Compose();
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
