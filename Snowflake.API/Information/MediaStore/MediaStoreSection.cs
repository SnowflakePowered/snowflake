using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Snowflake.Information.MediaStore
{
    public class MediaStoreSection
    {
        public string SectionName { get; set; }
        public Dictionary<string, string> MediaStoreItems;
        private string mediaStoreRoot;
        public MediaStoreSection(string sectionName, MediaStore mediaStore)
        {
            this.mediaStoreRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "mediastores", mediaStore.MediaStoreKey, sectionName);
            this.SectionName = sectionName;
            this.MediaStoreItems = this.LoadMediaStore();
        }

        private Dictionary<string, string> LoadMediaStore()
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(mediaStoreRoot, ".mediastore")));
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }
        public void Add(string key, string fileName){
            File.Copy(fileName, Path.Combine(this.mediaStoreRoot, Path.GetFileName(fileName));
            this.MediaStoreItems.Add(key, Path.GetFileName(fileName));
            this.UpdateMediaStoreRecord();
        }
        public void Remove(string key){
            File.Delete(Path.Combine(this.mediaStoreRoot, Path.GetFileName(fileName));
            this.MediaStoreItems.Remove(key);
            this.UpdateMediaStoreRecord();
        }
        private void UpdateMediaStoreRecord(){
            string record = JsonConvert.SerializeObject(this.MediaStoreItems);
            File.WriteAllText(Path.Combine(mediaStoreRoot, ".mediastore"));
        }
    }
}
