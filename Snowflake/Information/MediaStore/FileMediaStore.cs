using System;
using System.Collections.Generic;
using System.IO;

namespace Snowflake.Information.MediaStore
{
    [Obsolete("MediaStore has been superseded by GameMediaCache")]
    public class FileMediaStore : IMediaStore
    {
        private readonly Dictionary<string, IMediaStoreSection> sections;
        public IMediaStoreSection Images => this.sections["Images"];

        public IMediaStoreSection Audio => this.sections["Audio"];

        public IMediaStoreSection Video => this.sections["Video"];

        public IMediaStoreSection Resources => this.sections["Resources"];
        public string MediaStoreKey { get; }
        public string MediaStoreRoot { get; private set; }
        public FileMediaStore(string mediastoreKey, string mediastoreRoot)
        {
           if (!Directory.Exists(mediastoreRoot)) Directory.CreateDirectory(mediastoreRoot);
           if (!Directory.Exists(Path.Combine(mediastoreRoot, mediastoreKey))) Directory.CreateDirectory(Path.Combine(mediastoreRoot, mediastoreKey));
           this.MediaStoreKey = mediastoreKey;
           this.MediaStoreRoot = mediastoreRoot;
           this.sections = new Dictionary<string, IMediaStoreSection>
           {
               {"Images", new FileMediaStoreSection("Images", this) },
               {"Audio", new FileMediaStoreSection("Audio", this) },
               {"Video", new FileMediaStoreSection("Video", this) },
               {"Resources", new FileMediaStoreSection("Resources", this) }
           };
        }
        public FileMediaStore(string mediastoreKey) : this(mediastoreKey, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "mediastores")) { }
    }
}
