using Snowflake.Model.Game;
using Snowflake.Orchestration.Saving;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Extensibility
{
    /// <summary>
    /// A single instance of emulation for a specific <see cref="IGame"/>,
    /// with a specific combination of configuration, input, and initial save game state.
    /// 
    /// A <see cref="IGameEmulation"/> implementation is specific to the <see cref="IEmulatorOrchestrator"/>
    /// that produces it, and is responsible for handling the entire lifecycle of an emulation instance.
    /// </summary>
    public interface IGameEmulation
    {
        /// <summary>
        /// A list of <see cref="IEmulatedController"/> that representes the input devices that will be used
        /// in this emulation instance.
        /// </summary>
        IEnumerable<IEmulatedController> ControllerPorts { get; }

        /// <summary>
        /// The game that is being emulated in this instance.
        /// </summary>
        IGame Game { get; }

        /// <summary>
        /// The initial save game that should be copied into the working directory for this emulation instance.
        /// If this is null, then the implementation is responsible for creating a blank initial save game state
        /// in <see cref="RestoreSaveGame"/>.
        /// </summary>
        ISaveProfile SaveProfile { get; }

        /// <summary>
        /// The current state of the game emulation. It is absolutely impervious that
        /// implementations update this accordingly, or the <see cref="IGameEmulation"/> will 
        /// not function correctly.
        /// </summary>
        GameEmulationState EmulationState { get; }

        /// <summary>
        /// Compiles relevant configuration into the working directory of the emulation instance, ready for use.
        /// </summary>
        /// <returns>An asynchronous task that signals the completion of the compilation.</returns>
        Task CompileConfiguration();
       
        /// <summary>
        /// Persists the current state of the game's save information into a new immutable <see cref="ISaveGame"/>
        /// using the current <see cref="ISaveProfile"/>.
        /// 
        /// This can only be run if the game is not currently running, to avoid race conditions causing data corruption.
        /// </summary>
        /// <returns>A new <see cref="ISaveGame"/> with the current contents of the working directory save folder.</returns>
        Task<ISaveGame> PersistSaveGame();

        /// <summary>
        /// Restores the save into the working directory save folder, or otherwise prepares
        /// the folder for a new game if <see cref="SaveProfile"/> is null.
        /// <para>
        /// This method must be idempotent. If the save game restore succeeds, calling this method again must do
        /// nothing.
        /// </para>
        /// </summary>
        /// <returns>An asynchronous task that signals the completion of the restore.</returns>
        Task RestoreSaveGame();

        /// <summary>
        /// Run any misceallenous tasks required to prepare the working directory, such as copying BIOS files
        /// to the working directory if necessary.
        /// <para>
        /// This method must be idempotent. If the environment setup succeeds, calling this method again must do
        /// nothing.
        /// </para>
        /// </summary>
        /// <returns>An asynchronous task that signals the completion of the preparation.</returns>
        Task SetupEnvironment();

        /// <summary>
        /// Begin the emulation instancee (run the game).
        /// <para>
        /// This method must be idempotent. Once an emulation has started, this method must return the same cancellation token
        /// for the same process instance in future calls. If the emulation has stopped, this method must do nothing.
        /// </para>
        /// <para>
        /// Implementations of <see cref="IGameEmulation"/> may opt to refuse to start if setup is incomplete, and throw
        /// <see cref="InvalidOperationException"/> if this is the case. Implementations must keep track of the hooks themselves
        /// in a thread-safe manner if they opt to do so.
        /// </para>
        /// </summary>
        /// <exception cref="InvalidOperationException">If all hooks have not yet been called.</exception>
        /// <returns>A cancellation token that indicates when the emulation is halted by the user, or programmatically.</returns>
        CancellationToken StartEmulation();

        /// <summary>
        /// As safely as possible, halt the currently running emulation instance forever.
        /// Instead of calling this directly, consider calling <see cref="DisposeAsync"/> to dispose of the
        /// IGameEmulation instance.
        /// <para>
        /// This method must be idempotent. Once an emulation has stopped, it can never be restarted, and calling this
        /// method again must do nothing. If the emulation has yet to be started, this method must do nothing.
        /// </para>
        /// </summary>
        /// <returns>An asynchronous task that signals the completion of the instance halt.</returns>
        Task StopEmulation();

        /// <summary>
        /// Implements <see cref="IAsyncDisposable"/>.
        /// When disposed, immediately stop the currently running emulation instance, if any,
        /// persist the save, and then clean up the working directory if any.
        /// <para>
        /// This method must be idempotent. Repeated calls must be ignored.
        /// </para>
        /// </summary>
        ValueTask DisposeAsync();
    }
}