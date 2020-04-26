using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Extensibility
{
    /// <summary>
    /// Represents the various states a game emulation is allowed to be in.
    /// </summary>
    public enum GameEmulationState
    {
        /// <summary>
        /// This game emulation requires environment setup.
        /// 
        /// The following methods are legal to run.
        /// <list type="bullet">
        /// <item><see cref="IGameEmulation.SetupEnvironment"/></item>
        /// <item><see cref="IGameEmulation.DisposeAsync"/> to cancel the emulation and cleanup.</item>
        /// </list>
        /// </summary>
        RequiresSetupEnvironment,
        /// <summary>
        /// The game emulation requires configuration to be compiled.
        /// 
        /// The following methods are legal to run.
        /// <list type="bullet">
        /// <item><see cref="IGameEmulation.CompileConfiguration"/></item>
        /// <item><see cref="IGameEmulation.DisposeAsync"/> to cancel the emulation and cleanup.</item>
        /// </list>
        /// </summary>
        RequiresCompileConfiguration,
        /// <summary>
        /// The game emulation requires the save game to be restored.
        /// 
        /// The following methods are legal to run.
        /// <list type="bullet">
        /// <item><see cref="IGameEmulation.RestoreSaveGame"/></item>
        /// <item><see cref="IGameEmulation.DisposeAsync"/> to cancel the emulation and cleanup.</item>
        /// </list>
        /// </summary>
        RequiresRestoreSaveGame,
        /// <summary>
        /// The game emulation can be started.
        /// 
        /// The following methods are legal to run.
        /// <list type="bullet">
        /// <item><see cref="IGameEmulation.PersistSaveGame"/></item>
        /// <item><see cref="IGameEmulation.StartEmulation"/></item>
        /// <item><see cref="IGameEmulation.DisposeAsync"/> to cancel the emulation and cleanup.</item>
        /// </list>
        /// </summary>
        CanStartEmulation,
        /// <summary>
        /// The game emulation is running, and can be stopped.
        /// The following methods are legal to run.
        /// <list type="bullet">
        /// <item><see cref="IGameEmulation.StartEmulation"/> to fetch the cancellation token for the game.</item>
        /// <item><see cref="IGameEmulation.DisposeAsync"/> to stop the emulation and cleanup.</item>
        /// </list>
        /// </summary>
        CanStopEmulation,
        /// <summary>
        /// The game emulation has finished running, and now must be disposed.
        /// <list type="bullet">
        /// <item><see cref="IGameEmulation.PersistSaveGame"/> to persist the current state of the save.</item>
        /// <item><see cref="IGameEmulation.DisposeAsync"/> to cleanup the emulation.</item>
        /// </list>
        /// </summary>
        RequiresDispose
    }
}
