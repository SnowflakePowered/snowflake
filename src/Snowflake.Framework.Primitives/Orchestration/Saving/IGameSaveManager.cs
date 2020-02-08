using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving
{
    /// <summary>
    /// Provides management services for a specific game.
    /// </summary>
    public interface IGameSaveManager
    {
        /// <summary>
        /// Gets the profile specified by the GUID, or null otherwise.
        /// </summary>
        /// <param name="guid">The unique profile GUID.</param>
        /// <returns>The save profile if it exists, null if it does not.</returns>
        ISaveProfile? GetProfile(Guid guid);

        /// <summary>
        /// Deletes the specified save profile.
        /// </summary>
        /// <param name="guid">The unique profile GUID.</param>
        void DeleteProfile(Guid guid);

        /// <summary>
        /// Creates a profile for a game.
        /// </summary>
        /// <returns>A new, empty save profile.</returns>
        /// <param name="managementStrategy">The management strategy to use to manage save history.</param>
        /// <param name="profileName">The name of the save profile to use.</param>
        /// <param name="saveType">The type of the same this profile is of.</param>
        ISaveProfile CreateProfile(string profileName, string saveType, SaveManagementStrategy managementStrategy);
    }
}
