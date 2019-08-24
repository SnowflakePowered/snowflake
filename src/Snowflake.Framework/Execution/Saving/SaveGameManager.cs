using Newtonsoft.Json;
using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Execution.Saving
{
    public class SaveGameManager
    {
        private IDirectory SaveDirectory { get; }

        public SaveGameManager(IDirectory saveDirectory)
        {
            this.SaveDirectory = saveDirectory;
        }

        public SaveGame CreateSave(string type,
            Action<IDirectory> factory) => this.CreateSave(type, Enumerable.Empty<string>(), factory);

        public SaveGame CreateSave(string type, IEnumerable<string> tags, Action<IDirectory> factory)
        {
            var details = new SaveGameDetails()
            {
                CreatedTimestamp = DateTimeOffset.UtcNow,
                Type = type,
                Guid = Guid.NewGuid(),
                Tags = tags,
            };

            var saveDirectory = this.SaveDirectory
                .OpenDirectory($"{type}-{details.CreatedTimestamp.ToString("yyyy-MM-dd.HH-mm-ss")}");
            string manifestData = JsonConvert.SerializeObject(details);
            saveDirectory.OpenFile(".savemanifest").WriteAllText(manifestData);

            var saveContents = saveDirectory.OpenDirectory("savecontents");
            factory(saveContents);
            return new SaveGame(saveDirectory.AsReadOnly(), 
                details.CreatedTimestamp, details.Guid, details.Type!, details.Tags!);
        }

        private class SaveGameDetails
        {
            public DateTimeOffset CreatedTimestamp { get; set; }
            public Guid Guid { get; set; }
            public string? Type { get; set; }
            public IEnumerable<string>? Tags { get; set; }
        }
    }
}
