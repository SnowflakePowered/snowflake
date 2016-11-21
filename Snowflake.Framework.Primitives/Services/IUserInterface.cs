using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Services
{
    /// <summary>
    /// Represents a user interface that is started by the Snowflake shell.
    /// There can only be one interface at a time.
    /// </summary>
    public interface IUserInterface
    {
        /// <summary>
        /// Starts the user interface
        /// </summary>
        void StartUserInterface(params string[] args);
        /// <summary>
        /// Stops the user interface
        /// </summary>
        void StopUserInterface(params string[] args);
        
    }
}
