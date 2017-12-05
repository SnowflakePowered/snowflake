using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;
namespace Snowflake.Scraper
{
    /// <summary>
    /// Implements a pipeline to gather information from various sources, including a
    /// game database, scrape and media provider plugins, and the file itself
    /// through file signature matching.
    /// </summary>
    public interface IScrapeEngine
    {
        /// <summary>
        /// Gets as much information as possible about a single file
        /// </summary>
        /// <param name="romfile"></param>
        /// <returns></returns>
        IFileRecord GetFileInformation(string romfile);

        /// <summary>
        /// Assumes the file given is a game and gathers information about such game
        /// </summary>
        /// <param name="fileRecord"></param>
        /// <returns></returns>
        IGameRecord GetGameRecordFromFile(IFileRecord fileRecord);
    }
}
