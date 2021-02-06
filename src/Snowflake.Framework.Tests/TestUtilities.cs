using Snowflake.Filesystem;
using System;
using System.IO;
using System.Reflection;
using Zio;
using Zio.FileSystems;

namespace Snowflake.Tests
{
    internal static class TestUtilities
    {
        /// <summary>
        /// Gets a TestResource from the TestResources folder
        /// </summary>
        /// <param name="resourceName">The filename of the resource</param>
        /// <returns>The TestResource<</returns>
        internal static Stream GetResource(string resourceName)
        {
            var assembly = typeof(TestUtilities).GetTypeInfo().Assembly;
            var typeNamespace = typeof(TestUtilities).Namespace.Split(".")[0];
            return assembly.GetManifestResourceStream($"{typeNamespace}.TestResources.{resourceName}");
        }

        /// <summary>
        /// Gets a TestResource from the TestResources folder as a string
        /// </summary>
        /// <param name="resourceName">The filename of the resource</param>
        /// <returns>The TestResource</returns>
        internal static string GetStringResource(string resourceName)
        {
            using (Stream stream = TestUtilities.GetResource(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        internal static IDeletableDirectory GetTemporaryDirectory()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            return new Filesystem.Directory($"snowflake-test-{Guid.NewGuid()}", pfs, pfs.GetDirectoryEntry("/"));
        }

        internal static IDeletableDirectory GetTemporaryDirectory(string tag)
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            return new Filesystem.Directory($"snowflake-test-{tag}-{Guid.NewGuid()}", pfs, pfs.GetDirectoryEntry("/"));
        }
    }
}
