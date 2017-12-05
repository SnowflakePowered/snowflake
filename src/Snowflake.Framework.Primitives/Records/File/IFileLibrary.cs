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
    public interface IFileLibrary : IRecordLibrary<IFileRecord>
    {
        /// <summary>
        /// Gets a file by its path
        /// </summary>
        /// <param name="filePath">The path to look for</param>
        /// <returns>The file path</returns>
        IFileRecord Get(string filePath);
    }
}
