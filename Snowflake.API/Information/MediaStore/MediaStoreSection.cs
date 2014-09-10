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
        }

        private void LoadMediaStore()
        {
            try
            {
                JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(mediaStoreRoot, ".mediastore")));
            }
            catch
            {
                return;
            }
        }
        
    }
}
