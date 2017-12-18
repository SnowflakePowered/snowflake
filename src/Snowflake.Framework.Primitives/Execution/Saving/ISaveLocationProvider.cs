using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Game;

namespace Snowflake.Execution.Saving
{
    /// <summary>
    /// Provides and manages <see cref="ISaveLocation"s/>
    /// </summary>
    public interface ISaveLocationProvider
    {
        /// <summary>
        /// Provisions a new <see cref="ISaveLocation"/>
        /// </summary>
        /// <param name="gameRecord">The game to provision this location for.</param>
        /// <param name="saveType">The type of save this location contains.</param>
        /// <returns>The newly created save location</returns>
        Task<ISaveLocation> CreateSaveLocationAsync(IGameRecord gameRecord, string saveType);

        /// <summary>
        /// Updates the save location to the registery
        /// </summary>
        /// <param name="location">The location to update.</param>
        /// <returns>The new location.</returns>
        Task<ISaveLocation> UpdateSaveLocation(ISaveLocation location);

        /// <summary>
        /// Gets the save location with the given GUID.
        /// </summary>
        /// <param name="saveLocationGuid">The GUID of the desired save location.</param>
        /// <exception cref="FileNotFoundException">If the save location is not found.</exception>
        /// <returns>The save location.</returns>
        Task<ISaveLocation> GetSaveLocationAsync(Guid saveLocationGuid);

        /// <summary>
        /// Deletes the save location with the given GUID. Does nothing if the save location does not exist.
        /// </summary>
        /// <param name="saveLocationGuid">The GUID of the desired save location to delete.</param>
        void DeleteSaveLocation(Guid saveLocationGuid);

        /// <summary>
        /// Gets all save locations that were created for the given <see cref="IGameRecord"/>.
        /// It is suggested that the save locations be grouped by type and date created descendingly, but
        /// this is not required by implementations.
        /// </summary>
        /// <param name="gameRecord">The game associated with the save locations.</param>
        /// <returns>An enumerable of all the save locationa previously provisioned foe the given game.</returns>
        Task<IEnumerable<ISaveLocation>> GetSaveLocationsAsync(IGameRecord gameRecord);

        /// <summary>
        /// Gets all the save locations managed by this provider.
        /// </summary>
        /// <returns>All the save locations managed by this provider.</returns>
        Task<IEnumerable<ISaveLocation>> GetAllSaveLocationsAsync();
    }
}
