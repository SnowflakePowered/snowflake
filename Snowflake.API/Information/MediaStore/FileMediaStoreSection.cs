using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Snowflake.Information.MediaStore
{
    public sealed class FileMediaStoreSection : IMediaStoreSection
    {
        public string SectionName { get; set; }
        public Dictionary<string, string> MediaStoreItems { get; private set; }
        private string mediaStoreRoot;
        public FileMediaStoreSection(string sectionName, FileMediaStore mediaStore)
        {
            this.mediaStoreRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "mediastores", mediaStore.MediaStoreKey, sectionName);
            this.SectionName = sectionName;
            this.MediaStoreItems = this.LoadMediaStore();
        }

        private Dictionary<string, string> LoadMediaStore()
        {
            try
            {
                var record = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(mediaStoreRoot, ".mediastore")));
                return (record != null) ? record : new Dictionary<string, string>();
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
                this.MediaStoreItems.Add(key, Path.GetFileName(fileName));
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
                this.MediaStoreItems.Remove(key);
                this.UpdateMediaStoreRecord();
            }
            catch
            {
                throw;
            }
        }

        private void UpdateMediaStoreRecord(){
            string record = JsonConvert.SerializeObject(this.MediaStoreItems);
            File.WriteAllText(Path.Combine(mediaStoreRoot, ".mediastore"), record);
        }
    }
}
