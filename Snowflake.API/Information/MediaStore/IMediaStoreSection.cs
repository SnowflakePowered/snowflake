using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Information.MediaStore
{
    public interface IMediaStoreSection
    {
        string SectionName { get; set; }
        Dictionary<string, string> MediaStoreItems { get; }
        void Add(string key, string value);
        void Remove(string key);
    }
}
