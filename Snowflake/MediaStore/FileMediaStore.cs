using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Snowflake.MediaStore
{
    public class FileMediaStore : IMediaStore
    {
        private Dictionary<string, IMediaStoreSection> sections;
        public IMediaStoreSection Images
        {
            get { return this.sections["Images"]; }
        }
        public IMediaStoreSection Audio
        {
            get { return this.sections["Audio"]; }
        }
        public IMediaStoreSection Video
        {
            get { return this.sections["Video"]; }
        }
        public IMediaStoreSection Resources
        {
            get { return this.sections["Resources"]; }
        }
        public string MediaStoreKey { get; private set; }
        public string MediaStoreRoot { get; private set; }
        public FileMediaStore(string mediastoreKey, string mediastoreRoot)
        {
           if (!Directory.Exists(mediastoreRoot)) Directory.CreateDirectory(mediastoreRoot);
           if (!Directory.Exists(Path.Combine(mediastoreRoot, mediastoreKey))) Directory.CreateDirectory(Path.Combine(mediastoreRoot, mediastoreKey));
           this.MediaStoreKey = mediastoreKey;
           this.MediaStoreRoot = mediastoreRoot;
           this.sections = new Dictionary<string, IMediaStoreSection>()
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
