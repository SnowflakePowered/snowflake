using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zio;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Abstraction of GUID provisioning for <see cref="Directory"/>
    /// </summary>
    internal interface IFileGuidProvider
    {
        /// <summary>
        /// Tries to get a GUID for the file.
        /// </summary>
        /// <param name="rawInfo">The raw <see cref="FileInfo"/> referring to the file on disk.</param>
        /// <param name="guid">The result GUID if it exists, or <see cref="Guid.Empty"/> if it does not.</param>
        /// <returns><see langword="true"/> if a GUID exists and is returned, <see langword="false"/> otherwise.</returns>
        public bool TryGetGuid(FileInfo rawInfo, out Guid guid);

        /// <summary>
        /// Sets a GUID for a file. 
        /// </summary>
        /// <param name="rawInfo">The raw <see cref="FileInfo"/> referring to the file on disk.</param>
        /// <param name="guid">The result GUID if it exists, or <see cref="Guid.Empty"/> if it does not.</param>
        public void SetGuid(FileInfo rawInfo, Guid guid);
    }
}
