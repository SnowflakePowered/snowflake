using System.ComponentModel.Composition;
using System.IO;
using Snowflake.Plugin;

namespace Snowflake.Identifier
{
    /// <summary>
    /// An identifier identifies a game from its file contents
    /// </summary>
    [InheritedExport(typeof(IIdentifier))]
    public interface IIdentifier : IBasePlugin
    {
        /// <summary>
        /// Identifies a game by the filename
        /// </summary>
        /// <param name="fileName">The filename of the game</param>
        /// <param name="platformId">The platform of the game</param>
        /// <returns>An identifier that a scraper can use to get results. Usually the title of the game.</returns>
        string IdentifyGame(string fileName, string platformId);
        /// <summary>
        /// Identifies a game by the file contents
        /// </summary>
        /// <param name="file">The file contents of the game ROM</param>
        /// <param name="platformId">The platform of the game</param>
        /// <returns>An identifier that a scraper can use to get results. Usually the title of the game.</returns>
        string IdentifyGame(FileStream file, string platformId);
        /// <summary>
        /// The metadata value type this identifier returns
        /// </summary>
        string IdentifiedValueType { get; }
    }
}
