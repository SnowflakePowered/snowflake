using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator
{
    /// <summary>
    /// Represents the state of an instance.
    /// </summary>
    public enum EmulatorInstanceState
    {
        /// <summary>
        /// A game is running in this instance.
        /// </summary>
        GameRunning,
        /// <summary>
        /// A game is paused in this instance.
        /// </summary>
        GamePaused,
        /// <summary>
        /// The instance has been shutdown.
        /// </summary>
        InstanceShutdown,
        /// <summary>
        /// The instance has yet to be started.
        /// </summary>
        InstanceNotStarted,
        /// <summary>
        /// The instance is initiating startup.
        /// </summary>
        InstanceInitiating,
        /// <summary>
        /// The instance is inactive (paused and minimized).
        /// </summary>
        InstanceInactive,
        /// <summary>
        /// The instance is closed and can not be restarted
        /// </summary>
        InstanceClosed,
        /// <summary>
        /// The instance encountered an error
        /// </summary>
        InstanceError

    }
}
