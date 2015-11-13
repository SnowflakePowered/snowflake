using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;

namespace Snowflake.Emulator
{
    /// <summary>
    /// Represents an instance of an external emulator process
    /// </summary>
    public class ProcessEmulatorInstance : IEmulatorInstance
    {

        public Process runningEmulatorProcess;
        public ProcessStartInfo ProcessInfo { get; set; }
        public ProcessEmulatorInstance(IGameInfo gameInfo, IEmulatorBridge emulatorBridge)
        {
            this.InstanceGame = gameInfo;
            this.InstanceEmulator = emulatorBridge;
            this.InstanceState = EmulatorInstanceState.InstanceNotStarted;
            this.InstanceId = $"{this.InstanceGame.UUID}_{Guid.NewGuid()}";
            this.InstanceTemporaryDirectory = Path.Combine(Path.GetTempPath(), "snowflake", this.InstanceId);
            Directory.CreateDirectory(this.InstanceTemporaryDirectory);
        }
        public EmulatorInstanceState InstanceState { get; protected set; }
        public IGameInfo InstanceGame { get; }
        public string InstanceTemporaryDirectory { get; }
        public IEmulatorBridge InstanceEmulator { get; }
        public string InstanceId { get; }
        public virtual EmulatorInstanceState StartGame()
        {
            try
            {
                this.runningEmulatorProcess = Process.Start(this.ProcessInfo);
                this.InstanceState = EmulatorInstanceState.GameRunning;
            }
            catch
            {
                this.InstanceState = EmulatorInstanceState.InstanceError;
            }
            return this.InstanceState;
        }

        public virtual EmulatorInstanceState PauseGame()
        {
            this.InstanceState = EmulatorInstanceState.GamePaused;
            return this.InstanceState;
        }

        public virtual EmulatorInstanceState ShutdownGame()
        {
            try
            {
                this.runningEmulatorProcess.Close();
                this.InstanceState = EmulatorInstanceState.InstanceShutdown;
            }
            catch
            {
                this.InstanceState = EmulatorInstanceState.InstanceError;
            }
            return this.InstanceState;
        }

        public virtual EmulatorInstanceState CleanupInstance()
        {
            try
            {
                if (this.InstanceState != EmulatorInstanceState.InstanceShutdown &&
                    this.InstanceState != EmulatorInstanceState.InstanceError)
                {
                    this.ShutdownGame();
                }
                Directory.Delete(this.InstanceTemporaryDirectory, true);
                this.InstanceState = EmulatorInstanceState.InstanceClosed;
            }
            catch
            {
                this.InstanceState = EmulatorInstanceState.InstanceError;
            }
            return this.InstanceState;
        }

        public virtual EmulatorInstanceState SendCustomMessage(string message, out string response)
        {
            throw new NotImplementedException();
        }
    }
}
