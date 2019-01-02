using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Snowflake.Execution.Saving;

namespace Snowflake.Support.Execution
{
    public class SaveLocation : ISaveLocation
    {
        internal SaveLocation(Guid gameRecordGuid, string saveFormat,
            DirectoryInfo locationRoot,
            Guid locationGuid, DateTimeOffset lastModified)
        {
            this.RecordGuid = gameRecordGuid;
            this.SaveFormat = saveFormat;
            this.LocationRoot = locationRoot;
            this.LocationGuid = locationGuid;
            this.LastModified = lastModified;
        }

        public Guid LocationGuid { get; }

        public DirectoryInfo LocationRoot { get; }

        public Guid RecordGuid { get; }

        public string SaveFormat { get; }

        public DateTimeOffset LastModified { get; private set; }

        public IEnumerable<FileInfo> PersistFrom(DirectoryInfo emulatorSaveDirectory)
        {
            this.LastModified = DateTime.UtcNow;
            return SaveLocation.CopyAll(emulatorSaveDirectory, this.LocationRoot).ToList();
        }

        public IEnumerable<FileInfo> RetrieveTo(DirectoryInfo emulatorSaveDirectory)
        {
            return SaveLocation.CopyAll(this.LocationRoot, emulatorSaveDirectory).ToList();
        }

        private static IEnumerable<FileInfo> CopyAll(DirectoryInfo from, DirectoryInfo to)
        {
            foreach (var fileInfo in from.GetFiles("*.*", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(fileInfo.FullName);
                if (fileName == SaveLocationProvider.ManifestFileName)
                {
                    continue;
                }

                yield return fileInfo.CopyTo(Path.Combine(to.FullName, fileName), true);
            }
        }
    }
}
