using Newtonsoft.Json;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving
{
    public sealed class SaveGameManager : ISaveGameManager
    {
        private static readonly string DateFormat = "yyyy-MM-dd.HH-mm-ss";
        private static readonly string ManifestName = ".savemanifest";
        private static readonly string ContentsDirectoryName = "savecontents";

        private IDirectory SaveDirectory { get; }

        internal SaveGameManager(IDirectory saveDirectory)
        {
            this.SaveDirectory = saveDirectory;
        }

        public IEnumerable<ISaveGame> GetSaves()
        {
            foreach (var savedir in this.SaveDirectory.EnumerateDirectories())
            {
                var save = this.GetSave(savedir);
                if (save != null) yield return save;
            }
        }

        public ISaveGame? GetSave(Guid guid)
        {
            var savedir = this.SaveDirectory.EnumerateDirectories()
                .FirstOrDefault(f => f.Name.EndsWith(guid.ToString()));
            if (savedir == null) return null;
            return this.GetSave(savedir);
        }

        public ISaveGame? GetLatestSave(string type)
        {
            var savedirs = from f in this.SaveDirectory.EnumerateDirectories()
                            where f.Name.StartsWith(type)
                            let time = DateTimeOffset.TryParseExact(f.Name.Substring(type.Length + 1, DateFormat.Length),
                                                DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal,
                                                out var date) ? new DateTimeOffset?(date) : null
                            where time != null
                            orderby time.Value descending
                            select f;


            var savedir = savedirs.FirstOrDefault();
            if (savedir == null) return null;

            return this.GetSave(savedir);
        }

        private ISaveGame? GetSave(IDirectory directory)
        {
            var savemanifest = directory.OpenFile(ManifestName);
            if (!savemanifest.Created) return null;
            var saveContents = directory.OpenDirectory(ContentsDirectoryName);
            var details = JsonConvert.DeserializeObject<SaveGameDetails>(savemanifest.ReadAllText());
            return new SaveGame(saveContents.AsReadOnly(),
                details.CreatedTimestamp, details.Guid, details.Type, details.Tags);
        }

        public Task<ISaveGame> CreateSave(string type,
            Func<IDirectory, Task> factory) => this.CreateSave(type, Enumerable.Empty<string>(), factory);

        public async Task<ISaveGame> CreateSave(string type, IEnumerable<string> tags, Func<IDirectory, Task> factory)
        {
            var details = new SaveGameDetails()
            {
                CreatedTimestamp = DateTimeOffset.UtcNow,
                Type = type,
                Guid = Guid.NewGuid(),
                Tags = tags,
            };

            var saveDirectory = this.SaveDirectory
                .OpenDirectory($"{type}-{details.CreatedTimestamp.ToString(DateFormat)}-{details.Guid}");
            string manifestData = JsonConvert.SerializeObject(details);
            saveDirectory.OpenFile(ManifestName).WriteAllText(manifestData);

            var saveContents = saveDirectory.OpenDirectory(ContentsDirectoryName);
            await factory(saveContents);
            return new SaveGame(saveContents.AsReadOnly(),
                details.CreatedTimestamp, details.Guid, details.Type, details.Tags);
        }

        private class SaveGameDetails
        {
            public DateTimeOffset CreatedTimestamp { get; set; }
            public Guid Guid { get; set; }
            public string Type { get; set; } = "";
            public IEnumerable<string> Tags { get; set; } = new string[] { };
        }
    }
}
