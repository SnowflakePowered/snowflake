using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Snowflake.Information.MediaStore
{
    public class MediaStore
    {
        private Dictionary<string, MediaStoreSection> sections;
        public MediaStoreSection Images
        {
            get { return this.sections["Images"]; }
        }
        public MediaStoreSection Audio
        {
            get { return this.sections["Audio"]; }
        }
        public MediaStoreSection Video
        {
            get { return this.sections["Video"]; }
        }
        public MediaStoreSection Resources
        {
            get { return this.sections["Resources"]; }
        }
        public string MediaStoreKey { get; set; }

        public MediaStore(string mediastoreKey, string mediastoreRoot)
        {
           if (!Directory.Exists(mediastoreRoot)) Directory.CreateDirectory(mediastoreRoot);
           if (!Directory.Exists(Path.Combine(mediastoreRoot, mediastoreKey))) Directory.CreateDirectory(Path.Combine(mediastoreRoot, mediastoreKey));
           
        }
        public MediaStore(string mediastoreKey) : this(mediastoreKey, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "mediastores")) { }
    }
}
