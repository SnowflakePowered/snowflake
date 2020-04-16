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
        /// <param name="profileId">The unique profile GUID.</param>
        /// <returns>The save profile if it exists, null if it does not.</returns>
        ISaveProfile? GetProfile(Guid profileId);

        /// <summary>
        /// Gets all save profiles for this game.
        /// </summary>
        /// <returns>All save profiles for this game.</returns>
        IEnumerable<ISaveProfile> GetProfiles();

        /// <summary>
        /// Gets all save profiles with the provided save type.
        /// </summary>
        /// <returns>All save profiles with the provided save type.</returns>
        /// <param name="saveType">The type of the save.</param>
        IEnumerable<ISaveProfile> GetProfiles(string saveType);

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
