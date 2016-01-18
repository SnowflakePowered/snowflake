using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Events;
using Snowflake.Events.ServiceEvents;
namespace Snowflake.Shell.Windows.ElectronUserInterface
{
    internal class ElectronInterface : IUserInterface
    {

        private readonly ProcessStartInfo electronProcess;
        private readonly ICoreService coreInstance;
        private Process currentlyRunningProcess;
        private readonly string electronPath;

        public ElectronInterface(ICoreService coreInstance, string electronPath)
        {
            this.electronProcess = new ProcessStartInfo();
            this.coreInstance = coreInstance;
            this.electronPath = electronPath;
            this.electronProcess.FileName = Path.Combine(this.electronPath, "electron.exe");
        }

        public void StartUserInterface(params string[] args)
        {
            this.electronProcess.Arguments = $"{Path.Combine(this.electronPath, "bootstrap")} {String.Join(" ", args)}";
            var uiStartEvent = new UIStartEventArgs(this.coreInstance, this);
            SnowflakeEventManager.EventSource.RaiseEvent(uiStartEvent);
            if (uiStartEvent.Cancel) return;
            this.currentlyRunningProcess = Process.Start(this.electronProcess);
        }

        public void StopUserInterface(params string[] args)
        {
            var uiStopEvent = new UIStopEventArgs(this.coreInstance, this);
            SnowflakeEventManager.EventSource.RaiseEvent(uiStopEvent);
            if (uiStopEvent.Cancel) return;
            this.currentlyRunningProcess.Close();
            this.currentlyRunningProcess = null;
        }
    }
}
