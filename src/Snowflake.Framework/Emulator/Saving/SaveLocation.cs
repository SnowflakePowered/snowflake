using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Records.Game;

namespace Snowflake.Emulator.Saving
{
    public class SaveLocation : ISaveLocation
    {
        public SaveLocation(IGameRecord gameRecord, string saveFormat, DirectoryInfo providerRoot)
        {
            this.GameRecord = gameRecord;
            this.SaveFormat = saveFormat;
            this.LocationGuid = Guid.NewGuid();
            this.LocationRoot = providerRoot.CreateSubdirectory(this.LocationGuid.ToString());
        }

        internal SaveLocation(IGameRecord gameRecord, string saveFormat, DirectoryInfo locationRoot, Guid locationGuid)
        {
            this.GameRecord = gameRecord;
            this.SaveFormat = saveFormat;
            this.LocationRoot = locationRoot;
            this.LocationGuid = locationGuid;
        }

        public Guid LocationGuid { get; }

        public DirectoryInfo LocationRoot { get; }

        public IGameRecord GameRecord { get; }

        public string SaveFormat { get; }

        public DateTimeOffset LastModified { get; }

        public IEnumerable<FileInfo> PersistFrom(DirectoryInfo emulatorSaveDirectory)
        {
            var list = new List<FileInfo>();
            foreach (var fileInfo in emulatorSaveDirectory.GetFiles("*.*", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(fileInfo.FullName);
                list.Add(fileInfo.CopyTo(Path.Combine(this.LocationRoot.FullName, fileName), true));
            }

            return list;
        }

        public IEnumerable<FileInfo> RetrieveTo(DirectoryInfo emulatorSaveDirectory)
        {

            var list = new List<FileInfo>();
          

            return list;
        }

        private static IEnumerable<FileInfo> CopyAll(DirectoryInfo from, DirectoryInfo to)
        {
            foreach (var fileInfo in from.GetFiles("*.*", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(fileInfo.FullName);
                yield return fileInfo.CopyTo(Path.Combine(to.FullName, fileName), true);
            }

        }
    }
}
