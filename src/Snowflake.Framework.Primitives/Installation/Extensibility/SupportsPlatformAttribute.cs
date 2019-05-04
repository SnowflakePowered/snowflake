using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Model.Game;

namespace Snowflake.Installation.Extensibility
{
    /// <summary>
    /// Configuration attribute to indicate platform support for <see cref="IGameInstaller"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SupportedPlatformAttribute : Attribute
    {
        /// <summary>
        /// Constructor for <see cref="SupportedPlatformAttribute"/> with the given platform ID.
        /// </summary>
        /// <param name="platformId"></param>
        public SupportedPlatformAttribute(string platformId)
        {
            this.PlatformId = platformId;
        }

        /// <summary>
        /// The Platform ID
        /// </summary>
        public PlatformId PlatformId { get; }
    }
}
