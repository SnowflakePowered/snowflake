using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;

namespace Snowflake.Services
{
    /// <summary>
    /// Provides Stone platform and controller layout data
    /// </summary>
    public interface IStoneProvider
    {
        /// <summary>
        /// Gets the list of platforms loaded for this core service
        /// </summary>
        IDictionary<PlatformId, IPlatformInfo> Platforms { get; }

        /// <summary>
        /// Gets the list of controllers loaded for this core service
        /// </summary>
        IDictionary<ControllerId, IControllerLayout> Controllers { get; }

        /// <summary>
        /// Gets the version of stone definitions loaded
        /// </summary>
        Version StoneVersion { get; }
    }
}
