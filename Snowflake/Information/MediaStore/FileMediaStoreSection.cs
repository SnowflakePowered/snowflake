using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections.ObjectModel;
using Snowflake.Extensions;
namespace Snowflake.Information.MediaStore
{
    [Obsolete("MediaStore has been superseded by GameMediaCache")]
    public sealed class FileMediaStoreSection : IMediaStoreSection
    {
        public string SectionName { get; set; }
        public IReadOnlyDictionary<string, string> MediaStoreItems => this.mediaStoreItems.AsReadOnly();
        private IDictionary<string, string> mediaStoreItems;

        private string mediaStoreRoot;
        public FileMediaStoreSection(string sectionName, FileMediaStore mediaStore)
        {
            this.mediaStoreRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "mediastores", mediaStore.MediaStoreKey, sectionName);
            this.SectionName = sectionName;
            this.mediaStoreItems = this.LoadMediaStore();
        }

        private Dictionary<string, string> LoadMediaStore()
        {
            try
            {
                var record = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(this.mediaStoreRoot, ".mediastore")));
                return record ?? new Dictionary<string, string>();
            }
            catch
            {
                if (!Directory.Exists(this.mediaStoreRoot))
                {
                    Directory.CreateDirectory(this.mediaStoreRoot);
                    File.Create(Path.Combine(this.mediaStoreRoot, ".mediastore")).Close();
                }

                return new Dictionary<string, string>();
            }
        }

        public void Add(string key, string fileName){
            try
            {
                File.Copy(fileName, Path.Combine(this.mediaStoreRoot, Path.GetFileName(fileName)), true);
                this.mediaStoreItems.Add(key, Path.GetFileName(fileName));
                this.UpdateMediaStoreRecord();
            }
            catch
            {
                throw;
            }
        }
        public void Remove(string key){
            try
            {
                File.Delete(Path.Combine(this.mediaStoreRoot, this.MediaStoreItems[key]));
                this.mediaStoreItems.Remove(key);
                this.UpdateMediaStoreRecord();
            }
            catch
            {
                throw;
            }
        }

        private void UpdateMediaStoreRecord(){
            string record = JsonConvert.SerializeObject(this.MediaStoreItems);
            File.WriteAllText(Path.Combine(this.mediaStoreRoot, ".mediastore"), record);
        }

        public string this[string key]
        {
            get
            {
                return this.MediaStoreItems[key];
            }
            set
            {
                this.Add(key, value);
            }
        }
    }
}
