using Newtonsoft.Json;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving
{
    public sealed class SaveGameManager
    {
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
            .FirstOrDefault(f => f.Name.StartsWith(guid.ToString()));
            if (savedir == null) return null;
            return this.GetSave(savedir);
        }

        private ISaveGame? GetSave(IDirectory directory)
        {
            var savemanifest = directory.OpenFile(".savemanifest");
            if (!savemanifest.Created) return null;
            var saveContents = directory.OpenDirectory("savecontents");
            var details = JsonConvert.DeserializeObject<SaveGameDetails>(savemanifest.ReadAllText());
            return new SaveGame(saveContents.AsReadOnly(),
                details.CreatedTimestamp, details.Guid, details.Type, details.Tags);
        }

        public ISaveGame CreateSave(string type,
            Action<IDirectory> factory) => this.CreateSave(type, Enumerable.Empty<string>(), factory);

        public ISaveGame CreateSave(string type, IEnumerable<string> tags, Action<IDirectory> factory)
        {
            var details = new SaveGameDetails()
            {
                CreatedTimestamp = DateTimeOffset.UtcNow,
                Type = type,
                Guid = Guid.NewGuid(),
                Tags = tags,
            };

            var saveDirectory = this.SaveDirectory
                .OpenDirectory($"{details.Guid}-{type}-{details.CreatedTimestamp.ToString("yyyy-MM-dd.HH-mm-ss")}");
            string manifestData = JsonConvert.SerializeObject(details);
            saveDirectory.OpenFile(".savemanifest").WriteAllText(manifestData);

            var saveContents = saveDirectory.OpenDirectory("savecontents");
            factory(saveContents);
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
