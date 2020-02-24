using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;
using Snowflake.Model.Game;
using Snowflake.Romfile;

namespace Snowflake.Services
{
    /// <summary>
    /// Provides Stone platform and controller layout data
    /// </summary>
    public interface IStoneProvider
    {
        /// <summary>
        /// Gets the list of platforms loaded for this core service
        /// </summary>
        IReadOnlyDictionary<PlatformId, IPlatformInfo> Platforms { get; }

        /// <summary>
        /// Gets the list of controllers loaded for this core service
        /// </summary>
        IReadOnlyDictionary<ControllerId, IControllerLayout> Controllers { get; }

        /// <summary>
        /// Attempts to get a Stone mimetype from a file stream by reading the file headers provided, if
        /// the platform is known. 
        /// 
        /// If a mimetype can not be found through the inbuilt file signatures, it will rely on the extension fallback,
        /// which may be a filename or the known extension of the file. If extensionFallback is null or empty, 
        /// or is unable to be found in the platform, then the miemtype is unknown, and will return an empty string.
        /// 
        /// This method will preserve the stream position.
        /// </summary>
        /// <param name="knownPlatform">The platform that the provided ROM is for</param>
        /// <param name="romStream">A stream containing the ROM file.</param>
        /// <param name="extensionFallback">A fallback extension to look up for in case the stream analysis fails.</param>
        /// <returns>A Stone mimetype if the file format is known, or the empty string otherwise.</returns>
        string GetStoneMimetype(PlatformId knownPlatform, Stream romStream, string extensionFallback);

        /// <summary>
        /// Attempts to get a the filesignature for the given mimetype and ROM stream, preserving the
        /// position of the stream.
        /// </summary>
        /// <param name="mimetype">
        /// The mimetype of the ROM. Use <see cref="GetStoneMimetype(PlatformId, Stream, string)"/>
        /// to determine this.</param>
        /// <param name="romStream">A stream containing the ROM file.</param>
        /// <returns>A <see cref="IFileSignature"/> compatible with this mimetype and file, or null if none is found.</returns>
        IFileSignature? GetFileSignature(string mimetype, Stream romStream);

        /// <summary>
        /// Attempts to discover the mimetype from all known file signatures. No fallback is available for this method,
        /// so if the file does not match a known file signature, this method will return the empty string.
        /// 
        /// This method will preserve the stream position.
        /// </summary>
        /// <param name="romStream">A stream containing the ROM file.</param>
        /// <returns>A stone mimetype if the file format is known, or the empty string otherwise.</returns>
        string GetStoneMimetype(Stream romStream);

        /// <summary>
        /// If a filesignature is known for a mimetype, returns the filesignature object. Otherwise, returns null.
        /// </summary>
        /// <param name="mimetype">A stone mimetype</param>
        /// <returns>A list of possible signatures for the mimetype.</returns>
        IEnumerable<IFileSignature> GetSignatures(string mimetype);

        /// <summary>
        /// Gets the version of stone definitions loaded
        /// </summary>
        Version StoneVersion { get; }
    }
}
