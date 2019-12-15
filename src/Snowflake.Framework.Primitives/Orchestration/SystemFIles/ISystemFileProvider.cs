using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Filesystem;
using Snowflake.Model.Game;

namespace Snowflake.Orchestration.SystemFiles
{
    /// <summary>
    /// Provides System and BIOS files for emulator adapters.
    /// </summary>
    public interface ISystemFileProvider
    {
        /// <summary>
        /// Retrieves the <see cref="IDirectory"/> containing the system files for the specified platform.
        /// 
        /// Todo: expose as IReadOnlyDirectory
        /// </summary>
        /// <param name="biosPlatform">The Stone PlatformID for the requested system files.</param>
        /// <returns></returns>
        IDirectory GetSystemFileDirectory(PlatformId biosPlatform);

        /// <summary>
        /// Retrieves the list of system files not available for use within the system file directory,
        /// according to the platform's Stone specification.
        /// 
        /// This does only a cursory filename check and not a deeper MD5 verification of the file.
        /// </summary>
        /// <param name="biosPlatform">The Stone PlatformID for the requested system file.</param>
        /// <returns>Ahe list of system files not available for use within the system file directory,
        /// according to the platform's Stone specification.</returns>
        IEnumerable<IBiosFile> GetMissingSystemFiles(PlatformId biosPlatform);

        /// <summary>
        /// Retrieves the <see cref="IFile"/> for the specified system file with the provided MD5 hash
        /// </summary>
        /// <param name="md5Hash">
        /// The MD5 hash of the system file to request.
        /// </param>
        /// <param name="platformId">The platform ID of the system file to request.</param>
        /// <returns>The <see cref="IFile"/> entry for the specified file if it exists,
        /// otherwise returns null.
        /// </returns>
        IFile? GetSystemFileByMd5Hash(PlatformId platformId, string md5Hash);
 
        /// <summary>
        /// Retrieves the <see cref="IFile"/> for the specified system file with the provided file name.
        /// </summary>
        /// <param name="name">
        /// The name of the system file to request.</param>
        /// <returns>The <see cref="IFile"/> entry for the specified file if it exists,
        /// otherwise returns null.
        /// </returns>
        IFile? GetSystemFileByName(PlatformId platformId, string name);
    }
}
