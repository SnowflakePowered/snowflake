using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Game;

namespace Snowflake.Records.File
{
    /// <summary>
    /// A library to store file records that are related
    /// </summary>
    public interface IFileLibrary : ILibrary<IFileRecord>
    {

        /// <summary>
        /// Get a list of all files relating to a certain game, with
        /// executable application/romfile-* file types first.
        /// </summary>
        /// <returns>A list of all games in the database</returns>
        IEnumerable<IFileRecord> GetFilesForGame(IGameRecord game);

        /// <summary>
        /// Get a list of all files relating to a certain game, with
        /// executable application/romfile-* file types first.
        /// </summary>
        /// <returns>A list of all games in the database</returns>
        IEnumerable<IFileRecord> GetFilesForGame(Guid game);

        /// <summary>
        /// Gets a file by it's path
        /// </summary>
        /// <param name="filePath">The path to look for</param>
        /// <returns>The file path</returns>
        IFileRecord GetFile(string filePath);

        /// <summary>
        /// Gets a file by it's record.
        /// </summary>
        /// <param name="recordGuid"></param>
        /// <returns></returns>
        IFileRecord GetFile(Guid recordGuid);
    
    }
}
