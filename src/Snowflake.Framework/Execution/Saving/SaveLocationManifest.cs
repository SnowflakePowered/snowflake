using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Execution.Saving
{
    /// <summary>
    /// Represents a JSON manifest for savelocations.
    /// </summary>
    internal class SaveLocationManifest
    {
        public Guid RecordGuid { get; set; }
        public string SaveFormat { get; set; }
        public string Directory { get; set; }
        public Guid LocationGuid { get; set; }
        public DateTimeOffset LastModified { get; set; }
    }

    internal static class SaveLocationManifestExtensions
    {
        public static ISaveLocation ToSaveLocation (this SaveLocationManifest manifest)
        {
            return new SaveLocation(manifest.RecordGuid,
                manifest.SaveFormat,
                new DirectoryInfo(manifest.Directory),
                manifest.LocationGuid,
                manifest.LastModified);
        }
    }

    internal static class SaveLocationExtensions
    {
        public static SaveLocationManifest ToManifest(this ISaveLocation location)
        {
            return new SaveLocationManifest()
            {
                LocationGuid = location.LocationGuid,
                Directory = location.LocationRoot.FullName,
                LastModified = location.LastModified,
                RecordGuid = location.RecordGuid,
                SaveFormat = location.SaveFormat,
            };
        }
    }
}
